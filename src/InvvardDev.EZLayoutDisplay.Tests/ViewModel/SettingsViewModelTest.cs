using System;
using System.Threading.Tasks;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;
using InvvardDev.EZLayoutDisplay.Desktop.View;
using InvvardDev.EZLayoutDisplay.Desktop.ViewModel;
using Moq;
using Xunit;

namespace InvvardDev.EZLayoutDisplay.Tests.ViewModel
{
    public class SettingsViewModelTest
    {
        [ Fact ]
        public void SettingsViewModel_Constructor()
        {
            //Arrange
            var tbContentInitial = "This is a test";
            var settingsService = new Mock<ISettingsService>();
            settingsService.Setup(s => s.ErgodoxLayoutUrl).Returns(tbContentInitial);
            var windowService = new Mock<IWindowService>();
            var mockLayoutService = new Mock<ILayoutService>();

            //Act
            var settingsViewModel = new SettingsViewModel(settingsService.Object, windowService.Object, mockLayoutService.Object);

            //Assert
            Assert.Equal("Settings", settingsViewModel.WindowTitle);
            Assert.Equal("Configurator URL to your layout :", settingsViewModel.LayoutUrlLabel);
            Assert.Equal("Apply", settingsViewModel.ApplyCommandLabel);
            Assert.Equal("Close", settingsViewModel.CloseCommandLabel);
            Assert.Equal("Cancel", settingsViewModel.CancelCommandLabel);
            Assert.Equal(tbContentInitial, settingsViewModel.LayoutUrlContent);
            Assert.Equal("Hotkey to display layout", settingsViewModel.HotkeyTitleLabel);
            Assert.Equal("ALT", settingsViewModel.AltModifierLabel);
            Assert.Equal("CTRL", settingsViewModel.CtrlModifierLabel);
            Assert.Equal("SHIFT", settingsViewModel.ShiftModifierLabel);
            Assert.Equal("Windows", settingsViewModel.WindowsModifierLabel);
            Assert.Equal("Update", settingsViewModel.UpdateCommandLabel);
        }

