using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
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


        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            var username = Username.Text;
            var password = Password.Password;

            if (Validate(username, password))
            {
                var client = new HttpClient();

                var json = JsonConvert.SerializeObject(new {Username = username, Password = password});
                var resp = await client.PostAsync(new Uri("http://localhost:58253/api/Authentication/login"),
                    new StringContent(json, Encoding.UTF8, "application/json"));

                if (resp.StatusCode == HttpStatusCode.BadRequest)
                {
                    PasswordError.Text = "Foute gebruikersnaam of wachtwoord";
                    PasswordError.Visibility = Visibility.Visible;
                }
                else
                {
                    var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

                    var jsonResp = await resp.Content.ReadAsStringAsync();
                    var djson = JsonConvert.DeserializeObject<LoginResponse>(jsonResp);

                    localSettings.Values["username"] = djson.Username;
                    localSettings.Values["token"] = djson.Token;

                    _rootFrame.Navigate(typeof(MainPage), null, new DrillInNavigationTransitionInfo());
                }
                
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


public class LoginResponse
{
    public string Username { get; set; }
    public string Token { get; set; }
}