﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;
using WishlisApp.Models;
using WishlistApp.Annotations;
using WishlistApp.Models;
using WishlistApp.Utils;

namespace WishlistApp.Viewmodels
{
    class UitnodigingViewModel:INotifyPropertyChanged
    {
        public HttpClient Client { get; set; }

        private Uitnodiging _selectedUitnodiging;

        public Uitnodiging SelectedUitnodiging
        {
            get => _selectedUitnodiging;
            set { _selectedUitnodiging = value; OnPropertyChanged(nameof(SelectedUitnodiging)); }
        }

        public RelayCommand OpenUitnodigingDialogCommand => new RelayCommand((uitnodiging) => { ShowUitnodigingDialog((Uitnodiging)uitnodiging); });


        public RelayCommand UitnodigingAanvaarden => new RelayCommand((uitnodiging) => { UitnodigingBeantwoorden((Uitnodiging) uitnodiging, true); });
        public RelayCommand UitnodigingAfwijzen => new RelayCommand((uitnodiging) => { UitnodigingBeantwoorden((Uitnodiging)uitnodiging, false); });


        private ObservableCollection<Uitnodiging> _uitnodigingen;

        public ObservableCollection<Uitnodiging> Uitnodigingen
        {
            get => _uitnodigingen;
            set { _uitnodigingen = value; OnPropertyChanged(nameof(Uitnodigingen)); }
        }

        private ObservableCollection<Request> _requests;

        public ObservableCollection<Request> Requests
        {
            get => _requests;
            set { _requests = value; OnPropertyChanged(nameof(Requests)); }
        }

        public UitnodigingViewModel()
        {
            Client = new HttpClient();
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());

            GetUitnodigingen();
            GetRequests();
        }

        public async void GetUitnodigingen()
        {
            var json = await Client.GetStringAsync(new Uri("http://localhost:58253/api/Uitnodigingen"));
            var lst = JsonConvert.DeserializeObject<ObservableCollection<Uitnodiging>>(json);
            Uitnodigingen = lst;
        }

        public async void UitnodigingBeantwoorden(Uitnodiging u, bool antwoord)
        {
            var json = JsonConvert.SerializeObject(new {Antwoord = antwoord});

            HttpRequestMessage request = new HttpRequestMessage
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
                Method = HttpMethod.Delete,
                RequestUri = new Uri("http://localhost:58253/api/Uitnodigingen/" + u.Id)
            };

            var resp = await Client.SendAsync(request);

            if (resp.StatusCode == HttpStatusCode.NoContent)
            {
                Uitnodigingen.Remove(u);
            }
            
       }

        public async void GetRequests()
        {
            var json = await Client.GetStringAsync(new Uri("http://localhost:58253/api/Requests"));
            var lst = JsonConvert.DeserializeObject<ObservableCollection<Request>>(json);
            Requests = lst;
        }


        public async void ShowUitnodigingDialog(Uitnodiging uitnodiging)
        {
            ContentDialog uitnodigingDialog = new ContentDialog
            {
                Title = "Uitnodiging",
                Content = "Wat wil je met deze uitnodiging doen?",
                PrimaryButtonText = "Aanvaarden",
                SecondaryButtonText = "Afwijzen",
                PrimaryButtonCommand = UitnodigingAanvaarden,
                PrimaryButtonCommandParameter = uitnodiging,
                SecondaryButtonCommand = UitnodigingAfwijzen,
                SecondaryButtonCommandParameter = uitnodiging
            };

            await uitnodigingDialog.ShowAsync();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