        [ Theory ]
        [ InlineData("initial value", "initial value", false) ]
        [ InlineData("initial value", "new value", true) ]
        public void RelayCommand_CanExecute(string initialValue, string newValue, bool canExecute)
        {
            //Arrange
            var settingsService = new Mock<ISettingsService>();
            settingsService.Setup(s => s.ErgodoxLayoutUrl).Returns(initialValue);
            var windowService = new Mock<IWindowService>();
            var mockLayoutService = new Mock<ILayoutService>();

            //Act
            var settingsViewModel = new SettingsViewModel(settingsService.Object, windowService.Object, mockLayoutService.Object) {
                                                                                                                                      LayoutUrlContent = newValue
                                                                                                                                  };

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

        [ Fact ]
        public void ApplyCommand_Execute()
        {
            //Arrange
            var tbContentInitial = "This is a test";
            var tbContentNewValue = "new value";
            var settingsService = new Mock<ISettingsService>();
            settingsService.Setup(s => s.Save()).Verifiable();
            settingsService.SetupProperty(s => s.ErgodoxLayoutUrl, tbContentInitial);
            var windowService = new Mock<IWindowService>();
            var mockLayoutService = new Mock<ILayoutService>();

            //Act
            var settingsViewModel = new SettingsViewModel(settingsService.Object, windowService.Object, mockLayoutService.Object) {
                                                                                                                                      LayoutUrlContent = tbContentNewValue
                                                                                                                                  };
            settingsViewModel.ApplySettingsCommand.Execute(null);

            //Assert
            Assert.Equal(tbContentNewValue, settingsService.Object.ErgodoxLayoutUrl);
            settingsService.Verify(s => s.Save());
            Assert.False(settingsViewModel.ApplySettingsCommand.CanExecute(null));
            Assert.False(settingsViewModel.CancelSettingsCommand.CanExecute(null));
        }

        [ Fact ]
        public void CancelCommand_Execute()
        {
            //Arrange
            var tbContentInitial = "This is a test";
            var tbContentNewValue = "new value";
            var settingsService = new Mock<ISettingsService>();
            settingsService.Setup(s => s.Cancel()).Verifiable();
            settingsService.SetupProperty(s => s.ErgodoxLayoutUrl, tbContentInitial);
            var windowService = new Mock<IWindowService>();
            var mockLayoutService = new Mock<ILayoutService>();

            //Act
            var settingsViewModel = new SettingsViewModel(settingsService.Object, windowService.Object, mockLayoutService.Object) {
                                                                                                                                      LayoutUrlContent = tbContentNewValue
                                                                                                                                  };
            settingsViewModel.CancelSettingsCommand.Execute(null);

            //Assert
            Assert.Equal(tbContentInitial, settingsService.Object.ErgodoxLayoutUrl);
            settingsService.Verify(s => s.Cancel());
            Assert.False(settingsViewModel.ApplySettingsCommand.CanExecute(null));
            Assert.False(settingsViewModel.CancelSettingsCommand.CanExecute(null));
        }

        [ Theory ]
        [ InlineData(false) ]
        [ InlineData(true) ]
        public void CloseCommand_Execute(bool mustSave)
        {
            //Arrange
            var tbContentInitial = "This is a test";
            var tbContentNewValue = "new value";
            var settingsService = new Mock<ISettingsService>();
            settingsService.Setup(s => s.Save()).Verifiable();
            settingsService.SetupProperty(s => s.ErgodoxLayoutUrl, tbContentInitial);
            var windowService = new Mock<IWindowService>();
            windowService.Setup(w => w.CloseWindow<SettingsWindow>()).Verifiable();
            var mockLayoutService = new Mock<ILayoutService>();

            //Act
            var settingsViewModel = new SettingsViewModel(settingsService.Object, windowService.Object, mockLayoutService.Object);

            if (mustSave)
            {
                settingsViewModel.LayoutUrlContent = tbContentNewValue;
            }

            settingsViewModel.CloseSettingsCommand.Execute(null);

            //Assert
            if (mustSave)
            {
                Assert.Equal(tbContentNewValue, settingsService.Object.ErgodoxLayoutUrl);
                settingsService.Verify(s => s.Save(), Times.Once);
                Assert.False(settingsViewModel.ApplySettingsCommand.CanExecute(null));
                Assert.False(settingsViewModel.CancelSettingsCommand.CanExecute(null));
                windowService.Verify(w => w.CloseWindow<SettingsWindow>(), Times.Once);
            }
            else
            {
                Assert.Equal(tbContentInitial, settingsService.Object.ErgodoxLayoutUrl);
                Assert.False(settingsViewModel.ApplySettingsCommand.CanExecute(null));
                Assert.False(settingsViewModel.CancelSettingsCommand.CanExecute(null));
                settingsService.Verify(s => s.Save(), Times.Never);
                windowService.Verify(w => w.CloseWindow<SettingsWindow>(), Times.Once);
            }
        }

        [ Theory ]
        [ InlineData("https://configure.ergodox-ez.com/layouts/abcd/latest/0", "abcd") ]
        [ InlineData("https://configure.ergodox-ez.com/layouts/1234/latest/0", "1234") ]
        [ InlineData("https://configure.ergodox-ez.com/layouts/a2Vt/latest/0", "a2Vt") ]
        [ InlineData("https://configure.ergodox-ez.com/layouts/default/latest/0", "default") ]
        [ InlineData("https://configure.ergodox-ez.com/layouts/j3o4", "j3o4") ]
        [ InlineData("https://configure.ergodox-ez.com/layouts/r2d2/lat/9", "r2d2") ]
        [ InlineData("https://configure.ergodox-ez.com/layouts/def/latest/0", "default") ]
        [ InlineData("https://configure.ergodox-ez.com/layouts/_t3s/latest/0", "default") ]
        [ InlineData("https://configure.ergodox-ez.com/layouts/t3s/latest/0", "default") ]
        [ InlineData("https://configure.ergodox-ez.com/layouts/adbcd/latest/0", "adbcd") ]
        [ InlineData("https://configure.ergodox-ez.com/layouts/asdfasdfasdfasdfgfasdffgasf/latest/0", "asdfasdfasdfasdfgfasdffgasf") ]
        public void UpdateLayoutCommand_Execute(string layoutUrl, string expectedHashId)
        {
            //Arrange
            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.SetupProperty(s => s.ErgodoxLayoutUrl, layoutUrl);
            var mockWindowService = new Mock<IWindowService>();
            var mockLayoutService = new Mock<ILayoutService>();
            mockLayoutService.Setup(l => l.GetErgodoxLayout(expectedHashId)).Returns(Task.FromResult(It.IsAny<ErgodoxLayout>())).Verifiable();
            mockLayoutService.Setup(l => l.PrepareEZLayout(It.IsAny<ErgodoxLayout>())).Verifiable();

            //Act
            var settingsViewModel = new SettingsViewModel(mockSettingsService.Object, mockWindowService.Object, mockLayoutService.Object);

            settingsViewModel.UpdateLayoutCommand.Execute(null);

            //Assert
            mockLayoutService.Verify();
        }

        [ Fact ]
        public void UpdateLayoutCommandExecute_ArgumentExceptionRaised()
        {
            // Arrange
            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.SetupProperty(s => s.ErgodoxLayoutUrl, "test");
            var mockWindowService = new Mock<IWindowService>();
            mockWindowService.Setup(w => w.ShowWarning(It.IsAny<string>())).Verifiable();
            var mockLayoutService = new Mock<ILayoutService>();
            mockLayoutService.Setup(l => l.GetErgodoxLayout(It.IsAny<string>())).Throws<ArgumentException>();

            // Act
            var settingsViewModel = new SettingsViewModel(mockSettingsService.Object, mockWindowService.Object, mockLayoutService.Object);
            settingsViewModel.UpdateLayoutCommand.Execute(null);

            // Assert
            mockLayoutService.Verify();
            mockWindowService.Verify();
        }

        [ Fact ]
        public void UpdateLayoutCommandExecute_ArgumentNullExceptionRaised()
        {
            // Arrange
            var mockSettingsService = new Mock<ISettingsService>();
            mockSettingsService.SetupProperty(s => s.ErgodoxLayoutUrl, "");
            var mockWindowService = new Mock<IWindowService>();
            mockWindowService.Setup(w => w.ShowWarning(It.IsAny<string>())).Verifiable();
            var mockLayoutService = new Mock<ILayoutService>();
            mockLayoutService.Setup(l => l.GetErgodoxLayout(It.IsAny<string>())).Throws<ArgumentNullException>();

            // Act
            var settingsViewModel = new SettingsViewModel(mockSettingsService.Object, mockWindowService.Object, mockLayoutService.Object);
            settingsViewModel.UpdateLayoutCommand.Execute(null);

            // Assert
            mockLayoutService.Verify();
            mockWindowService.Verify();
        }
    }
}