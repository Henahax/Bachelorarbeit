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

        //TODO: Dialog return Object
    }
}
