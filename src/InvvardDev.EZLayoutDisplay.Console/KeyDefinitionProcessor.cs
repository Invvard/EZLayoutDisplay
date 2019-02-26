using System;
using System.Net;

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

        public void StartProcess()
        {
            DownloadKeyDefinitionScript();
        }

        private void DownloadKeyDefinitionScript()
        {
            using (var client = new WebClient())
            {
                client.DownloadFileAsync(_keyDefinitionUrl, KeyDefinitionFileName);
            }
        }
    }
}