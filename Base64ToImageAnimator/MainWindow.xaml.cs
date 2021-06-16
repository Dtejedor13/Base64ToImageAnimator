﻿using Base64ToImageAnimator.ViewModels;
using System.Windows;

namespace Base64ToImageAnimator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((ViewModel)(DataContext)).LunchAnnimation();
        }
    }
}
