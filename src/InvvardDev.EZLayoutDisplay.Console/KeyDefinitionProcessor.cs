using System;
using System.Net;
using System.Threading.Tasks;

namespace InvvardDev.EZLayoutDisplay.Console
{
    public class KeyDefinitionProcessor
    {
        private const string KeyDefinitionFileName = "keyDefinitions.js";
        private readonly Uri _keyDefinitionUrl;

        public KeyDefinitionProcessor()
        {
            _keyDefinitionUrl = new Uri($"https://configure.ergodox-ez.com/static/js/config/{KeyDefinitionFileName}");
        }

        public async Task StartProcess()
        {
            await DownloadKeyDefinitionScript();
        }

        private async Task<string> DownloadKeyDefinitionScript()
        {
            string keyDefinitionjs;

            using (var client = new WebClient())
            {
                keyDefinitionjs = await client.DownloadStringTaskAsync(_keyDefinitionUrl);
            }

            return keyDefinitionjs;
        }
    }
}