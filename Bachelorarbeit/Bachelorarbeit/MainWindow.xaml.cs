using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Data.Entity;
using System.Data;
using System.Linq;

namespace Bachelorarbeit
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        mainEntities _entities;
        CollectionViewSource _kundenViewSource;
        bool neuerKundeWirdAngelegt = false;


        public MainWindow()
        {
            //TODO: Bei Datenbankänderungen, Identify Spalte neu konfigurieren im Model
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

        private void EinstellungenOeffnen(object sender, RoutedEventArgs e)
        {
            Einstellungen einstellungen = new Einstellungen();
            einstellungen.ShowDialog();
        }

        private void SucheEingeben(object sender, TextChangedEventArgs e)
        {
            //KundenlisteFuellen();
        }


        private void KundenlisteBearbeiten(object sender, RoutedEventArgs e)
        {
            kunden kunde = (kunden)kundenListe.SelectedItem;

            groupBoxKunde.IsEnabled = true;
            groupBoxKunden.IsEnabled = false;

            kundeKundennummer.Content = kunde.kundennummer;
            kundeAnrede.Text = kunde.anrede;
            kundeVorname.Text = kunde.vorname;
            kundeNachname.Text = kunde.nachname;
            kundeFirma.Text = kunde.firma;
            kundeStraße.Text = kunde.strasse;
            kundePostleitzahl.Text = kunde.postleitzahl;
            kundeOrt.Text = kunde.ort;
            kundeLand.Text = kunde.land;
            kundeTelefon.Text = kunde.telefon;
            kundeTelefax.Text = kunde.telefax;
            kundeMobiltelefon.Text = kunde.mobiltelefon;
            kundeEmail.Text = kunde.email;
            kundeWebseite.Text = kunde.webseite;
            kundeNotizen.Text = kunde.notizen;
        }

        private void KundenlisteLoeschen(object sender, RoutedEventArgs e)
        {
            kunden kunde = (kunden)kundenListe.SelectedItem;
            if (kunde.angebote.Count != 0 || kunde.rechnungen.Count != 0)
            {
                MessageBox.Show("Dieser Kunde kann nicht gelöscht werden, da ihm Rechnungen oder Angebote zugeordnet sind.", "Hinweis");
                return;
            }
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Sind sie sicher, dass sie diesen Kunden löschen wollen?", "Kunden Löschen Bestätigung", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {

                _entities.kunden.Remove(kunde);
                _entities.SaveChanges();
                Refresh();

                kundenListe.SelectedItem = null;
                kundenlisteBearbeiten.IsEnabled = false;
                kundenlisteLoeschen.IsEnabled = false;
            }
        }

        private void KundeSpeichern(object sender, RoutedEventArgs e)
        {
            if (neuerKundeWirdAngelegt == true)
            {
                kunden kunde = new kunden();

                long kundennummer;

                Int64.TryParse(kundeKundennummer.Content.ToString(), out kundennummer);
                kunde.kundennummer = kundennummer;
                kunde.anrede = kundeAnrede.Text;
                kunde.vorname = kundeVorname.Text;
                kunde.nachname = kundeNachname.Text;
                kunde.firma = kundeFirma.Text;
                kunde.strasse = kundeStraße.Text;
                kunde.postleitzahl = kundePostleitzahl.Text;
                kunde.ort = kundeOrt.Text;
                kunde.land = kundeLand.Text;
                kunde.telefon = kundeTelefon.Text;
                kunde.telefax = kundeTelefax.Text;
                kunde.mobiltelefon = kundeMobiltelefon.Text;
                kunde.email = kundeEmail.Text;
                kunde.webseite = kundeWebseite.Text;
                kunde.notizen = kundeNotizen.Text;

                if (kundeAnrede.SelectedItem == null)
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Anrede muss ausgefüllt sein!");
                    return;
                }
                if (kundeNachname.Text == "" && kundeFirma.Text == "")
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Nachname oder Firma muss ausgefüllt sein!");
                    return;
                }

                _entities.kunden.Add(kunde);
                _entities.SaveChanges();
                Refresh();

                groupBoxKunde.IsEnabled = false;
                groupBoxKunden.IsEnabled = true;

                kundeKundennummer.Content = null;
                kundeAnrede.Text = null;
                kundeVorname.Text = null;
                kundeNachname.Text = null;
                kundeFirma.Text = null;
                kundeStraße.Text = null;
                kundePostleitzahl.Text = null;
                kundeOrt.Text = null;
                kundeLand.Text = null;
                kundeTelefon.Text = null;
                kundeTelefax.Text = null;
                kundeMobiltelefon.Text = null;
                kundeEmail.Text = null;
                kundeWebseite.Text = null;
                kundeNotizen.Text = null;

                kundenListe.SelectedItem = null;
                kundenlisteBearbeiten.IsEnabled = false;
                kundenlisteLoeschen.IsEnabled = false;
                neuerKundeWirdAngelegt = false;
            }
            else
            {
                kunden kunde = (kunden)kundenListe.SelectedItem;

                kunde.anrede = kundeAnrede.Text;
                kunde.vorname = kundeVorname.Text;
                kunde.nachname = kundeNachname.Text;
                kunde.firma = kundeFirma.Text;
                kunde.strasse = kundeStraße.Text;
                kunde.postleitzahl = kundePostleitzahl.Text;
                kunde.ort = kundeOrt.Text;
                kunde.land = kundeLand.Text;
                kunde.telefon = kundeTelefon.Text;
                kunde.telefax = kundeTelefax.Text;
                kunde.mobiltelefon = kundeMobiltelefon.Text;
                kunde.email = kundeEmail.Text;
                kunde.webseite = kundeWebseite.Text;
                kunde.notizen = kundeNotizen.Text;

                if (kundeAnrede.SelectedItem == null)
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Anrede muss ausgefüllt sein!");
                    return;
                }
                if (kundeNachname.Text == "" && kundeFirma.Text == "")
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Nachname oder Firma muss ausgefüllt sein!");
                    return;
                }

                _entities.kunden.Attach(kunde);
                _entities.Entry(kunde).State = EntityState.Modified;
                _entities.SaveChanges();
                Refresh();

                groupBoxKunde.IsEnabled = false;
                groupBoxKunden.IsEnabled = true;

                kundeKundennummer.Content = null;
                kundeAnrede.Text = null;
                kundeVorname.Text = null;
                kundeNachname.Text = null;
                kundeFirma.Text = null;
                kundeStraße.Text = null;
                kundePostleitzahl.Text = null;
                kundeOrt.Text = null;
                kundeLand.Text = null;
                kundeTelefon.Text = null;
                kundeTelefax.Text = null;
                kundeMobiltelefon.Text = null;
                kundeEmail.Text = null;
                kundeWebseite.Text = null;
                kundeNotizen.Text = null;

                kundenListe.SelectedItem = null;
                kundenlisteBearbeiten.IsEnabled = false;
                kundenlisteLoeschen.IsEnabled = false;
                neuerKundeWirdAngelegt = false;
            }
        }

        private void KundeAbbrechen(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Sind sie sicher, dass sie den Bearbeitungsvorgang abbrechen wollen?\nAlle Änderungen gehen verloren.", "Abbrechen Bestätigung", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                groupBoxKunde.IsEnabled = false;
                groupBoxKunden.IsEnabled = true;

                kundeKundennummer.Content = null;
                kundeAnrede.Text = null;
                kundeVorname.Text = null;
                kundeNachname.Text = null;
                kundeFirma.Text = null;
                kundeStraße.Text = null;
                kundePostleitzahl.Text = null;
                kundeOrt.Text = null;
                kundeLand.Text = null;
                kundeTelefon.Text = null;
                kundeTelefax.Text = null;
                kundeMobiltelefon.Text = null;
                kundeEmail.Text = null;
                kundeWebseite.Text = null;
                kundeNotizen.Text = null;

                kundenListe.SelectedItem = null;
                kundenlisteBearbeiten.IsEnabled = false;
                kundenlisteLoeschen.IsEnabled = false;
                neuerKundeWirdAngelegt = false;
            }
        }

        private void KundenlisteAnlegen(object sender, RoutedEventArgs e)
        {
            groupBoxKunde.IsEnabled = true;
            groupBoxKunden.IsEnabled = false;

            kundeKundennummer.Content = null;
            kundeAnrede.Text = null;
            kundeVorname.Text = null;
            kundeNachname.Text = null;
            kundeFirma.Text = null;
            kundeStraße.Text = null;
            kundePostleitzahl.Text = null;
            kundeOrt.Text = null;
            kundeLand.Text = null;
            kundeTelefon.Text = null;
            kundeTelefax.Text = null;
            kundeMobiltelefon.Text = null;
            kundeEmail.Text = null;
            kundeWebseite.Text = null;
            kundeNotizen.Text = null;

            long kundennummer;

            if (_entities.kunden.Any() == false)
            {
                kundennummer = 1;
            }else
            {
                long hoechsteKundennummer = _entities.kunden.Max(k => k.kundennummer);
                kundennummer = ++hoechsteKundennummer;
            }

            kundeKundennummer.Content = kundennummer;

            kundenListe.SelectedItem = null;
            kundenlisteBearbeiten.IsEnabled = false;
            kundenlisteLoeschen.IsEnabled = false;
            neuerKundeWirdAngelegt = true;

        }

        private void NeueZeile(object sender, RoutedEventArgs e)
        {
            /*
            rechnungPositionenListe.Add(new RechnungPosition());
            rechnungPositionen.ItemsSource = null;
            rechnungPositionen.ItemsSource = rechnungPositionenListe;
            */
        }

        private void RechnungAbbrechen(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Sind sie sicher, dass sie den Bearbeitungsvorgang abbrechen wollen?\nAlle Änderungen gehen verloren.", "Abbrechen Bestätigung", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                /*
                //TODO: Rechnung deaktivieren
                rechnungPositionenListe.Clear();
                rechnungPositionen.ItemsSource = null;
                rechnungPositionen.ItemsSource = rechnungPositionenListe;
                */
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource kundenViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("kundenViewSource")));
            // Laden Sie Daten durch Festlegen der CollectionViewSource.Source-Eigenschaft:
            // kundenViewSource.Source = [generische Datenquelle]
        }

        private void KundeAusgewaehlt(object sender, SelectedCellsChangedEventArgs e)
        {
            if (kundenListe.SelectedCells != null)
            {
                try { 
                kundenlisteBearbeiten.IsEnabled = true;
                kundenlisteLoeschen.IsEnabled = true;

                kunden kunde = (kunden)kundenListe.SelectedItem;

                kundeKundennummer.Content = kunde.kundennummer;
                kundeAnrede.Text = kunde.anrede;
                kundeVorname.Text = kunde.vorname;
                kundeNachname.Text = kunde.nachname;
                kundeFirma.Text = kunde.firma;
                kundeStraße.Text = kunde.strasse;
                kundePostleitzahl.Text = kunde.postleitzahl;
                kundeOrt.Text = kunde.ort;
                kundeLand.Text = kunde.land;
                kundeTelefon.Text = kunde.telefon;
                kundeTelefax.Text = kunde.telefax;
                kundeMobiltelefon.Text = kunde.mobiltelefon;
                kundeEmail.Text = kunde.email;
                kundeWebseite.Text = kunde.webseite;
                kundeNotizen.Text = kunde.notizen;
                }
                catch (Exception){}
            }
            else
            {
                kundenlisteBearbeiten.IsEnabled = false;
                kundenlisteLoeschen.IsEnabled = false;
            }
        }
    }
}