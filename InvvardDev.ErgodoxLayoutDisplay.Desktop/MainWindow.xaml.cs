﻿using System.Windows;
using InvvardDev.ErgodoxLayoutDisplay.Desktop.ViewModel;

namespace InvvardDev.ErgodoxLayoutDisplay.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }
    }
}