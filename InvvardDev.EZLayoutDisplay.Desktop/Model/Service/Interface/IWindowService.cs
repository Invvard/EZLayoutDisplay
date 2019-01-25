using System.Windows;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface
{
    public interface IWindowService
    {
        /// <summary>
        /// Shows the specified <see cref="Window"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Window"/>.</typeparam>
        void ShowWindow<T>()
            where T : Window, new();

        /// <summary>
        /// Closes the specified <see cref="Window"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Window"/>.</typeparam>
        void CloseWindow<T>();

        /// <summary>
        /// Shows a modal window.
        /// </summary>
        void ShowModalDialog();
    }
}