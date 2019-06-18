using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;
using InvvardDev.EZLayoutDisplay.Desktop.View;
using InvvardDev.EZLayoutDisplay.Desktop.ViewModel;
using Moq;
using Xunit;

namespace InvvardDev.EZLayoutDisplay.Tests.ViewModel
{
    public class AboutViewModelTest
    {
        [ Fact ]
        public void AboutViewModelConstructor()
        {
            // Arrange
            var mockWindowService = new Mock<IWindowService>();
            var mockProcessService = new Mock<IProcessService>();

            // Act
            var aboutViewModel = new AboutViewModel(mockWindowService.Object, mockProcessService.Object);

            // Assert
            Assert.Equal("About EZ Layout Display", aboutViewModel.WindowTitle);
            Assert.Equal("EZ Layout Display", aboutViewModel.AppTitleLabel);
            Assert.Equal("v.1.1.2", aboutViewModel.AppVersionLabel);
            Assert.Equal("Created by", aboutViewModel.CreatedTitleLabel);
            Assert.Equal("Based on", aboutViewModel.BasedOnTitleLabel);
            Assert.Equal("Project home", aboutViewModel.ProjectHomeTitleLabel);
            Assert.Equal("Contact", aboutViewModel.ContactTitleLabel);
            Assert.Equal("Pierre CAVAROC", aboutViewModel.CreatorInfoLabel);
            Assert.Equal("ErgoDox EZ Configurator", aboutViewModel.BasedOnInfoLabel);
            Assert.Equal("EZ Layout Display", aboutViewModel.ProjectHomeInfoLabel);
            Assert.Equal("@Invvard", aboutViewModel.TwitterInfoLabel);
            Assert.Equal("r/EZLayoutDisplay", aboutViewModel.RedditInfoLabel);
            Assert.Equal("OK", aboutViewModel.CloseButtonLabel);
        }

        [ Fact ]
        public void CloseAboutWindow_Execute()
        {
            // Arrange
            var mockWindowService = new Mock<IWindowService>();
            mockWindowService.Setup(w => w.CloseWindow<AboutWindow>()).Verifiable();
            var mockProcessService = new Mock<IProcessService>();

            // Act
            var aboutViewModel = new AboutViewModel(mockWindowService.Object, mockProcessService.Object);
            aboutViewModel.CloseAboutCommand.Execute(null);

            // Assert
            mockWindowService.Verify(w => w.CloseWindow<AboutWindow>(), Times.Once);
        }

        [ Fact ]
        public void NavigateBasedOnUrl_Execute()
        {
            // Arrange
            var expectedUrl = "https://configure.ergodox-ez.com/layouts/default/latest/0";
            var mockWindowService = new Mock<IWindowService>();
            var mockProcessService = new Mock<IProcessService>();
            mockProcessService.Setup(p => p.StartWebUrl(expectedUrl)).Verifiable();

            // Act
            var aboutViewModel = new AboutViewModel(mockWindowService.Object, mockProcessService.Object);
            aboutViewModel.NavigateBasedOnUrlCommand.Execute(null);

            // Assert
            mockProcessService.Verify(p => p.StartWebUrl(expectedUrl));
        }

        [ Fact ]
        public void NavigateProjectHomeUrl_Execute()
        {
            // Arrange
            var expectedUrl = "https://github.com/Invvard/EZLayoutDisplay";
            var mockWindowService = new Mock<IWindowService>();
            var mockProcessService = new Mock<IProcessService>();
            mockProcessService.Setup(p => p.StartWebUrl(expectedUrl)).Verifiable();

            // Act
            var aboutViewModel = new AboutViewModel(mockWindowService.Object, mockProcessService.Object);
            aboutViewModel.NavigateProjectHomeUrlCommand.Execute(null);

            // Assert
            mockProcessService.Verify(p => p.StartWebUrl(expectedUrl));
        }

        [ Fact ]
        public void NavigateTwitterUrl_Execute()
        {
            // Arrange
            var expectedUrl = "https://twitter.com/invvard";
            var mockWindowService = new Mock<IWindowService>();
            var mockProcessService = new Mock<IProcessService>();
            mockProcessService.Setup(p => p.StartWebUrl(expectedUrl)).Verifiable();

            // Act
            var aboutViewModel = new AboutViewModel(mockWindowService.Object, mockProcessService.Object);
            aboutViewModel.NavigateTwitterUrlCommand.Execute(null);

            // Assert
            mockProcessService.Verify(p => p.StartWebUrl(expectedUrl));
        }

        [ Fact ]
        public void NavigateRedditUrl_Execute()
        {
            // Arrange
            var expectedUrl = "https://www.reddit.com/r/EZLayoutDisplay/";
            var mockWindowService = new Mock<IWindowService>();
            var mockProcessService = new Mock<IProcessService>();
            mockProcessService.Setup(p => p.StartWebUrl(expectedUrl)).Verifiable();

            // Act
            var aboutViewModel = new AboutViewModel(mockWindowService.Object, mockProcessService.Object);
            aboutViewModel.NavigateRedditUrlCommand.Execute(null);

            // Assert
            mockProcessService.Verify(p => p.StartWebUrl(expectedUrl));
        }
    }
}