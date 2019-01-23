using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Implementation
{
    public class LayoutService : ILayoutService
    {
        private readonly string GetLayoutBody = "{{\"operationName\":\"getLayout\",\"variables\":{{\"hashId\":\"{0}\"}},\"query\":\"query getLayout($hashId: String!) {{\\n Layout(hashId: $hashId) {{\\n ...LayoutData\\n }}\\n}}\\n\\nfragment LayoutData on Layout {{\\n hashId\\n title\\n revisions {{\\n hashId\\n model\\n layers {{\\n hashId\\n keys\\n position\\n title\\n color\\n }}\\n}}\\n}}\\n\"}}";
        private const string GetLayoutRequestUri = "https://oryx.ergodox-ez.com/graphql";

        /// <inheritdoc />
        public async Task<ErgodoxLayout> GetErgodoxLayout(string layoutHashId)
        {
            if (string.IsNullOrWhiteSpace(layoutHashId))
            {
                throw new ArgumentNullException(nameof(layoutHashId), $"{nameof(layoutHashId)} should not be null or empty.");
            }

            DataRoot layout;
            using (HttpClient client = new HttpClient())
            {
                var body = string.Format(GetLayoutBody, layoutHashId);
                var response = await client.PostAsync(GetLayoutRequestUri, new StringContent(body, Encoding.UTF8, "application/json"));
                var result = await response.Content.ReadAsStringAsync();

                layout = JsonConvert.DeserializeObject<DataRoot>(result); ;
            }

            return layout.LayoutRoot.Layout;
        }
    }
}