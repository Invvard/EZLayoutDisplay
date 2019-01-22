namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface
{
    public interface ILayoutService
    {
        /// <summary>
        /// Gets the <see cref="ErgodoxLayout"/>.
        /// </summary>
        /// <param name="layoutUrl"></param>
        /// <returns></returns>
        ErgodoxLayout GetErgodoxLayout(string layoutUrl);
    }
}