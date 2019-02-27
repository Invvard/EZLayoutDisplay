using System.IO;

namespace InvvardDev.EZLayoutDisplay.Console
{
    public class KeyDefinitionProcessor
    {
        private const string KeyDefinitionJsFilename = "keyDefinitions.js";
        private const string DictionariesPattern = "export const keyCategories = {(?<category>[\\S\\s]*)};[\\S\\s]*export const keyCodes = {(?<keyCodes>[\\S\\s]*)};";
        private const string CategoriesPattern = "\\s*(?<categoryName>[a-zA-Z]*)\\s?:\\s?\\\"(?<categoryLabel>[\\-a-zA-Z\\s]+)\\\",?";

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