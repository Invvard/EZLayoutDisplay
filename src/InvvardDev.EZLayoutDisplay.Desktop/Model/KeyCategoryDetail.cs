using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
    internal class KeyCategoryDetail
    {
        /// <summary>
        /// Gets the <see cref="KeyCategory"/>.
        /// </summary>
        public KeyCategory Category { get; }

        /// <summary>
        /// Gets the <see cref="KeyCategory"/> title.
        /// </summary>
        public string Title { get; }

        public KeyCategoryDetail(KeyCategory category, string title)
        {
            Category = category;
            Title = title;
        }
    }
}