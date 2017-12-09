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
using Windows.UI.Xaml.Navigation;

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

            ResizeOptions();
        }

        private void StackPanel_Tapped_2(object sender, TappedRoutedEventArgs e)
        {
            mainFrame.Navigate(typeof(Page1));
        }

        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //mainFrame.Navigate(typeof(Page2));
        }

        private void StackPanel_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            //mainFrame.Navigate(typeof(Page3));
        }

        private void Option1Button_Checked(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(typeof(Page1));
        }

        private void Option2Button_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void ResizeOptions()
        {
            // calculate the actual width of the navigation pane

            var width = NavigationPane.CompactPaneLength;
            if (NavigationPane.IsPaneOpen)
                width = NavigationPane.OpenPaneLength;

            // change the width of all control in the navigation pane

            HamburgerButton.Width = width;

            foreach (var option in NavigationMenu.Children)
            {
                var radioButton = (option as RadioButton);
                if (radioButton != null)
                    radioButton.Width = width;
            }
        }
    }
}
