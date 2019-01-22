namespace InvvardDev.EZLayoutDisplay.Desktop.Model {
    public class Key
    {
        /// <summary>
        /// Gets or sets the supported OS name.
        /// </summary>
        public string Os { get; set; }

        /// <summary>
        /// Gets or sets the key code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the Color.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the layer index.
        /// </summary>
        public int? Layer { get; set; }

        /// <summary>
        /// Gets or sets the primary command.
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Gets or sets the GlowColor.
        /// </summary>
        public string GlowColor { get; set; }

        /// <summary>
        /// Gets or sets the modifiers.
        /// </summary>
        public Modifiers Modifiers { get; set; }
    }
}