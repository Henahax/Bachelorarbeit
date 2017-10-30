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
        CollectionViewSource _rechnungenViewSource;
        String kundenFilter = "";
        bool neuerKundeWirdAngelegt = false;
        bool neueRechnungWirdAngelegt = false;

        public MainWindow()
        {
            //TODO: Datenbank relativer Pfad
            //TODO: Suche Vorschauanzeige

            //TODO: Bei Datenbankänderungen, Identify Spalte neu konfigurieren im Model
            InitializeComponent();
            _kundenViewSource = ((CollectionViewSource)(this.FindResource("kundenViewSource")));
            _rechnungenViewSource = ((CollectionViewSource)(this.FindResource("rechnungenViewSource")));
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
            _entities.rechnungen.Load();
            _rechnungenViewSource.Source = _entities.rechnungen.Local;
        }
        

        private void EinstellungenOeffnen(object sender, RoutedEventArgs e)
        {
            Einstellungen einstellungen = new Einstellungen();
            einstellungen.ShowDialog();
        }

        private void KundenSucheEingeben(object sender, TextChangedEventArgs e)
        {
            kundenFilter = kundenSuche.Text;
            Refresh();
            kundenListe.SelectedItem = null;
        }


        private void KundenlisteBearbeiten(object sender, RoutedEventArgs e)
        {
            kunden kunde = (kunden)kundenListe.SelectedItem;

            groupBoxKunde.IsEnabled = true;
            groupBoxKunden.IsEnabled = false;

            tabRechnungen.IsEnabled = false;
            tabAngebote.IsEnabled = false;

            kundeKundennummer.Content = kunde.kundennummer;
            kundeAnrede.Text = kunde.anrede;
            kundeTitel.Text = kunde.titel;
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

                kundeKundennummer.Content = null;
                kundeAnrede.Text = null;
                kundeTitel.Text = null;
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
            }
        }

        private void KundeSpeichern(object sender, RoutedEventArgs e)
        {
            if (neuerKundeWirdAngelegt == true)
            {
                if (kundeAnrede.SelectedItem == null)
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Anrede muss ausgefüllt sein!");
                    return;
                }
                if (kundeAnrede.Text == "Herr" || kundeAnrede.Text == "Frau")
                {
                    if (kundeNachname.Text == "")
                    {
                        MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Nachname ausgefüllt sein!");
                        return;
                    }
                }
                if (kundeAnrede.Text == "Firma" && kundeFirma.Text == "")
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Firma muss ausgefüllt sein!");
                    return;
                }

                kunden kunde = new kunden();

                long kundennummer;

                Int64.TryParse(kundeKundennummer.Content.ToString(), out kundennummer);
                kunde.kundennummer = kundennummer;
                kunde.anrede = kundeAnrede.Text;
                kunde.titel = kundeTitel.Text;
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

                _entities.kunden.Add(kunde);
                _entities.SaveChanges();
                Refresh();

                groupBoxKunde.IsEnabled = false;
                groupBoxKunden.IsEnabled = true;

                tabRechnungen.IsEnabled = true;
                tabAngebote.IsEnabled = true;

                kundeKundennummer.Content = null;
                kundeAnrede.Text = null;
                kundeTitel.Text = null;
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

                kunden kunde = (kunden)kundenListe.SelectedItem;

                kunde.anrede = kundeAnrede.Text;
                kunde.titel = kundeTitel.Text;
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

                _entities.kunden.Attach(kunde);
                _entities.Entry(kunde).State = EntityState.Modified;
                _entities.SaveChanges();
                Refresh();

                groupBoxKunde.IsEnabled = false;
                groupBoxKunden.IsEnabled = true;

                tabRechnungen.IsEnabled = true;
                tabAngebote.IsEnabled = true;

                kundeKundennummer.Content = null;
                kundeAnrede.Text = null;
                kundeTitel.Text = null;
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

                tabRechnungen.IsEnabled = true;
                tabAngebote.IsEnabled = true;

                kundeKundennummer.Content = null;
                kundeAnrede.Text = null;
                kundeTitel.Text = null;
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

            tabRechnungen.IsEnabled = false;
            tabAngebote.IsEnabled = false;

            kundeKundennummer.Content = null;
            kundeAnrede.Text = null;
            kundeTitel.Text = null;
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
            }
            else
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource kundenViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("kundenViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // kundenViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource rechnungenViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("rechnungenViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // rechnungenViewSource.Source = [generic data source]
        }

        private void KundeAusgewaehlt(object sender, SelectedCellsChangedEventArgs e)
        {
            if (kundenListe.SelectedCells != null)
            {
                try
                {
                    kundenlisteBearbeiten.IsEnabled = true;
                    kundenlisteLoeschen.IsEnabled = true;

                    kunden kunde = (kunden)kundenListe.SelectedItem;

                    kundeKundennummer.Content = kunde.kundennummer;
                    kundeAnrede.Text = kunde.anrede;
                    kundeTitel.Text = kunde.titel;
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
                catch (Exception) { }
            }
            else
            {
                kundenlisteBearbeiten.IsEnabled = false;
                kundenlisteLoeschen.IsEnabled = false;
            }
        }

        private void KundenFilter(object sender, FilterEventArgs e)
        {
            kunden kunde = e.Item as kunden;
            if (kunde.titel.ToLower().Contains(kundenFilter.ToLower()) || kunde.vorname.ToLower().Contains(kundenFilter.ToLower()) || kunde.nachname.ToLower().Contains(kundenFilter.ToLower()) || kunde.firma.ToLower().Contains(kundenFilter.ToLower()) || kunde.strasse.ToLower().Contains(kundenFilter.ToLower()) || kunde.postleitzahl.ToLower().Contains(kundenFilter.ToLower()) || kunde.ort.ToLower().Contains(kundenFilter.ToLower()) || kunde.land.ToLower().Contains(kundenFilter.ToLower()) || kunde.telefon.ToLower().Contains(kundenFilter.ToLower()) || kunde.telefax.ToLower().Contains(kundenFilter.ToLower()) || kunde.mobiltelefon.ToLower().Contains(kundenFilter.ToLower()) || kunde.email.ToLower().Contains(kundenFilter.ToLower()) || kunde.webseite.ToLower().Contains(kundenFilter.ToLower()) || kunde.notizen.ToLower().Contains(kundenFilter.ToLower()))
            {
                e.Accepted = true;
            }else
            {
                e.Accepted = false;
            }
        }

        private void RechnungslisteAnlegen(object sender, RoutedEventArgs e)
        {
            groupBoxRechnung.IsEnabled = true;
            groupBoxRechnungen.IsEnabled = false;

            tabKunden.IsEnabled = false;
            tabAngebote.IsEnabled = false;

            rechnungRechnungsnummer.Content = null;
            //TODO:

            long rechnungsnummer;

            if (_entities.rechnungen.Any() == false)
            {
                rechnungsnummer = 1;
            }
            else
            {
                long hoechsteRechnungsnummer = _entities.rechnungen.Max(r => r.rechnungsnummer);
                rechnungsnummer = ++hoechsteRechnungsnummer;
            }

            rechnungRechnungsnummer.Content = rechnungsnummer;

            rechnungsListe.SelectedItem = null;
            rechnungslisteBearbeiten.IsEnabled = false;
            rechnungslisteLoeschen.IsEnabled = false;
            neueRechnungWirdAngelegt = true;
        }

        private void RechnungslisteBearbeiten(object sender, RoutedEventArgs e)
        {
            rechnungen rechnung = (rechnungen)rechnungsListe.SelectedItem;

            groupBoxRechnung.IsEnabled = true;
            groupBoxRechnungen.IsEnabled = false;

            tabKunden.IsEnabled = false;
            tabAngebote.IsEnabled = false;


            rechnungRechnungsnummer.Content = rechnung.rechnungsnummer;
            //TODO:
        }

        private void RechnungslisteLoeschen(object sender, RoutedEventArgs e)
        {
            rechnungen rechnung = (rechnungen)rechnungsListe.SelectedItem;

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Sind sie sicher, dass sie diesen Kunden löschen wollen?", "Kunden Löschen Bestätigung", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {

                _entities.rechnungen.Remove(rechnung);
                //TODO: Prüfen ob Positionen gelöscht werden
                _entities.SaveChanges();
                Refresh();

                rechnungRechnungsnummer.Content = null;
                //TODO:

                rechnungsListe.SelectedItem = null;
                rechnungslisteBearbeiten.IsEnabled = false;
                rechnungslisteLoeschen.IsEnabled = false;
            }
        }

        private void RechnungSpeichern(object sender, RoutedEventArgs e)
        {
            if (neueRechnungWirdAngelegt == true)
            {
                //TODO: Pflichfelder prüfen

                long rechnungsnummer;
                Int64.TryParse(kundeKundennummer.Content.ToString(), out rechnungsnummer);
                
                long kundennummer;
                Int64.TryParse(rechnungKundennummer.Content.ToString(), out kundennummer);

                var query = from kunden in _entities.kunden where kunden.kundennummer == kundennummer select kunden;
                
                //TODO: evtl unsauber
                long kundenId = 0;
                foreach (var result in query)
                {
                    kundenId = result.id;
                }

                rechnungen rechnung = new rechnungen();

                rechnung.rechnungsnummer = rechnungsnummer;
                rechnung.kunde_id = kundenId;
                //TODO

                _entities.rechnungen.Add(rechnung);
                _entities.SaveChanges();
                Refresh();

                groupBoxRechnung.IsEnabled = false;
                groupBoxRechnungen.IsEnabled = true;

                tabKunden.IsEnabled = true;
                tabAngebote.IsEnabled = true;

                rechnungRechnungsnummer.Content = null;
                rechnungKundennummer.Content = null;
                //TODO

                rechnungsListe.SelectedItem = null;
                rechnungslisteBearbeiten.IsEnabled = false;
                rechnungslisteLoeschen.IsEnabled = false;
                neueRechnungWirdAngelegt = false;
            }
            else
            {
                //TODO: Pflichfelder prüfen

                long kundennummer;
                Int64.TryParse(rechnungKundennummer.Content.ToString(), out kundennummer);

                var query = from kunden in _entities.kunden where kunden.kundennummer == kundennummer select kunden;

                //TODO: evtl unsauber
                long kundenId = 0;
                foreach (var result in query)
                {
                    kundenId = result.id;
                }

                rechnungen rechnung = (rechnungen)rechnungsListe.SelectedItem;

                rechnung.kunde_id = kundenId;
                //TODO

                _entities.rechnungen.Attach(rechnung);
                _entities.Entry(rechnung).State = EntityState.Modified;
                _entities.SaveChanges();
                Refresh();

                groupBoxRechnung.IsEnabled = false;
                groupBoxRechnungen.IsEnabled = true;

                tabKunden.IsEnabled = true;
                tabAngebote.IsEnabled = true;

                rechnungRechnungsnummer.Content = null;
                rechnungKundennummer.Content = null;
                //TODO

                rechnungsListe.SelectedItem = null;
                rechnungslisteBearbeiten.IsEnabled = false;
                rechnungslisteLoeschen.IsEnabled = false;
                neueRechnungWirdAngelegt = false;
            }
        }

        private void RechnungSpeichernPDF(object sender, RoutedEventArgs e)
        {
            //TODO:
        }

        private void RechnungDrucken(object sender, RoutedEventArgs e)
        {
            //TODO:
            PrintDialog druckdialog = new PrintDialog();
            druckdialog.ShowDialog();
        }

        private void RechnungAbbrechen(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Sind sie sicher, dass sie den Bearbeitungsvorgang abbrechen wollen?\nAlle Änderungen gehen verloren.", "Abbrechen Bestätigung", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                groupBoxRechnung.IsEnabled = false;
                groupBoxRechnungen.IsEnabled = true;

                tabKunden.IsEnabled = true;
                tabAngebote.IsEnabled = true;

                rechnungRechnungsnummer.Content = null;
                //TODO:

                rechnungsListe.SelectedItem = null;
                rechnungslisteBearbeiten.IsEnabled = false;
                rechnungslisteLoeschen.IsEnabled = false;
                neueRechnungWirdAngelegt = false;
            }
        }

        private void RechnungAusgewaehlt(object sender, SelectedCellsChangedEventArgs e)
        {

        }
    }
}