using System;
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
        /// Shows a modal dialog that warns the user of something.
        /// </summary>
        /// <param name="warningMessage">The warning message to display.</param>
        /// <returns><c>True</c> if user clicks on the OK button.</returns>
        bool ShowWarning(string warningMessage);
    }
}