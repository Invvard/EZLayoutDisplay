﻿using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Implementation;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;
using NLog;

namespace InvvardDev.EZLayoutDisplay.Desktop.ViewModel
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
                SimpleIoc.Default.Register<IWindowService, Service.Design.WindowService>();
                SimpleIoc.Default.Register<IKeyboardHookService, Service.Design.KeyboardHookService>();
                SimpleIoc.Default.Register<ISettingsService, Service.Design.SettingsService>();
                SimpleIoc.Default.Register<IApplicationService, Service.Design.ApplicationService>();
                SimpleIoc.Default.Register<ILayoutService, Service.Design.LayoutService>();
                SimpleIoc.Default.Register<IProcessService, Service.Design.ProcessService>();
            }
            else
            {
                SimpleIoc.Default.Register<IWindowService, WindowService>();
                SimpleIoc.Default.Register<ISettingsService>(() => new SettingsService(Properties.Settings.Default));
                SimpleIoc.Default.Register<IKeyboardHookService, KeyboardHookService>(true);
                SimpleIoc.Default.Register<IApplicationService, ApplicationService>();
                SimpleIoc.Default.Register<ILayoutService, LayoutService>();
                SimpleIoc.Default.Register<IProcessService, ProcessService>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<DisplayLayoutViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<AboutViewModel>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public DisplayLayoutViewModel DisplayLayout => ServiceLocator.Current.GetInstance<DisplayLayoutViewModel>();

        public SettingsViewModel Settings => ServiceLocator.Current.GetInstance<SettingsViewModel>();

        public AboutViewModel About => ServiceLocator.Current.GetInstance<AboutViewModel>();
        
        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
            SimpleIoc.Default.GetInstance<IKeyboardHookService>()?.Dispose();
            LogManager.Shutdown();
        }
    }
}