﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WishlistApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Registreer : Page
    {
        private Frame _rootFrame = Window.Current.Content as Frame;

        public Registreer()
        {
            this.InitializeComponent();
        }

        private async void Registreer_Click(object sender, RoutedEventArgs e)
        {
            var username = Username.Text;
            var password = Password.Password;
            var passwordherh = PasswordHerh.Password;

            if (Validate(username, password, passwordherh))
            {
                var client = new HttpClient();

                var json = JsonConvert.SerializeObject(new { Username = username, Password = password });
                var resp = await client.PostAsync(new Uri("http://localhost:58253/api/Authentication/register"),
                    new StringContent(json, Encoding.UTF8, "application/json"));

                if (resp.StatusCode == HttpStatusCode.BadRequest)
                {
                    PasswordErrorHerh.Text = "Registration failed";
                    PasswordErrorHerh.Visibility = Visibility.Visible;
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
        private void Annuleer_Click(object sender, RoutedEventArgs e)
        {

            _rootFrame.Navigate(typeof(Login));

        }

        private bool Validate(string username, string password, string passwordherh)
        {
            bool validationOk = true;

            if (string.IsNullOrEmpty(username))
            {
                validationOk = false;
                UsernameError.Text = "Verplicht";
                UsernameError.Visibility = Visibility.Visible;
            }
            else if (UsernameAlInGebruik(username))
            {
                validationOk = false;
                UsernameError.Text = "Deze gebruikersnaam is al in gebruik";
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
            else if (password.Length < 8)
            {
                validationOk = false;
                PasswordError.Text = "Wachtwoord moet minimum 8 tekens zijn";
                PasswordError.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordError.Visibility = Visibility.Collapsed;
            }

            if (string.IsNullOrEmpty(passwordherh))
            {
                validationOk = false;
                PasswordErrorHerh.Text = "Verplicht";
                PasswordErrorHerh.Visibility = Visibility.Visible;
            }
            else if (!passwordherh.Equals(password))
            {
                validationOk = false;
                PasswordErrorHerh.Text = "Wachtwoorden komen niet overeen";
                PasswordErrorHerh.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordErrorHerh.Visibility = Visibility.Collapsed;
            }

            return validationOk;
        }

        private bool UsernameAlInGebruik(string username)
        {
            return false;
        }
    }
}
