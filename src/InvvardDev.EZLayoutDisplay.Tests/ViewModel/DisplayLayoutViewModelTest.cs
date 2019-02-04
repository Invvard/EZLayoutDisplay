using System.Collections.Generic;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
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
            var mockLayoutService = new Mock<ILayoutService>();

            //Act
            var displayLayoutViewModel = new DisplayLayoutViewModel(mockWindowService.Object, mockLayoutService.Object);

            //Assert
            Assert.Equal("Ergodox Layout", displayLayoutViewModel.WindowTitle);
        }

        [ Fact ]
        public void LostFocusCommandExecute()
        {
            //Arrange
            var mockWindowService = new Mock<IWindowService>();
            mockWindowService.Setup(w => w.CloseWindow<DisplayLayoutWindow>()).Verifiable();
            var mockLayoutService = new Mock<ILayoutService>();

            //Act
            var displayLayoutViewModel = new DisplayLayoutViewModel(mockWindowService.Object, mockLayoutService.Object);
            displayLayoutViewModel.LostFocusCommand.Execute(null);

            //Assert
            mockWindowService.Verify(w => w.CloseWindow<DisplayLayoutWindow>(), Times.Once);
        }

        [ Theory ]
        [ InlineData(0) ]
        [ InlineData(1) ]
        [ InlineData(76) ]
        public void PopulateModel(int numberOfKey)
        {
            //Arrange
            var layoutTemplate = new List<KeyTemplate>();

            for (int i = 0 ; i < numberOfKey ; i++) { layoutTemplate.Add(new KeyTemplate(i, i, 54, 81)); }

            var mockWindowService = new Mock<IWindowService>();
            var mockLayoutService = new Mock<ILayoutService>();
            mockLayoutService.Setup(l => l.GetLayoutTemplate()).ReturnsAsync(layoutTemplate).Verifiable();

            //Act
            var displayLayoutViewModel = new DisplayLayoutViewModel(mockWindowService.Object, mockLayoutService.Object);

            //Assert
            mockLayoutService.Verify(l => l.GetLayoutTemplate(), Times.Once);
            Assert.Equal(numberOfKey, displayLayoutViewModel.LayoutTemplate.Count);
        }
    }
}