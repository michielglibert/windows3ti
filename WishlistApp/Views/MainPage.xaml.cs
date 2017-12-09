﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WishlistApp.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WishlistApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationPane.IsPaneOpen = !NavigationPane.IsPaneOpen;
        }

        private void Zoeken_Click(object sender, TappedRoutedEventArgs e)
        {
            mainFrame.Navigate(typeof(Zoeken));
        }

        private void Wishlists_Click(object sender, TappedRoutedEventArgs e)
        {
            mainFrame.Navigate(typeof(WishlistsOverzicht));
        }

        private void Profiel_Click(object sender, TappedRoutedEventArgs e)
        {
            mainFrame.Navigate(typeof(Profiel));
        }

        private void Uitnodigingen_Click(object sender, TappedRoutedEventArgs e)
        {
            mainFrame.Navigate(typeof(Profiel));
        }
    }
}