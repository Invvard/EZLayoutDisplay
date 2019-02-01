using System.Collections.ObjectModel;
using System.Threading.Tasks;
using InvvardDev.EZLayoutDisplay.Desktop.Model;

namespace InvvardDev.EZLayoutDisplay.Desktop.Service.Interface
{
    public interface ILayoutService
    {
        /// <summary>
        /// Gets the <see cref="ErgodoxLayout"/>.
        /// </summary>
        /// <param name="layoutHashId">The layout hash ID to get.</param>
        /// <returns>The <see cref="ErgodoxLayout"/>.</returns>
        Task<ErgodoxLayout> GetErgodoxLayout(string layoutHashId);

        /// <summary>
        /// Transforms an <see cref="ErgodoxLayout"/> into a <see cref="EZLayout"/>.
        /// </summary>
        /// <param name="ergodoxLayout">The <see cref="ErgodoxLayout"/> to be transformed.</param>
        /// <returns>The <see cref="EZLayout"/> transformed into.</returns>
        EZLayout PrepareEZLayout(ErgodoxLayout ergodoxLayout);

        /// <summary>
        /// Gets the list of <see cref="KeyTemplate"/>.
        /// </summary>
        /// <returns>An <see cref="ObservableCollection{KeyTemplate}"/></returns>
        ObservableCollection<KeyTemplate> GetLayoutTemplate();
    }
}