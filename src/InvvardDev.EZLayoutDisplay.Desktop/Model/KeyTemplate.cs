using System.ComponentModel;
using Newtonsoft.Json;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
    public class KeyTemplate
    {
        private const int KeyUnitSize = 54;

        /// <summary>
        /// Gets or sets the key X position.
        /// </summary>
        [JsonProperty("x")]
        public double BaseRelativeLeft { get; set; }

        /// <summary>
        /// Gets or sets the key Y position.
        /// </summary>
        [JsonProperty("y")]
        public double BaseRelativeTop { get; set; }

        /// <summary>
        /// Gets or sets the key .
        /// </summary>
        [JsonProperty("width", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(1)]
        public double BaseRelativeWidth { get; set; }

        /// <summary>
        /// Gets or sets the key .
        /// </summary>
        [JsonProperty("height", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(1)]
        public double BaseRelativeHeight { get; set; }

        /// <summary>
        /// Gets or sets the key .
        /// </summary>
        [JsonProperty("voffset", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(0)]
        public double VerticalOffset { get; set; }

        /// <summary>
        /// Gets or sets the key .
        /// </summary>
        [JsonProperty("hoffset", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(0)]
        public double HorizontalOffset { get; set; }

        /// <summary>
        /// Gets or sets the key .
        /// </summary>
        [JsonProperty("rotate", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(0)]
        public int Rotate { get; set; }

        /// <summary>
        /// Gets or sets the key .
        /// </summary>
        //[JsonProperty("origin", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(false)]
        public bool IsTopRightRotationOrigin { get; set; }

        /// <summary>
        /// Gets or sets the key .
        /// </summary>
        //[JsonProperty("glow", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(false)]
        public bool IsGlowing { get; set; }

        public double BaseKeyX => HorizontalOffset * KeyUnitSize + BaseRelativeLeft * KeyUnitSize;
        public double BaseKeyY => VerticalOffset * KeyUnitSize + BaseRelativeTop * KeyUnitSize;
        public double TopKeyX => BaseKeyX + 3;
        public double TopKeyY => BaseKeyY + 3;

        public double BaseKeyWidth => BaseRelativeWidth * KeyUnitSize;
        public double BaseKeyHeight => BaseRelativeHeight * KeyUnitSize;
        public double TopKeyWidth => BaseKeyWidth - 6;
        public double TopKeyHeight => BaseKeyHeight - 11;

        public KeyTemplate()
        {
            
        }

        public KeyTemplate(double x, double y, double width = 1, double height = 1, double vOffset = 0, double hOffset = 0, int rotate = 0, bool isOriginTopRight = false, bool isGlowing = false)
        {
            BaseRelativeLeft = x;
            BaseRelativeTop = y;
            BaseRelativeWidth = width;
            BaseRelativeHeight = height;
            VerticalOffset = vOffset;
            HorizontalOffset = hOffset;
            Rotate = rotate;
            IsTopRightRotationOrigin = isOriginTopRight;
            IsGlowing = isGlowing;
        }
    }
}