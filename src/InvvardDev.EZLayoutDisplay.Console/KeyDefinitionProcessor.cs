using System.IO;

namespace InvvardDev.EZLayoutDisplay.Console
{
    public class KeyDefinitionProcessor
    {
        private const string KeyDefinitionJsFilename = "keyDefinitions.json";

        public KeyDefinitionProcessor() { }

        public void RunProcess()
        {
            if (!CheckKeyDefinitionJsExists())
            {
                return;
            }

            ParseJson();
        }

        private bool CheckKeyDefinitionJsExists()
        {
            var fileExist = File.Exists(KeyDefinitionJsFilename);

            return fileExist;
        }

        private void ParseJson()
        {
        }
    }
}