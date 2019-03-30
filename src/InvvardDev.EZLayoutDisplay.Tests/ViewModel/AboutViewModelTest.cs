using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;
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

            // Act
            var aboutViewModel = new AboutViewModel(mockWindowService.Object);

            // Assert
            Assert.Equal("About EZ Layout Display", aboutViewModel.WindowTitle);
            Assert.Equal("EZ Layout Display", aboutViewModel.AppTitleLabel);
            Assert.Equal("v.0.3.1.2002", aboutViewModel.AppVersionLabel);
            Assert.Equal("Created by", aboutViewModel.CreatedTitleLabel);
            Assert.Equal("Based on", aboutViewModel.BasedOnTitleLabel);
            Assert.Equal("Project home", aboutViewModel.ProjectHomeTitleLabel);
            Assert.Equal("Contact", aboutViewModel.ContactTitleLabel);
            Assert.Equal("Pierre CAVAROC", aboutViewModel.CreatorInfoLabel);
            Assert.Equal("ErgoDox EZ Configurator", aboutViewModel.BasedOnInfoLabel);
            Assert.Equal("EZ Layout Display", aboutViewModel.ProjectHomeInfoLabel);
            Assert.Equal("@Invvard", aboutViewModel.ContactInfoLabel);
            Assert.Equal("OK", aboutViewModel.CloseButtonLabel);
        }
    }
}