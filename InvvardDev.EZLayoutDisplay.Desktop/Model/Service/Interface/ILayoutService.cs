using System.Threading.Tasks;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface
{
    public interface ILayoutService
    {
        /// <summary>
        /// Gets the <see cref="ErgodoxLayout"/>.
        /// </summary>
        /// <param name="layoutHashId">The layout hash ID to get.</param>
        /// <returns>The <see cref="ErgodoxLayout"/>.</returns>
        Task<ErgodoxLayout> GetErgodoxLayout(string layoutHashId);
    }
}