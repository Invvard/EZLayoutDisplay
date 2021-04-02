using System;
using System.Collections.Generic;
using System.Windows.Input;
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
        private static EZLayout CreateLayers(int layerNumber)
        {
            var keyboardLayout = new EZLayout();

            for (int i = 0 ; i < layerNumber ; i++)
            {
                keyboardLayout.EZLayers.Add(new EZLayer {
                                                            Index = i,
                                                            EZKeys = new List<EZKey> {
                                                                                         new EZKey()
                                                                                     }
                                                        });
            }

            return keyboardLayout;
        }

        [ Fact ]
        public void DisplayLayoutViewModel_Constructor()
        {
            //Arrange
            var mockWindowService = new Mock<IWindowService>();
            var mockLayoutService = new Mock<ILayoutService>();
            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.SetupProperty(s => s.EZLayout, CreateLayers(1));
            mockLayoutService.Setup(l => l.SupportsGeometry(null)).Returns(true).Verifiable();

            //Act
            var displayLayoutViewModel = new DisplayLayoutViewModel(mockWindowService.Object, mockLayoutService.Object, mockSettingsService.Object);

            //Assert
            Assert.Equal("ErgoDox Layout", displayLayoutViewModel.WindowTitle);
            Assert.Null(displayLayoutViewModel.NoLayoutWarningFirstLine);
            Assert.Null(displayLayoutViewModel.NoLayoutWarningSecondLine);
            Assert.Equal("Current layer :", displayLayoutViewModel.CurrentLayerNameTitle);
            Assert.Equal("", displayLayoutViewModel.CurrentLayerName);
            Assert.Equal("Scroll up/down or press 'Space' to display next layer", displayLayoutViewModel.ControlHintSpaceLabel);
            Assert.Equal("Press 'Escape' to hide window", displayLayoutViewModel.ControlHintEscapeLabel);
            Assert.Equal("_Pin window", displayLayoutViewModel.ToggleBtnPinWindowContent);
            Assert.Equal("Press 'P' to toggle", displayLayoutViewModel.ToggleBtnPinWindowTooltip);
        }

        [ Fact ]
        public void LostFocusCommand_Execute()
        {
            //Arrange
            var mockWindowService = new Mock<IWindowService>();
            mockWindowService.Setup(w => w.CloseWindow<DisplayLayoutWindow>()).Verifiable();
            var mockLayoutService = new Mock<ILayoutService>();
            var mockSettingsService = new Mock<ISettingsService>();

            //Act
            var displayLayoutViewModel = new DisplayLayoutViewModel(mockWindowService.Object, mockLayoutService.Object, mockSettingsService.Object);
            displayLayoutViewModel.LostFocusCommand.Execute(null);

            //Assert
            mockWindowService.Verify(w => w.CloseWindow<DisplayLayoutWindow>(), Times.Once);
        }

        [ Theory ]
        [ InlineData(true) ]
        [ InlineData(false) ]
        public void LostFocusCommand_CanExecute(bool isPinned)
        {
            //Arrange
            var mockWindowService = new Mock<IWindowService>();
            mockWindowService.Setup(w => w.CloseWindow<DisplayLayoutWindow>()).Verifiable();
            var mockLayoutService = new Mock<ILayoutService>();
            var mockSettingsService = new Mock<ISettingsService>();

            //Act
            var displayLayoutViewModel = new DisplayLayoutViewModel(mockWindowService.Object, mockLayoutService.Object, mockSettingsService.Object);
            displayLayoutViewModel.IsWindowPinned = isPinned;
            displayLayoutViewModel.LostFocusCommand.Execute(null);

            //Assert
            if (isPinned)
            {
                mockWindowService.Verify(w => w.CloseWindow<DisplayLayoutWindow>(), Times.Never);
            }
            else
            {
                mockWindowService.Verify(w => w.CloseWindow<DisplayLayoutWindow>(), Times.Once);
            }
        }

        [ Fact ]
        public void HideWindowCommand_Execute()
        {
            //Arrange
            var mockLayoutService = new Mock<ILayoutService>();
            var mockWindowService = new Mock<IWindowService>();
            mockWindowService.Setup(w => w.CloseWindow<DisplayLayoutWindow>()).Verifiable();
            var mockSettingsService = new Mock<ISettingsService>();

            //Act
            var displayLayoutViewModel = new DisplayLayoutViewModel(mockWindowService.Object, mockLayoutService.Object, mockSettingsService.Object);
            displayLayoutViewModel.HideWindowCommand.Execute(null);

            //Assert
            mockWindowService.Verify(w => w.CloseWindow<DisplayLayoutWindow>(), Times.Once);
        }

        [ Theory ]
        [ InlineData(0, 1, true) ]
        [ InlineData(1, 2, false) ]
        [ InlineData(76, 2, false) ]
        public void LoadCompleteLayout(int numberOfKey, int numberOfLayer, bool noLayoutAvailable)
        {
            //Arrange
            var layoutTemplate = new List<KeyTemplate>();
            var ezKeys = new List<EZKey>();

            for (int i = 0 ; i < numberOfKey ; i++)
            {
                layoutTemplate.Add(new KeyTemplate(i, i, 54, 81));
                ezKeys.Add(new EZKey());
            }

            var keyboardLayout = new EZLayout();

            for (int i = 0 ; i < numberOfLayer ; i++)
            {
                keyboardLayout.EZLayers.Add(new EZLayer {
                                                            Index = i,
                                                            EZKeys = new List<EZKey>(ezKeys)
                                                        });
            }

            var mockWindowService = new Mock<IWindowService>();
            var mockLayoutService = new Mock<ILayoutService>();
            mockLayoutService.Setup(l => l.SupportsGeometry(null)).Returns(true);
            mockLayoutService.Setup(l => l.GetLayoutTemplate(null)).ReturnsAsync(layoutTemplate).Verifiable();
            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.SetupProperty(s => s.EZLayout, keyboardLayout);

            //Act
            var displayLayoutViewModel = new DisplayLayoutViewModel(mockWindowService.Object, mockLayoutService.Object, mockSettingsService.Object);

            //Assert
            Assert.Equal(noLayoutAvailable, displayLayoutViewModel.NoLayoutAvailable);
            mockLayoutService.Verify(l => l.GetLayoutTemplate(null), Times.AtMost(numberOfLayer));
            Assert.Equal(numberOfKey, displayLayoutViewModel.CurrentLayoutTemplate.Count);
        }

        [ Theory ]
        [ InlineData(0, false) ]
        [ InlineData(1, true) ]
        public void NextLayerCommand_CanExecute(int layerNumber, bool expectedCanExecute)
        {
            //Arrange
            var keyboardLayout = CreateLayers(layerNumber);

            var layoutTemplate = new List<KeyTemplate>();

            for (int i = 0 ; i < 1 ; i++)
            {
                layoutTemplate.Add(new KeyTemplate(i, i, 54, 81));
            }

            var mockLayoutService = new Mock<ILayoutService>();
            mockLayoutService.Setup(l => l.SupportsGeometry(null)).Returns(true);
            mockLayoutService.Setup(l => l.GetLayoutTemplate(null)).ReturnsAsync(layoutTemplate);
            var mockWindowService = new Mock<IWindowService>();
            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.SetupProperty(s => s.EZLayout, keyboardLayout);

            //Act
            var displayLayoutViewModel = new DisplayLayoutViewModel(mockWindowService.Object, mockLayoutService.Object, mockSettingsService.Object);

            //Assert
            Assert.Equal(expectedCanExecute, displayLayoutViewModel.NextLayerCommand.CanExecute(null));
        }

        [ WpfTheory ]
        [ InlineData(0, 0, 120) ]
        [ InlineData(0, 0, -120) ]
        [ InlineData(1, 0, 120) ]
        [ InlineData(1, 0, -120) ]
        [ InlineData(2, 1, -120) ]
        [ InlineData(2, 1, 120) ]
        [ InlineData(2, 0, 120, -120) ]
        [ InlineData(2, 0, 120, 120) ]
        [ InlineData(2, 1, 120, 120, -120) ]
        [ InlineData(3, 2, 120) ]
        [ InlineData(3, 1, -120) ]
        [ InlineData(3, 2, -120, -120) ]
        [ InlineData(3, 1, 120, 120) ]
        [ InlineData(3, 0, -120, -120, -120) ]
        [ InlineData(3, 0, 120, 120, 120) ]
        public void ScrollLayerCommand_Execute(int layerNumber, int expectedCurrentLayerIndex, params int[] scrollingValues)
        {
            //Arrange
            var keyboardLayout = CreateLayers(layerNumber);

            var layoutTemplate = new List<KeyTemplate>();

            for (int i = 0 ; i < 1 ; i++)
            {
                layoutTemplate.Add(new KeyTemplate(i, i, 54, 81));
            }

            var mockLayoutService = new Mock<ILayoutService>();
            mockLayoutService.Setup(l => l.SupportsGeometry(null)).Returns(true);
            mockLayoutService.Setup(l => l.GetLayoutTemplate(null)).ReturnsAsync(layoutTemplate);
            var mockWindowService = new Mock<IWindowService>();
            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.SetupProperty(s => s.EZLayout, keyboardLayout);

            //Act
            var displayLayoutViewModel = new DisplayLayoutViewModel(mockWindowService.Object, mockLayoutService.Object, mockSettingsService.Object);

            foreach (var scrollingValue in scrollingValues)
            {
                displayLayoutViewModel.ScrollLayerCommand.Execute(new MouseWheelEventArgs(Mouse.PrimaryDevice, 0, scrollingValue));
            }

            //Assert
            Assert.Equal(expectedCurrentLayerIndex, displayLayoutViewModel.CurrentLayerIndex);
        }

        [ Theory ]
        [ InlineData(0, 0, 0) ]
        [ InlineData(1, 0, 0) ]
        [ InlineData(1, 1, 0) ]
        [ InlineData(2, 0, 0) ]
        [ InlineData(2, 1, 1) ]
        [ InlineData(2, 2, 0) ]
        [ InlineData(3, 0, 0) ]
        [ InlineData(3, 1, 1) ]
        [ InlineData(3, 2, 2) ]
        [ InlineData(3, 3, 0) ]
        public void NextLayerCommand_Execute(int layerNumber, int nextLayerHit, int expectedCurrentLayerIndex)
        {
            //Arrange
            var keyboardLayout = CreateLayers(layerNumber);
            var layoutTemplate = new List<KeyTemplate>();

            for (int i = 0 ; i < 1 ; i++)
            {
                layoutTemplate.Add(new KeyTemplate(i, i, 54, 81));
            }

            var mockLayoutService = new Mock<ILayoutService>();
            mockLayoutService.Setup(l => l.SupportsGeometry(null)).Returns(true);
            mockLayoutService.Setup(l => l.GetLayoutTemplate(null)).ReturnsAsync(layoutTemplate);
            var mockWindowService = new Mock<IWindowService>();
            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.SetupProperty(s => s.EZLayout, keyboardLayout);

            //Act
            var displayLayoutViewModel = new DisplayLayoutViewModel(mockWindowService.Object, mockLayoutService.Object, mockSettingsService.Object);

            for (int i = 0 ; i < nextLayerHit ; i++)
            {
                displayLayoutViewModel.NextLayerCommand.Execute(null);
            }

            //Assert
            Assert.Equal(expectedCurrentLayerIndex, displayLayoutViewModel.CurrentLayerIndex);
        }

        [ Fact ]
        public void NoLayoutAvailable()
        {
            // Arrange
            var mockLayoutService = new Mock<ILayoutService>();
            var mockWindowService = new Mock<IWindowService>();
            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.SetupProperty(s => s.EZLayout, CreateLayers(0));

            // Act
            var displayLayoutViewModel = new DisplayLayoutViewModel(mockWindowService.Object, mockLayoutService.Object, mockSettingsService.Object);

            // Assert
            Assert.True(displayLayoutViewModel.NoLayoutAvailable);
        }
    }
}