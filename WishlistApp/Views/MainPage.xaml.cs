using System;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using WishlistApp.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WishlistApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public new static Frame Frame { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            Frame = mainFrame;
            mainFrame.Navigate(typeof(WishlistsOverzicht));
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationPane.IsPaneOpen = !NavigationPane.IsPaneOpen;
        }

        private void Zoeken_Click(object sender, TappedRoutedEventArgs e)
        {
            mainFrame.Navigate(typeof(Zoeken));
            NavigationPane.IsPaneOpen = false;
        }

        private void Wishlists_Click(object sender, TappedRoutedEventArgs e)
        {
            mainFrame.Navigate(typeof(WishlistsOverzicht));
            NavigationPane.IsPaneOpen = false;
        }

        private void Profiel_Click(object sender, TappedRoutedEventArgs e)
        {
            mainFrame.Navigate(typeof(Profiel));
            NavigationPane.IsPaneOpen = false;
        }

        private void Uitnodigingen_Click(object sender, TappedRoutedEventArgs e)
        {
            mainFrame.Navigate(typeof(UitnodigingRequest));
            NavigationPane.IsPaneOpen = false;
        }

        private void Logout_Click(object sender, TappedRoutedEventArgs e)
        {
            if (Window.Current.Content is Frame rootFrame)
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                
                localSettings.Values["username"] = null;
                localSettings.Values["token"] = null;

                rootFrame.ContentTransitions = new TransitionCollection {new NavigationThemeTransition()};
                rootFrame.Navigate(typeof(Login), null, new ContinuumNavigationTransitionInfo());
            }
        }
    }
}