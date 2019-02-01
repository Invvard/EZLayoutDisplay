using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;
using InvvardDev.EZLayoutDisplay.Desktop.View;
using InvvardDev.EZLayoutDisplay.Desktop.ViewModel;
using Moq;
using Xunit;

namespace InvvardDev.EZLayoutDisplay.Tests.ViewModel
{
    public class DisplayLayoutViewModelTest
    {
        [ Fact ]
        public void DisplayLayoutViewModelConstructor()
        {
            //Arrange
            var mockWindowService = new Mock<IWindowService>();

            //Act
            var displayLayoutViewModel = new DisplayLayoutViewModel(mockWindowService.Object);

            //Assert
            Assert.Equal("Ergodox Layout", displayLayoutViewModel.WindowTitle);
        }

        [ Fact ]
        public void LostFocusCommandExecute()
        {
            //Arrange
            var mockWindowService = new Mock<IWindowService>();
            mockWindowService.Setup(w => w.CloseWindow<DisplayLayoutWindow>()).Verifiable();

            //Act
            var displayLayoutViewModel = new DisplayLayoutViewModel(mockWindowService.Object);
            displayLayoutViewModel.LostFocusCommand.Execute(null);

            //Assert
            mockWindowService.Verify(w => w.CloseWindow<DisplayLayoutWindow>(), Times.Once);
        }
    }
}