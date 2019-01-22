using InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Implementation
{
    public class LayoutService : ILayoutService
    {
        /// <inheritdoc />
        public EZLayout GetErgodoxLayout(string layoutUrl)
        {
            var ezLayout = new EZLayout();

            return ezLayout;
        }
    }
}