using System.IO;

namespace InvvardDev.EZLayoutDisplay.Console
{
    public class KeyDefinitionProcessor
    {
        private const string KeyDefinitionJsFilename = "keyDefinitions.js";
        private const string KeyDefinitionUrl = "https://configure.ergodox-ez.com/static/js/config/keyDefinitions.js";

        public KeyDefinitionProcessor() { }

        public void RunProcess()
        {
            if (!CheckKeyDefinitionJsExists())
            {
                return;
            }

            ParseJs();
        }

        private bool CheckKeyDefinitionJsExists()
        {
            var fileExist = File.Exists(KeyDefinitionJsFilename);

            return fileExist;
        }

        private void ParseJs()
        {
        }
    }
}