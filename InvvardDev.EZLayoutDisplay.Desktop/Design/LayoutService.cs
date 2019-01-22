using System.Diagnostics;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface;

namespace InvvardDev.EZLayoutDisplay.Desktop.Design
{
    public class LayoutService : ILayoutService
    {
        /// <inheritdoc />
        public EZLayout GetErgodoxLayout(string layoutUrl)
        {
            Debug.WriteLine("Layout retrieved.");

            return new EZLayout();
        }
    }
}