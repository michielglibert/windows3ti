using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WishlistApp.Annotations;
using WishlistApp.Models;

namespace WishlistApp.Viewmodels
{
    class WensViewModel:INotifyPropertyChanged
    {
        public HttpClient Client { get; set; }

        private Wens _wens;
        public Wens Wens
        {
            get => _wens;
            set { _wens = value; OnPropertyChanged(nameof(Wens)); }
        }

        public WensViewModel()
        {
            Client = new HttpClient();
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());
        }

        public void InitData()
        {
            GetWens();
        }

        public async void GetWens()
        {
            var json = await Client.GetStringAsync(new Uri("http://localhost:58253/api/Wensen/" + Wens.Id));
            var wens = JsonConvert.DeserializeObject<Wens>(json);
            Wens = wens;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
