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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WishlistApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        private Frame _rootFrame;

        public Login()
        {
            _rootFrame = Window.Current.Content as Frame;
            if (_rootFrame != null)
            {
                _rootFrame.ContentTransitions = new TransitionCollection();
                _rootFrame.ContentTransitions.Add(new NavigationThemeTransition());
            }

            this.InitializeComponent();
        }


        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var username = Username.Text;
            var password = Password.Password;

            if (Validate(username, password))
            {
                Windows.Storage.ApplicationDataContainer localSettings =
                    Windows.Storage.ApplicationData.Current.LocalSettings;

                localSettings.Values["username"] = "";
                localSettings.Values["token"] = "";

                _rootFrame.Navigate(typeof(MainPage), null, new DrillInNavigationTransitionInfo());
            }
            
        }

        private void Registreer_Click(object sender, RoutedEventArgs e)
        {
            _rootFrame.Navigate(typeof(Registreer), null, new SlideNavigationTransitionInfo());
        }

        private bool Validate(string username, string password)
        {
            bool validationOk = true;

            if (string.IsNullOrEmpty(username))
            {
                validationOk = false;
                UsernameError.Text = "Verplicht";
                UsernameError.Visibility = Visibility.Visible;
            }
            else
            {
                UsernameError.Visibility = Visibility.Collapsed;
            }

            if (string.IsNullOrEmpty(password))
            {
                validationOk = false;
                PasswordError.Text = "Verplicht";
                PasswordError.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordError.Visibility = Visibility.Collapsed;
            }

            return validationOk;
        }
    }
}
