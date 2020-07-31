using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;

namespace InvvardDev.EZLayoutDisplay.Desktop.Service.Design
{
    public class LayoutService : ILayoutService
    {
        public async Task<ErgodoxLayout> GetLayoutInfo(string layoutHashId, string layoutRevisionId)
        {
            Debug.WriteLine("Layout retrieved.");

            var layoutInfo = new ErgodoxLayout();
            layoutInfo.Title = "Layout title v1.0";

            return await new Task<ErgodoxLayout>(() => layoutInfo);
        }

        /// <inheritdoc />
        public async Task<ErgodoxLayout> GetErgodoxLayout(string layoutHashId, string layoutRevisionId)
        {
            Debug.WriteLine("Layout retrieved.");

            return await new Task<ErgodoxLayout>(() => new ErgodoxLayout());
        }

        /// <inheritdoc />
        public EZLayout PrepareEZLayout(ErgodoxLayout ergodoxLayouts)
        {
            Debug.WriteLine("Layout prepared");

            return new EZLayout();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<KeyTemplate>> GetLayoutTemplate()
        {
            var layoutTemplate = new Task<IEnumerable<KeyTemplate>>(() => new List<KeyTemplate>());

            return await layoutTemplate;
        }
    }
}