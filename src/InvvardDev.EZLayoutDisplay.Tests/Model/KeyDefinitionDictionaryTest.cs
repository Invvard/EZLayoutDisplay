using InvvardDev.EZLayoutDisplay.Desktop.Model.Dictionary;
using Xunit;

namespace InvvardDev.EZLayoutDisplay.Tests.Model
{
    public class KeyDefinitionDictionaryTest
    {
        [ Fact ]
        public void InitializeKeyDefinitions()
        {
            // Arrange

            // Act
            var dictionary = new KeyDefinitionDictionary();

            // Assert
            Assert.NotEmpty(dictionary.KeyDefinitions);
            Assert.Equal(530, dictionary.KeyDefinitions.Count);
        }
    }
}