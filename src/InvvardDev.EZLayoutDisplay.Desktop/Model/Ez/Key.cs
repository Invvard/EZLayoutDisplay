using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Ez.Content;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Ez
{
    public class Key
    {
        public BaseContent Primary { get; set; }

        public BaseContent Secondary { get; set;}

        /// <summary>
        /// Gets or sets the key glowing color.
        /// </summary>
        public string GlowColor { get; set; }

        /// <summary>
        /// Gets or sets the key category.
        /// </summary>
        public KeyCategory Category { get; set; }
    }
}