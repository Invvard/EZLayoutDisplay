namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface
{
    public interface ILayoutService
    {
        /// <summary>
        /// Gets the <see cref="ErgodoxLayout"/>.
        /// </summary>
        /// <param name="layoutUrl">The layout URL to get.</param>
        /// <returns>The prepared EZLayout.</returns>
        EZLayout GetErgodoxLayout(string layoutUrl);
    }
}