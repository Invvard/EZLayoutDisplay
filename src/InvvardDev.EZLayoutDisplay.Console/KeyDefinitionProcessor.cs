using System.IO;

namespace InvvardDev.EZLayoutDisplay.Console
{
    public class KeyDefinitionProcessor
    {
        private const string KeyDefinitionJsFilename = "keyDefinitions.js";

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