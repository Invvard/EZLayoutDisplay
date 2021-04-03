using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using InvvardDev.EZLayoutDisplay.Desktop.Helper;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Properties;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;
using Newtonsoft.Json;
using NLog;

namespace InvvardDev.EZLayoutDisplay.Desktop.Service.Implementation
{
    public class LayoutService : ILayoutService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Supported keyboards and their layout definition resources.
        /// </summary>
        private static readonly Dictionary<string, byte[]> LayoutDefinitions = new Dictionary<string, byte[]>()
        {
            { "ergodox-ez", Resources.layoutDefinition_ergodox },
            { "moonlander", Resources.layoutDefinition_moonlander }
        };

        private readonly string GetLayoutBody =
            "{{\"operationName\":\"getLayout\",\"variables\":{{\"hashId\":\"{0}\",\"geometry\":\"{1}\",\"revisionId\":\"{2}\"}},\"query\":\"query getLayout($hashId: String!, $revisionId: String!, $geometry: String) {{\\n Layout(hashId: $hashId, geometry: $geometry, revisionId: $revisionId) {{\\n ...LayoutData\\n }}\\n}}\\n\\nfragment LayoutData on Layout {{\\n geometry\\n hashId\\n title\\n tags {{\\n id\\n hashId\\n name\\n }}\\n revision {{\\n ...RevisionData\\n }}\\n}}\\n\\nfragment RevisionData on Revision {{\\n hashId\\n model\\n title\\n swatch\\n hexUrl\\n zipUrl\\n qmkVersion\\n qmkUptodate\\n config\\n layers {{\\n hashId\\n keys\\n position\\n title\\n color\\n}}\\n}}\\n\"}}";

        private readonly string GetLayoutInfoRequestBody =
            "{{\"operationName\":\"getLayout\",\"variables\":{{\"hashId\":\"{0}\",\"geometry\":\"{1}\",\"revisionId\":\"{2}\"}},\"query\":\"query getLayout($hashId: String!, $revisionId: String!, $geometry: String) {{\\n Layout(hashId: $hashId, geometry: $geometry, revisionId: $revisionId) {{\\n ...LayoutData\\n }}\\n}}\\n\\nfragment LayoutData on Layout {{\\n geometry\\n hashId\\n title\\n tags {{\\n id\\n hashId\\n name\\n }}\\n revision {{\\n hashId\\n title\\n hexUrl\\n model\\n zipUrl\\n  qmkVersion\\n  qmkUptodate\\n layers {{\\n position\\n title\\n }}\\n }}\\n}}\\n\"}}";

        private const string GetLayoutRequestUri = "https://oryx.zsa.io/graphql";

        #region ILayoutService implementation

        /// <inheritdoc />
        public async Task<ErgodoxLayout> GetLayoutInfo(string layoutHashId, string layoutRevisionId)
        {
            Logger.TraceMethod();
            Logger.DebugInputParam(nameof(layoutHashId), layoutHashId);
            Logger.DebugInputParam(nameof(layoutRevisionId), layoutRevisionId);

            ValidateLayoutHashId(layoutHashId);

            var info = await QueryData(layoutHashId, layoutRevisionId, GetLayoutInfoRequestBody);
            
            return info;
        }

        /// <inheritdoc />
        public async Task<ErgodoxLayout> GetErgodoxLayout(string layoutHashId, string layoutRevisionId)
        {
            Logger.TraceMethod();
            Logger.DebugInputParam(nameof(layoutHashId), layoutHashId);
            Logger.DebugInputParam(nameof(layoutRevisionId), layoutRevisionId);

            ValidateLayoutHashId(layoutHashId);

            var layout = await QueryData(layoutHashId, layoutRevisionId, GetLayoutBody);

            return layout;
        }

        /// <inheritdoc />
        public EZLayout PrepareEZLayout(ErgodoxLayout ergodoxLayout)
        {
            Logger.TraceMethod();

            var ezLayoutMaker = new EZLayoutMaker();
            EZLayout ezLayout = ezLayoutMaker.PrepareEZLayout(ergodoxLayout);

            return ezLayout;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<KeyTemplate>> GetLayoutTemplate(string geometry)
        {
            Logger.TraceMethod();

            IEnumerable<KeyTemplate> layoutTemplate = await ReadLayoutDefinition(geometry);

            return layoutTemplate;
        }

        /// <inheritdoc />
        public bool SupportsGeometry(string geometry)
        {
            return LayoutDefinitions.ContainsKey(geometry);
        }

        #endregion

        #region Private methods

        private async Task<ErgodoxLayout> QueryData(string layoutHashId, string layoutRevisionId, string graphQlQuery)
        {
            var requestBody = string.Format(graphQlQuery, layoutHashId, layoutRevisionId);

            var layout = await HttpClientCall(requestBody);

            if (layout?.LayoutRoot?.Layout == null)
            {
                Logger.Error("Layout {0} does not exist", layoutHashId);

                throw new ArgumentException(layoutHashId, $"Hash ID \"{layoutHashId}\" does not exist");
            }

            return layout.LayoutRoot.Layout;
        }

        private static void ValidateLayoutHashId(string layoutHashId)
        {
            if (!string.IsNullOrWhiteSpace(layoutHashId)) return;

            Logger.Error("Layout {0} was not found", layoutHashId);

            // ReSharper disable once LocalizableElement
            throw new ArgumentNullException(nameof(layoutHashId), $"Layout hash ID '{layoutHashId}' was not found.");
        }

        private async Task<DataRoot> HttpClientCall(string requestBody)
        {
            DataRoot layout;

            using (HttpClient client = new HttpClient())
            {
                Logger.Debug("Request body : {@body}", requestBody);

                var response = await client.PostAsync(GetLayoutRequestUri, new StringContent(requestBody, Encoding.UTF8, "application/json"));
                Logger.Debug("Response : {@response}", response);

                var result = await response.Content.ReadAsStringAsync();
                Logger.Debug("Content result : {@result}", result);

                layout = JsonConvert.DeserializeObject<DataRoot>(result);
                Logger.Debug("Deserialized layout : {@layout}", layout);
            }

            return layout;
        }

        private async Task<IEnumerable<KeyTemplate>> ReadLayoutDefinition(string geometry)
        {
            Logger.TraceMethod();

            byte[] layoutDefinitionJson;

            if (!LayoutDefinitions.TryGetValue(geometry, out layoutDefinitionJson))
            {
                layoutDefinitionJson = Resources.layoutDefinition;
            }

            if (layoutDefinitionJson.Length <= 0)
            {
                Logger.Warn("Layout definition is empty");

                return new List<KeyTemplate>();
            }

            var layoutTemplate = await Task.Run(() => {
                                                    var json = Encoding.Default.GetString(layoutDefinitionJson);

                                                    var layoutDefinition = JsonConvert.DeserializeObject<IEnumerable<KeyTemplate>>(json);

                                                    return layoutDefinition;
                                                });

            Logger.DebugOutputParam(nameof(layoutTemplate), layoutTemplate);

            return layoutTemplate;
        }

        #endregion
    }
}