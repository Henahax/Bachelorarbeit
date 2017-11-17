using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bachelorarbeit
{
    /// <summary>
    /// Interaction logic for KundenAuswahl.xaml
    /// </summary>
    public partial class KundenAuswahl : Window
    {
        mainEntities _entities;
        CollectionViewSource _kundenViewSource;
        String kundenFilter = "";
        public kunden kunde;

        public KundenAuswahl()
        {
            InitializeComponent();
            _kundenViewSource = ((CollectionViewSource)(this.FindResource("kundenViewSource")));
            Refresh();
        }

        private void Refresh()
        {
            if (_entities != null)
            {
                _entities.Dispose();
            }
            _entities = new mainEntities();

            _entities.kunden.Load();
            _kundenViewSource.Source = _entities.kunden.Local;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource kundenViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("kundenViewSource")));
        }

        private void Auswaehlen(object sender, RoutedEventArgs e)
        {
            this.kunde = (kunden)kundenAuswahlListe.SelectedItem;
            this.Close();
        }

        private void Ausgewaehlt(object sender, SelectedCellsChangedEventArgs e)
        {
            if ((kunden)kundenAuswahlListe.SelectedItem != null)
            {
                kundenAuswahlAuswaehlen.IsEnabled = true;
            }
            else
            {
                kundenAuswahlAuswaehlen.IsEnabled = false;
            }
        }

        private void KundenFilter(object sender, FilterEventArgs e)
        {
            kunden kunde = e.Item as kunden;
            if (kunde.titel.ToLower().Contains(kundenFilter.ToLower()) || kunde.vorname.ToLower().Contains(kundenFilter.ToLower()) || kunde.nachname.ToLower().Contains(kundenFilter.ToLower()) || kunde.firma.ToLower().Contains(kundenFilter.ToLower()) || kunde.strasse.ToLower().Contains(kundenFilter.ToLower()) || kunde.postleitzahl.ToLower().Contains(kundenFilter.ToLower()) || kunde.ort.ToLower().Contains(kundenFilter.ToLower()) || kunde.land.ToLower().Contains(kundenFilter.ToLower()) || kunde.telefon.ToLower().Contains(kundenFilter.ToLower()) || kunde.telefax.ToLower().Contains(kundenFilter.ToLower()) || kunde.mobiltelefon.ToLower().Contains(kundenFilter.ToLower()) || kunde.email.ToLower().Contains(kundenFilter.ToLower()) || kunde.webseite.ToLower().Contains(kundenFilter.ToLower()) || kunde.notizen.ToLower().Contains(kundenFilter.ToLower()))
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }
        }

        private void KundenSucheEingeben(object sender, TextChangedEventArgs e)
        {
            kundenFilter = suche.Text;
            Refresh();
            kundenAuswahlListe.SelectedItem = null;
        }
    }
}
