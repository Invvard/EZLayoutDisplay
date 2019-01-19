using InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface;
using InvvardDev.EZLayoutDisplay.Desktop.ViewModel;
using Moq;
using Xunit;

namespace InvvardDev.EZLayoutDisplay.Tests
{
    public class SettingsViewModelTest
    {
        [Fact]
        public void SettingsViewModelConstructor()
        {
            //Arrange
            var tbContent = "This is a test";
            var settingsService = new Mock<ISettingsService>();
            settingsService.Setup(s => s.ErgodoxLayoutUrl).Returns(tbContent);
            var windowService = new Mock<IWindowService>();

            //Act
            var settingsViewModel = new SettingsViewModel(settingsService.Object, windowService.Object);

            //Assert
            Assert.Equal(tbContent, settingsViewModel.TxtLayoutUrlText);
        }
    }
}
