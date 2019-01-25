using System.Collections.Generic;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Dictionary;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;
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
            Assert.Equal(330, dictionary.KeyDefinitions.Count);
        }
    }
}