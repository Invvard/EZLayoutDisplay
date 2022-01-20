using System.Windows;
using System.Windows.Controls;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Ez.Content;

namespace InvvardDev.EZLayoutDisplay.Desktop.Helper
{
    public class KeyFeatureTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SimpleLabelDataTemplate { private get; set; }
        public DataTemplate GlyphDataTemplate { private get; set; }
        public DataTemplate ModdedGlyphDataTemplate { private get; set; }
        public DataTemplate LayerDataTemplate { private get; set; }
        public DataTemplate ColorPickerDataTemplate { private get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                Glyph glyph when !string.IsNullOrWhiteSpace(glyph.Modifier) => ModdedGlyphDataTemplate,
                Glyph glyph when string.IsNullOrWhiteSpace(glyph.Modifier) => GlyphDataTemplate,
                Layer => LayerDataTemplate,
                ColorPicker => ColorPickerDataTemplate,
                _ => SimpleLabelDataTemplate
            };
        }
    }
}