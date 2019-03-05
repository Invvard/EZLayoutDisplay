using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;
using InvvardDev.EZLayoutDisplay.Desktop.View;
using InvvardDev.EZLayoutDisplay.Desktop.ViewModel;
using Moq;
using Xunit;

namespace InvvardDev.EZLayoutDisplay.Tests.ViewModel
{
    public class MainViewModelTest
    {
        [ Fact ]
        public void MainViewModelConstructor()
        {
            //Arrange
            var mockWindowService = new Mock<IWindowService>();
            var mockApplicationService = new Mock<IApplicationService>();

            //Act
            var mainViewModel = new MainViewModel(mockWindowService.Object, mockApplicationService.Object);

            //Assert
            Assert.Equal("Show Layout", mainViewModel.TrayMenuShowLayoutCommandLabel);
            Assert.Equal("Hyper+Space", mainViewModel.TrayMenuShowLayoutShortcutLabel);
            Assert.Equal("Settings", mainViewModel.TrayMenuShowSettingsCommandLabel);
            Assert.Equal("About", mainViewModel.TrayMenuShowAboutCommandLabel);
            Assert.Equal("Exit", mainViewModel.TrayMenuExitCommandLabel);
        }

        [ Fact ]
        public void ShowLayoutCommand()
        {
            //Arrange
            var mockWindowService = new Mock<IWindowService>();
            mockWindowService.Setup(w => w.ShowWindow<DisplayLayoutWindow>()).Verifiable();
            var mockApplicationService = new Mock<IApplicationService>();

            //Act
            var mainViewModel = new MainViewModel(mockWindowService.Object, mockApplicationService.Object);
            mainViewModel.ShowLayoutCommand.Execute(null);

            //Assert
            mockWindowService.Verify(w => w.ShowWindow<DisplayLayoutWindow>(), Times.AtLeastOnce);
        }

        [ Fact ]
        public void ShowSettingsCommand()
        {
            //Arrange
            var mockWindowService = new Mock<IWindowService>();
            mockWindowService.Setup(w => w.ShowWindow<SettingsWindow>()).Verifiable();
            var mockApplicationService = new Mock<IApplicationService>();

            //Act
            var mainViewModel = new MainViewModel(mockWindowService.Object, mockApplicationService.Object);
            mainViewModel.ShowSettingsCommand.Execute(null);

            //Assert
            mockWindowService.Verify(w => w.ShowWindow<SettingsWindow>(), Times.AtLeastOnce);
        }

        [Fact]
        public void ShowAboutCommand()
        {
            //Arrange
            var mockWindowService = new Mock<IWindowService>();
            mockWindowService.Setup(w => w.ShowWindow<AboutWindow>()).Verifiable();
            var mockApplicationService = new Mock<IApplicationService>();

            //Act
            var mainViewModel = new MainViewModel(mockWindowService.Object, mockApplicationService.Object);
            mainViewModel.ShowAboutCommand.Execute(null);

            //Assert
            mockWindowService.Verify(w => w.ShowWindow<AboutWindow>(), Times.AtLeastOnce);
        }

        [ Fact ]
        public void ExitApplicationCommand()
        {
            //Arrange
            var mockWindowService = new Mock<IWindowService>();
            var mockApplicationService = new Mock<IApplicationService>();
            mockApplicationService.Setup(a => a.ShutdownApplication()).Verifiable();

            //Act
            var mainViewModel = new MainViewModel(mockWindowService.Object, mockApplicationService.Object);
            mainViewModel.ExitApplicationCommand.Execute(null);

            //Assert
            mockApplicationService.Verify(w => w.ShutdownApplication(), Times.Once);
        }
    }
}