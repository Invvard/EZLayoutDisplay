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

        private readonly string GetLayoutBody =
            "{{\"operationName\":\"getLayout\",\"variables\":{{\"hashId\":\"{0}\"}},\"query\":\"query getLayout($hashId: String!) {{\\n Layout(hashId: $hashId) {{\\n ...LayoutData\\n }}\\n}}\\n\\nfragment LayoutData on Layout {{\\n hashId\\n title\\n revisions {{\\n hashId\\n model\\n layers {{\\n hashId\\n keys\\n position\\n title\\n color\\n }}\\n}}\\n}}\\n\"}}";

        private readonly string GetLayoutInfoRequestBody =
            "{{\"operationName\":\"getLayout\",\"variables\":{{\"hashId\":\"{0}\"}},\"query\":\"query getLayout($hashId: String!) {{\\n Layout(hashId: $hashId) {{\\n ...LayoutData\\n __typename\\n }}\\n}}\\n\\nfragment LayoutData on Layout {{\\n geometry\\n hashId\\n title\\n tags {{\\n id\\n hashId\\n name\\n }}\\n revisions {{\\n hexUrl\\n model\\n zipUrl\\n layers {{\\n position\\n title\\n }}\\n }}\\n __typename\\n}}\\n\"}}";

        private const string GetLayoutRequestUri = "https://oryx.ergodox-ez.com/graphql";

        #region ILayoutService implementation

        /// <inheritdoc />
        public async Task<ErgodoxLayout> GetLayoutInfo(string layoutHashId)
        {
            Logger.TraceMethod();
            Logger.DebugInputParam(nameof(layoutHashId), layoutHashId);

            ValidateLayoutHashId(layoutHashId);

            var requestBody = string.Format(GetLayoutInfoRequestBody, layoutHashId);

            var layout = await HttpClientCall(requestBody);

            if (layout?.LayoutRoot?.Layout == null)
            {
                Logger.Error("Layout {0} does not exist", layoutHashId);

                throw new ArgumentException(layoutHashId, $"Hash ID \"{layoutHashId}\" does not exist");
            }

            return layout.LayoutRoot.Layout;
        }

        /// <inheritdoc />
        public async Task<ErgodoxLayout> GetErgodoxLayout(string layoutHashId)
        {
            Logger.TraceMethod();
            Logger.DebugInputParam(nameof(layoutHashId), layoutHashId);

            ValidateLayoutHashId(layoutHashId);

            var requestBody = string.Format(GetLayoutBody, layoutHashId);

            var layout = await HttpClientCall(requestBody);

            if (layout?.LayoutRoot?.Layout == null)
            {
                Logger.Error("Layout {0} does not exist", layoutHashId);

                throw new ArgumentException(layoutHashId, $"Hash ID \"{layoutHashId}\" does not exist");
            }

            return layout.LayoutRoot.Layout;
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

        private static void ValidateLayoutHashId(string layoutHashId)
        {
            if (!string.IsNullOrWhiteSpace(layoutHashId)) return;

            Logger.Error("Layout {0} was not found", layoutHashId);

            // ReSharper disable once LocalizableElement
            throw new ArgumentNullException(nameof(layoutHashId), $"Layout hash ID '{layoutHashId}' was not found.");
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
        public async Task<IEnumerable<KeyTemplate>> GetLayoutTemplate()
        {
            Logger.TraceMethod();

            IEnumerable<KeyTemplate> layoutTemplate = await ReadLayoutDefinition();

            return layoutTemplate;
        }

        #endregion

        #region Private methods

        private async Task<IEnumerable<KeyTemplate>> ReadLayoutDefinition()
        {
            Logger.TraceMethod();

            if (Resources.layoutDefinition.Length <= 0)
            {
                Logger.Warn("Layout definition is empty");

                return new List<KeyTemplate>();
            }

            var layoutTemplate = await Task.Run(() => {
                                                    var json = Encoding.Default.GetString(Resources.layoutDefinition);

                                                    var layoutDefinition = JsonConvert.DeserializeObject<IEnumerable<KeyTemplate>>(json);

                                                    return layoutDefinition;
                                                });

            Logger.DebugOutputParam(nameof(layoutTemplate), layoutTemplate);

            return layoutTemplate;
        }

        #endregion
    }
}