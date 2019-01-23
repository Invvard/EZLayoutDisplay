using System.Threading.Tasks;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Implementation;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface;
using Xunit;

namespace InvvardDev.EZLayoutDisplay.Tests.Service
{
    public class LayoutServiceTest
    {
        [ Fact ]
        public async Task GetErgodoxLayout()
        {
            // Arrange
            ILayoutService layoutService = new LayoutService();
            var layoutHashId = "EOEb";

            // Act
            var response = await layoutService.GetErgodoxLayout(layoutHashId);

            // Assert
            Assert.IsType<ErgodoxLayout>(response);
            Assert.Single(response.Revisions);
            Assert.False(string.IsNullOrWhiteSpace(response.HashId));
            Assert.False(string.IsNullOrWhiteSpace(response.Title));
        }
    }
}