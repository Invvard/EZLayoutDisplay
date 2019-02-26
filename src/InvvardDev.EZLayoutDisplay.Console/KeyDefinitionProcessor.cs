using System;
using System.CodeDom;
using System.Net;
using System.Threading.Tasks;
using Jint;

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

        public void RunProcess()
        {
            var keyDefinitionJs = DownloadKeyDefinitionScript();
            ParseJs(keyDefinitionJs);
        }

        private string DownloadKeyDefinitionScript()
        {
            string keyDefinitionjs;

            using (var client = new WebClient())
            {
                keyDefinitionjs = client.DownloadString(_keyDefinitionUrl);
            }

            return keyDefinitionjs;
        }

        private void ParseJs(string keyDefinitionJs)
        {
            var engine = new Engine();
            var keyCodes = engine.Execute(keyDefinitionJs).Execute("return keyCodes;");
            System.Console.WriteLine($"Key codes {keyCodes}");
            System.Console.ReadKey();
        }
    }
}