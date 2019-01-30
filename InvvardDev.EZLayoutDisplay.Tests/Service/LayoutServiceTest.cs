using System;
using System.Threading.Tasks;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Implementation;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;
using Xunit;

namespace InvvardDev.EZLayoutDisplay.Tests.Service
{
    public class LayoutServiceTest
    {
        [ Theory ]
        [ InlineData("EOEb", true) ]
        [ InlineData("default", true) ]
        [ InlineData("test", false) ]
        public async Task GetErgodoxLayout(string layoutHashId, bool exist)
        {
            // Arrange
            ILayoutService layoutService = new LayoutService();

            // Act
            ErgodoxLayout response = null;

            if (exist)
            {
                response = await layoutService.GetErgodoxLayout(layoutHashId);
            }
            else
            {
                await Assert.ThrowsAsync<ArgumentException>(() => layoutService.GetErgodoxLayout(layoutHashId));
            }

            // Assert
            if (exist)
            {
                Assert.NotNull(response);
                Assert.IsType<ErgodoxLayout>(response);
                Assert.Single(response.Revisions);
                Assert.False(string.IsNullOrWhiteSpace(response.HashId));
                Assert.False(string.IsNullOrWhiteSpace(response.Title));
            }
            else { }
        }
    }
}