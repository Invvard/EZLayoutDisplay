using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using InvvardDev.ErgodoxLayoutDisplay.Desktop.Model.Service.Implementation;
using InvvardDev.ErgodoxLayoutDisplay.Desktop.Model.Service.Interface;
using Microsoft.Practices.ServiceLocation;

namespace InvvardDev.ErgodoxLayoutDisplay.Desktop.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IKeyboardHookService, Design.KeyboardHookService>();
            }
            else
            {
                SimpleIoc.Default.Register<IKeyboardHookService, KeyboardHookService>(true);
            }

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<DisplayLayoutViewModel>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public DisplayLayoutViewModel DisplayLayout => ServiceLocator.Current.GetInstance<DisplayLayoutViewModel>();
        
        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
            SimpleIoc.Default.GetInstance<IKeyboardHookService>()?.Dispose();
        }
    }
}