using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Tool.KeyDefinitionProvider.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

internal class KeyDefinitionProvider
{
    private const string MetadataUrl = "https://oryx.zsa.io/metadata";
    private const string OryxGlyphUrl = "https://configure.zsa.io/assets/index.d0847e58.css";
    private const string GlyphPattern = @".icon-(?<glyphName>[a-z_]*):before{content:""\\(?<glyphCode>[a-f0-9]{4})""}";
    private const string KeyDefinitionOutputFilename = "keyDefinitions.output.json";
    private const string KeyCategoriesOutputFilename = "keyCategories.output.json";

    private OryxMetadataModel? _oryxMetadata;

    internal async Task RunAsync()
    {
        await LoadZsaOryxMedadata();
        await LoadZsaOryxGlyphs();
        List<KeyDefinition> ezKeys = PrepareEZLayoutKeys();
        WriteJsonFile(ezKeys, KeyDefinitionOutputFilename);
        WriteJsonFile(_oryxMetadata!.Categories.OrderBy(c => c.CategoryId), KeyCategoriesOutputFilename);
    }

    private async Task LoadZsaOryxMedadata()
    {
        using var client = new HttpClient();
        var metadata = await client.GetStringAsync(MetadataUrl);
        _oryxMetadata = JsonConvert.DeserializeObject<OryxMetadataModel>(metadata);

        // Special Cases
        _oryxMetadata.Keys.FirstOrDefault(k => k.Code == "MOD_LGUI")!.Label = "Left Windows";
        _oryxMetadata.Keys.FirstOrDefault(k => k.Code == "MOD_RGUI")!.Label = "Right Windows";
        _oryxMetadata.Keys.FirstOrDefault(k => k.Code == "KC_LALT")!.Label = "Left Alt";
        _oryxMetadata.Keys.FirstOrDefault(k => k.Code == "KC_RALT")!.Label = "Right Alt";
        _oryxMetadata.Keys.FirstOrDefault(k => k.Code == "MOD_LALT")!.Label = "Left Alt";
        _oryxMetadata.Keys.FirstOrDefault(k => k.Code == "MOD_RALT")!.Label = "Right Alt";
        _oryxMetadata.Keys.FirstOrDefault(k => k.Code == "KC_F14")!.Label = "F14";
        _oryxMetadata.Keys.FirstOrDefault(k => k.Code == "KC_F15")!.Label = "F15";
        _oryxMetadata.Keys.FirstOrDefault(k => k.Code == "KC_LGUI")!.GlyphName = "windows_left";
        _oryxMetadata.Keys.FirstOrDefault(k => k.Code == "KC_LGUI")!.Label = "Left Windows";
        _oryxMetadata.Keys.FirstOrDefault(k => k.Code == "KC_RGUI")!.GlyphName = "windows_right";
        _oryxMetadata.Keys.FirstOrDefault(k => k.Code == "KC_RGUI")!.Label = "Right Windows";
    }

    private async Task LoadZsaOryxGlyphs()
    {
        using var client = new HttpClient();
        var glyphCss = await client.GetStringAsync(OryxGlyphUrl);

        foreach (Match match in Regex.Matches(glyphCss, GlyphPattern, RegexOptions.IgnoreCase))
        {
            var keys = _oryxMetadata!.Keys.Where(k => k.GlyphName == match.Groups["glyphName"].Value);
            foreach (var key in keys)
            {
                key.GlyphCode = @$"\u{match.Groups["glyphCode"].Value}";
            }
        }
    }

    private List<KeyDefinition> PrepareEZLayoutKeys()
    {
        var ezKeys = _oryxMetadata!.Keys.Select((k) => (KeyDefinition) k).ToList();

        return ezKeys;
    }

    private static void WriteJsonFile(object dataToWrite, string outputFilename)
    {
        var json = JsonConvert.SerializeObject(dataToWrite);
        json = json.Replace(@"\\u", @"\u");

        File.WriteAllText(outputFilename, json);
    }
}