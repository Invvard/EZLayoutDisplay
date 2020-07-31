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
        /// <param name="layoutRevisionId">The layout revision ID to get.</param>
        /// <returns>The <see cref="ErgodoxLayout"/>.</returns>
        Task<ErgodoxLayout> GetLayoutInfo(string layoutHashId, string layoutRevisionId);

        /// <summary>
        /// Gets the <see cref="ErgodoxLayout"/>.
        /// </summary>
        /// <param name="layoutHashId">The layout hash ID to get.</param>
        /// <param name="layoutRevisionId">The layout revision ID to get.</param>
        /// <returns>The <see cref="ErgodoxLayout"/>.</returns>
        Task<ErgodoxLayout> GetErgodoxLayout(string layoutHashId, string layoutRevisionId);

        /// <summary>
        /// Transforms an <see cref="ErgodoxLayout"/> into a <see cref="EZLayout"/>.
        /// </summary>
        /// <param name="ergodoxLayout">The <see cref="ErgodoxLayout"/> to be transformed.</param>
        /// <param name="layoutRevisionId">The layout revision identifier.</param>
        /// <returns>The <see cref="EZLayout"/> transformed into.</returns>
        EZLayout PrepareEZLayout(ErgodoxLayout ergodoxLayout, string layoutRevisionId);

        /// <summary>
        /// Gets the list of <see cref="KeyTemplate"/> from the local repository.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{KeyTemplate}"/></returns>
        Task<IEnumerable<KeyTemplate>> GetLayoutTemplate();
    }
}