using System.Collections.Generic;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
    public class EZModifier
    {
        /// <summary>
        /// Indicate the label size.
        /// </summary>
        public enum LabelSize
        {
            /// <summary>
            /// Used when 3+ modifiers are applied.
            /// </summary>
            Small,

            /// <summary>
            /// Used when 2 modifiers are applied.
            /// </summary>
            Medium,

            /// <summary>
            /// Used when 1 modifier is applied.
            /// </summary>
            Large,
        }

        /// <summary>
        /// Gets the modifier index.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Gets the <see cref="KeyModifier"/> type.
        /// </summary>
        public KeyModifier KeyModifier { get; }

        /// <summary>
        /// Gets the labels.
        /// </summary>
        public Dictionary<LabelSize, string> Labels { get; }

        public EZModifier(KeyModifier keyModifier, int index, string smallLabel, string mediumLabel, string largeLabel)
        {
            KeyModifier = keyModifier;
            Index = index;
            Labels = new Dictionary<LabelSize, string> {
                                                           { LabelSize.Small, smallLabel },
                                                           { LabelSize.Medium, mediumLabel },
                                                           { LabelSize.Large, largeLabel }
                                                       };
        }
    }
}