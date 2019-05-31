﻿using System.Windows;

namespace InvvardDev.EZLayoutDisplay.Desktop.View
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();

#if DEBUG
            BtnUpdate.IsEnabled = true;
            BtnUpdate.Visibility = Visibility.Visible;
#endif
        }
    }
}