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

        [Theory]
        [InlineData("initial value", "initial value", false)]
        [InlineData("initial value", "new value", true)]
        public void RelayCommand_CanExecute(string initialValue, string newValue, bool canExecute)
        {
            //Arrange
            var settingsService = new Mock<ISettingsService>();
            settingsService.Setup(s => s.ErgodoxLayoutUrl).Returns(initialValue);
            var windowService = new Mock<IWindowService>();

            //Act
            var settingsViewModel = new SettingsViewModel(settingsService.Object, windowService.Object);
            settingsViewModel.TxtLayoutUrlText = newValue;

            //Assert
            if (canExecute)
            {
                Assert.True(settingsViewModel.ApplySettingsCommand.CanExecute(null));
                Assert.True(settingsViewModel.CancelSettingsCommand.CanExecute(null));
            }
            else
            {
                Assert.False(settingsViewModel.ApplySettingsCommand.CanExecute(null));
                Assert.False(settingsViewModel.CancelSettingsCommand.CanExecute(null));
            }

            Assert.True(settingsViewModel.CloseSettingsCommand.CanExecute(null));
        }
    }
}
