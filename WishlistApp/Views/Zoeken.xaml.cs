using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using WishlistApp.Annotations;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WishlistApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Zoeken : Page, INotifyPropertyChanged
    {
        public List<Object> ResultLijst { get; set; }
        public string ZoekString { get; set; }

        private readonly List<string> _lijstSoorten;
        private string _zoekSoort;

        public List<string> LijstSoorten
        {
            get { return _lijstSoorten; }
        }

        public string ZoekSoort
        {
            get { return _zoekSoort; }
            set
            {
                if (_zoekSoort == value) return;
                _zoekSoort = value;
                OnPropertyChanged("ZoekSoort");
            }
        }
        public Zoeken()
        {
            this.InitializeComponent();
            _lijstSoorten = new List<string> { "Gebruiker", "Wishlist" };
        }

        private void Submit_OnClick(object sender, RoutedEventArgs e)
        {
            if (_zoekSoort == "Gebruiker")
            {

            }
            else
            {
                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
