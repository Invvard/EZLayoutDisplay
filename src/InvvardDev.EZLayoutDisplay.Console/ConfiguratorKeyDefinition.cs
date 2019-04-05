namespace InvvardDev.EZLayoutDisplay.Console
{
    public class ConfiguratorKeyDefinition
    {
        public string KeyCode { get; set; }
        public string Label { get; set; }
        public string MenuLabel { get; set; }
        public string Glyph { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Command { get; set; }
        public bool? PrecedingKey { get; set; }
    }
}