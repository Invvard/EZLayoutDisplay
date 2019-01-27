using System.Diagnostics;
using System.Threading.Tasks;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface;

namespace InvvardDev.EZLayoutDisplay.Desktop.Design
{
    public class LayoutService : ILayoutService
    {
        /// <inheritdoc />
        public async Task<ErgodoxLayout> GetErgodoxLayout(string layoutHashId)
        {
            Debug.WriteLine("Layout retrieved.");

            return await new Task<ErgodoxLayout>(() => new ErgodoxLayout());
        }

        /// <inheritdoc />
        public EZLayout PrepareEZLayout(ErgodoxLayout ergodoxLayout)
        {
            Debug.WriteLine("Layout prepared");

            return new EZLayout();
        }
    }
}