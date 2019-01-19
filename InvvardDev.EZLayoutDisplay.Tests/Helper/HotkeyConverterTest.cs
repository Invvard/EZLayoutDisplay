using InvvardDev.EZLayoutDisplay.Desktop.Model;
using Newtonsoft.Json;
using NonInvasiveKeyboardHookLibrary;
using Xunit;

namespace InvvardDev.EZLayoutDisplay.Tests.Helper
{
    public class HotkeyConverterTest
    {
        [ Theory ]
        [ InlineData(new[] { ModifierKeys.Alt }, "{\"modifiers\":[0],\"keycode\":96}") ]
        [ InlineData(new[] { ModifierKeys.Alt, ModifierKeys.Control }, "{\"modifiers\":[0,1],\"keycode\":96}") ]
        public void HotkeySerialization(ModifierKeys[] modifiers, string expectedSerializedHotkey)
        {
            // Arrange
            var hotkeyJson = expectedSerializedHotkey;
            var hotkey = new Hotkey(0x60, modifiers);
            object result = "";

            // Act
            result = JsonConvert.SerializeObject(hotkey);

            // Assert
            Assert.IsType<string>(result);
            Assert.Equal(hotkeyJson, result);
        }

        [ Fact ]
        public void HotkeyDeserialization()
        {
            // Arrange
            var hotkeyJson = "{\"modifiers\":[0],\"keycode\":96}";
            var hotkey = new Hotkey(0x60, ModifierKeys.Alt);

            // Act
            var result = JsonConvert.DeserializeObject<Hotkey>(hotkeyJson);

            // Assert
            Assert.IsType<Hotkey>(result);
            Assert.Equal(hotkey.KeyCode, result.KeyCode);
        }
    }
}