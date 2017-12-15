using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WishlistApp.Annotations;
using WishlistApp.Models;
using WishlistApp.Utils;

namespace WishlistApp.Viewmodels
{
    public class WensWijzigenViewModel: INotifyPropertyChanged
    {

        public HttpClient Client { get; set; }
        public RelayCommand EditCommand => new RelayCommand((param) => { WensWijzigen(); });

        private string _nieuweWensTitel;
        public string NieuweWensTitel
        {
            get { return _nieuweWensTitel; }
            set { _nieuweWensTitel = value; OnPropertyChanged(nameof(NieuweWensTitel)); }
        }

        private string _nieuweWensOmschrijving;
        public string NieuweWensOmschrijving
        {
            get { return _nieuweWensOmschrijving; }
            set { _nieuweWensOmschrijving = value; OnPropertyChanged(nameof(NieuweWensOmschrijving)); }
        }

        private Categorie _geselecteerdeCategorie;
        public Categorie GeselecteerdeCategorie
        {
            get { return _geselecteerdeCategorie; }
            set { _geselecteerdeCategorie = value; OnPropertyChanged(nameof(GeselecteerdeCategorie)); }
        }

        private Wens _wens;
        public Wens Wens
        {
            get { return _wens; }
            set { _wens = value; OnPropertyChanged(nameof(Wens)); }
        }

        public List<Categorie> CategorieLijst { get; set; }
        

        public WensWijzigenViewModel()
        {
            CategorieLijst = Enum.GetValues(typeof(Categorie)).Cast<Categorie>().ToList();
            Client = new HttpClient();
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", localSettings.Values["token"].ToString());
        }

        private async void WensWijzigen()
        {
            var json = JsonConvert.SerializeObject(Wens);
            var res = await Client.PutAsync(new Uri("http://localhost:58253/api/Wensen" + Wens.Id),
                new StringContent(json, Encoding.UTF8, "application/json"));
            var jsonResp = await res.Content.ReadAsStringAsync();
            var gewijzigdeWens = JsonConvert.DeserializeObject<Wens>(jsonResp);
            Debug.WriteLine(gewijzigdeWens);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
