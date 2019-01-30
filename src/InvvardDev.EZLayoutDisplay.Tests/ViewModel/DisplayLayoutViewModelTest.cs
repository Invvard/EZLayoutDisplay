using InvvardDev.EZLayoutDisplay.Desktop.ViewModel;
using Xunit;

namespace InvvardDev.EZLayoutDisplay.Tests.ViewModel
{
    public class DisplayLayoutViewModelTest
    {
        [Fact]
        public void DisplayLayoutViewModelConstructor()
        {
            //Arrange

            //Act
            var displayLayoutViewModel = new DisplayLayoutViewModel();

            //Assert
            Assert.Equal("Ergodox Layout", displayLayoutViewModel.WindowTitle);
        }
    }
}