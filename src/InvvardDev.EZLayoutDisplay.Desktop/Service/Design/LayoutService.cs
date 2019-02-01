using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;

namespace InvvardDev.EZLayoutDisplay.Desktop.Service.Design
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

        public ObservableCollection<KeyTemplate> GetLayoutTemplate()
        {
            var layoutTemplate = new ObservableCollection<KeyTemplate> {
                                                                           new KeyTemplate(0, 0, 1.5, vOffset: 0.37),
                                                                           new KeyTemplate(1.5, 0, vOffset: 0.37, isGlowing: true),
                                                                           new KeyTemplate(2.5, 0, vOffset: 129, isGlowing: true),
                                                                           new KeyTemplate(3.5, 0, isGlowing: true),
                                                                           new KeyTemplate(0, 1, 1.5, vOffset:.37, isGlowing: true),
                                                                           new KeyTemplate(1.5, 1, vOffset:.37, isGlowing: true),
                                                                           new KeyTemplate(2.5, 1, vOffset:.129, isGlowing: true),
                                                                           new KeyTemplate(3.5, 1, isGlowing: true),
                                                                       };



            return layoutTemplate;
        }
    }
}