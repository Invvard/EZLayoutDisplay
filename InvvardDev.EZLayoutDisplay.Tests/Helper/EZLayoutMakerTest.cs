using System.Collections.Generic;
using InvvardDev.EZLayoutDisplay.Desktop.Helper;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using Xunit;
using Assert = Xunit.Assert;

namespace InvvardDev.EZLayoutDisplay.Tests.Helper
{
    public class EZLayoutMakerTest
    {
        [ Theory ]
        [ InlineData("expectedTitle", "expectedHashId") ]
        public void PrepareEZLayout_InitializeEZLayout(string expectedTitle, string expectedHashId)
        {
            // Arrange
            var revision = new Revision {
                                            Layers = new List<ErgodoxLayer>()
                                        };
            ErgodoxLayout ergodoxLayout = new ErgodoxLayout {
                                                                Title = expectedTitle,
                                                                HashId = expectedHashId,
                                                                Revisions = new List<Revision>()
                                                            };
            ergodoxLayout.Revisions.Add(revision);
            EZLayout ezLayoutResult;

            // Act
            var ezLayoutMaker = new EZLayoutMaker();
            ezLayoutResult = ezLayoutMaker.PrepareEZLayout(ergodoxLayout);

            // Assert
            Assert.Equal(expectedTitle, ezLayoutResult.Name);
            Assert.Equal(expectedHashId, ezLayoutResult.HashId);
        }
    }
}