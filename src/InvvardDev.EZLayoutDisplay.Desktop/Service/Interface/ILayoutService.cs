using System.Collections.Generic;
using System.Threading.Tasks;
using InvvardDev.EZLayoutDisplay.Desktop.Model;

namespace InvvardDev.EZLayoutDisplay.Desktop.Service.Interface
{
    public interface ILayoutService
    {
        /// <summary>
        /// Gets the <see cref="ErgodoxLayout"/> basic info.
        /// </summary>
        /// <param name="layoutHashId">The layout hash ID to get.</param>
        /// <param name="geometry">The keyboard geometry to get.</param>
        /// <param name="layoutRevisionId">The layout revision ID to get.</param>
        /// <returns>The <see cref="ErgodoxLayout"/>.</returns>
        Task<ErgodoxLayout> GetLayoutInfo(string layoutHashId, string geometry, string layoutRevisionId);

        /// <summary>
        /// Gets the <see cref="ErgodoxLayout"/>.
        /// </summary>
        /// <param name="layoutHashId">The layout hash ID to get.</param>
        /// <param name="geometry">The keyboard geometry to get.</param>
        /// <param name="layoutRevisionId">The layout revision ID to get.</param>
        /// <returns>The <see cref="ErgodoxLayout"/>.</returns>
        Task<ErgodoxLayout> GetErgodoxLayout(string layoutHashId, string geometry, string layoutRevisionId);

        /// <summary>
        /// Transforms an <see cref="ErgodoxLayout"/> into a <see cref="EZLayout"/>.
        /// </summary>
        /// <param name="ergodoxLayout">The <see cref="ErgodoxLayout"/> to be transformed.</param>
        /// <returns>The <see cref="EZLayout"/> transformed into.</returns>
        EZLayout PrepareEZLayout(ErgodoxLayout ergodoxLayout);

        /// <summary>
        /// Gets the list of <see cref="KeyTemplate"/> from the local repository.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{KeyTemplate}"/></returns>
        Task<IEnumerable<KeyTemplate>> GetLayoutTemplate(string geometry);

        /// <summary>
        /// Returns whether the keyboard geometry is supported.
        /// </summary>
        /// <returns>true if supported, false if not</returns>
        bool SupportsGeometry(string geometry);
    }
}