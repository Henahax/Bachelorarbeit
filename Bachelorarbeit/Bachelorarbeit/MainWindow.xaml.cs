using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Data.Entity;
using System.Data;
using System.Linq;

using System.Collections.Generic;
using System.Globalization;
using System.Printing;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;
using System.IO.Packaging;
using System.Drawing.Printing;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Xps.Packaging;
using System.Windows.Markup;
using System.Windows.Xps;

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
        String rechnungenFilter = "";
        bool neuerKundeWirdAngelegt = false;
        bool neueRechnungWirdAngelegt = false;
        List<rechnung_positionen> rechnungPositionenListe = new List<rechnung_positionen>();

        public MainWindow()
        {
            //TODO: Datenbank relativer Pfad
            //TODO: Suche Vorschauanzeige

            //TODO: evtl pdfsharp 1.31 dlls einfügen https://nathanpjones.com/2013/03/output-to-pdf-in-wpf-for-free/
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

        private void KundeAuswaehlen(kunden kunde)
        {
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

        private void KundeAbwaehlen()
        {
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

        private void RechnungAbwaehlen()
        {
            rechnungRechnungsnummer.Content = null;
            rechnungDatum.SelectedDate = null;

            rechnungKundennummer.Content = null;
            rechnungKundenname.Content = null;
            rechnungKundenstraße.Content = null;
            rechnungKundenort.Content = null;
            rechnungKundenland.Content = null;

            //TODO:
            rechnungPositionenListe.Clear();
            rechnungPositionen.ItemsSource = null;

            rechnungZahlbarTage.Text = null;
            rechnungZahlbarDatum.SelectedDate = null;
            rechnungSkontoProzent.Text = null;
            rechnungSkontoBetrag.Content = null;
            rechnungSkontoTage.Text = null;
            rechnungSkontoDatum.SelectedDate = null;

            rechnungBemerkung.Text = null;
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

        private void rechnungenSucheEingeben(object sender, TextChangedEventArgs e)
        {
            rechnungenFilter = rechnungenSuche.Text;
            Refresh();
            rechnungenListe.SelectedItem = null;
        }


        private void KundenlisteBearbeiten(object sender, RoutedEventArgs e)
        {
            kunden kunde = (kunden)kundenListe.SelectedItem;

            groupBoxKunde.IsEnabled = true;
            groupBoxKunden.IsEnabled = false;

            tabRechnungen.IsEnabled = false;
            tabAngebote.IsEnabled = false;

            KundeAuswaehlen(kunde);
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

                KundeAbwaehlen();
            }
        }

        private void KundeSpeichern(object sender, RoutedEventArgs e)
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

            if (neuerKundeWirdAngelegt == true)
            {
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
            }
            else
            {
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
            }
            _entities.SaveChanges();
            Refresh();

            groupBoxKunde.IsEnabled = false;
            groupBoxKunden.IsEnabled = true;

            tabRechnungen.IsEnabled = true;
            tabAngebote.IsEnabled = true;

            KundeAbwaehlen();

            neuerKundeWirdAngelegt = false;
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

                KundeAbwaehlen();

                neuerKundeWirdAngelegt = false;
            }
        }

        private void KundenlisteAnlegen(object sender, RoutedEventArgs e)
        {
            groupBoxKunde.IsEnabled = true;
            groupBoxKunden.IsEnabled = false;

            tabRechnungen.IsEnabled = false;
            tabAngebote.IsEnabled = false;

            KundeAbwaehlen();

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

            neuerKundeWirdAngelegt = true;
        }

        private void RechnungZeileHinzufuegen(object sender, RoutedEventArgs e)
        {
            rechnungPositionenListe.Add(new rechnung_positionen());
            rechnungPositionen.ItemsSource = null;
            rechnungPositionen.ItemsSource = rechnungPositionenListe;
        }

        private void RechnungZeileLoeschen(object sender, RoutedEventArgs e)
        {
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

                    KundeAuswaehlen(kunde);
                }
                catch (Exception)
                {
                    //Passiert bei leeren Datensätzen
                }
            }
            else
            {
                kundenlisteBearbeiten.IsEnabled = false;
                kundenlisteLoeschen.IsEnabled = false;
            }
        }

        private void RechnungAusgewaehlt(object sender, SelectedCellsChangedEventArgs e)
        {
            //TODO: Problem: Erster Eintrag in Liste funktioniert nicht
            if (rechnungenListe.SelectedCells != null)
            {
                try
                {
                    rechnungslisteBearbeiten.IsEnabled = true;
                    rechnungslisteLoeschen.IsEnabled = true;

                    rechnungen rechnung = (rechnungen)rechnungenListe.SelectedItem;

                    RechnungAbwaehlen();

                    //TODO: Exception besser behandeln

                    rechnungRechnungsnummer.Content = rechnung.rechnungsnummer.ToString();

                    rechnungDatum.SelectedDate = null;
                    String stringDatum = rechnung.datum;
                    DateTime datum = DateTime.Parse(stringDatum);

                    rechnungDatum.SelectedDate = DateTime.Parse(stringDatum);

                    rechnungKundennummer.Content = rechnung.kunden.kundennummer;

                    if (rechnung.kunden.anrede == "Firma")
                    {
                        rechnungKundenname.Content = rechnung.kunden.firma;
                    }
                    else if(rechnung.kunden.anrede == "Herr")
                    {
                        rechnungKundenname.Content = rechnung.kunden.anrede + "n " + rechnung.kunden.vorname + " " + rechnung.kunden.nachname;
                    }
                    else if(rechnung.kunden.anrede == "Frau")
                    {
                        rechnungKundenname.Content = rechnung.kunden.anrede + " " + rechnung.kunden.vorname + " " + rechnung.kunden.nachname;
                    }

                    rechnungKundenstraße.Content = rechnung.kunden.strasse;

                    rechnungKundenort.Content = rechnung.kunden.postleitzahl + " " + rechnung.kunden.ort;

                    rechnungKundenland.Content = rechnung.kunden.land;

                    //TODO

                    rechnungPositionenListe.Clear();
                    foreach (rechnung_positionen element in rechnung.rechnung_positionen)
                    {
                        rechnungPositionenListe.Add(element);
                    }
                    rechnungPositionen.ItemsSource = null;
                    rechnungPositionen.ItemsSource = rechnungPositionenListe;

                    rechnungZahlbarTage.Text = rechnung.zahlbartage.ToString();

                    //TODO
                }
                catch (Exception) { }
            }
            else
            {
                rechnungslisteBearbeiten.IsEnabled = false;
                rechnungslisteLoeschen.IsEnabled = false;
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

        private void RechnungenFilter(object sender, FilterEventArgs e)
        {
            //TODO: Testen (Geht noch nicht)
            rechnungen rechnung = e.Item as rechnungen;
            if (rechnung.rechnung_positionen != null)
            {
                foreach (rechnung_positionen p in rechnung.rechnung_positionen)
                {
                    if (p.beschreibung != null || p.name != null)
                    {
                        if (p.beschreibung.ToLower().Contains(rechnungenFilter))
                        {
                            e.Accepted = true;
                        }
                        if (p.name.ToLower().Contains(rechnungenFilter))
                        {
                            e.Accepted = true;
                        }
                    } 
                }
            }
            else if (rechnung.kunden.titel.ToLower().Contains(rechnungenFilter.ToLower()) || rechnung.kunden.vorname.ToLower().Contains(rechnungenFilter.ToLower()) || rechnung.kunden.nachname.ToLower().Contains(rechnungenFilter.ToLower()) || rechnung.kunden.firma.ToLower().Contains(rechnungenFilter.ToLower()) || rechnung.kunden.strasse.ToLower().Contains(rechnungenFilter.ToLower()) || rechnung.kunden.postleitzahl.ToLower().Contains(rechnungenFilter.ToLower()) || rechnung.kunden.ort.ToLower().Contains(rechnungenFilter.ToLower()) || rechnung.kunden.land.ToLower().Contains(rechnungenFilter.ToLower()) || rechnung.kunden.telefon.ToLower().Contains(rechnungenFilter.ToLower()) || rechnung.kunden.telefax.ToLower().Contains(rechnungenFilter.ToLower()) || rechnung.kunden.mobiltelefon.ToLower().Contains(rechnungenFilter.ToLower()) || rechnung.kunden.email.ToLower().Contains(rechnungenFilter.ToLower()) || rechnung.kunden.webseite.ToLower().Contains(rechnungenFilter.ToLower()) || rechnung.kunden.notizen.ToLower().Contains(rechnungenFilter.ToLower()))
            {
                e.Accepted = true;
            }

            else
            {
                e.Accepted = false;
            }
        }

        private void RechnungslisteAnlegen(object sender, RoutedEventArgs e)
        {
            KundenAuswahl kundenAuswahl = new KundenAuswahl();
            kundenAuswahl.ShowDialog();
            kunden kunde = kundenAuswahl.kunde;
            if(kunde == null)
            {
                return;
            }

            rechnungenListe.SelectedItem = null;
            rechnungslisteBearbeiten.IsEnabled = false;
            rechnungslisteLoeschen.IsEnabled = false; 

            groupBoxRechnung.IsEnabled = true;
            groupBoxRechnungen.IsEnabled = false;

            tabKunden.IsEnabled = false;
            tabAngebote.IsEnabled = false;

            groupBoxRechnungKunde.IsEnabled = true;

            RechnungAbwaehlen();

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

            String stringDatum = System.DateTime.Now.ToShortDateString();
            DateTime datum = DateTime.Parse(stringDatum);
            rechnungDatum.SelectedDate = datum;

            rechnungKundennummer.Content = kunde.kundennummer;

            if (kunde.anrede == "Firma")
            {
                rechnungKundenname.Content = kunde.firma;
            }
            else if (kunde.anrede == "Herr")
            {
                rechnungKundenname.Content = kunde.anrede + "n " + kunde.vorname + " " + kunde.nachname;
            }
            else if (kunde.anrede == "Frau")
            {
                rechnungKundenname.Content = kunde.anrede + " " + kunde.vorname + " " + kunde.nachname;
            }

            rechnungKundenstraße.Content = kunde.strasse;

            rechnungKundenort.Content = kunde.postleitzahl + " " + kunde.ort;

            rechnungKundenland.Content = kunde.land;

            rechnungPositionen.ItemsSource = rechnungPositionenListe;

            neueRechnungWirdAngelegt = true;
        }

        private void RechnungslisteBearbeiten(object sender, RoutedEventArgs e)
        {
            rechnungen rechnung = (rechnungen)rechnungenListe.SelectedItem;

            groupBoxRechnung.IsEnabled = true;
            groupBoxRechnungen.IsEnabled = false;

            tabKunden.IsEnabled = false;
            tabAngebote.IsEnabled = false;

            groupBoxRechnungKunde.IsEnabled = false;

            rechnungRechnungsnummer.Content = rechnung.rechnungsnummer;

            rechnungPositionenListe.Clear();
            foreach (rechnung_positionen element in rechnung.rechnung_positionen)
            {
                rechnungPositionenListe.Add(element);
            }
            rechnungPositionen.ItemsSource = null;
            rechnungPositionen.ItemsSource = rechnungPositionenListe;
            
            //TODO: Ändern und testen
            /*
            String stringDatum = rechnung.datum;
            DateTime datum = DateTime.Parse(stringDatum);
            rechnungDatum.SelectedDate = datum;
            */

            //TODO
        }

        private void RechnungslisteLoeschen(object sender, RoutedEventArgs e)
        {
            rechnungen rechnung = (rechnungen)rechnungenListe.SelectedItem;

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Sind sie sicher, dass sie diese Rechnung löschen wollen?", "Rechnung Löschen Bestätigung", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                _entities.rechnungen.Remove(rechnung);
                _entities.SaveChanges();
                Refresh();

                RechnungAbwaehlen();

                rechnungenListe.SelectedItem = null;
                rechnungslisteBearbeiten.IsEnabled = false;
                rechnungslisteLoeschen.IsEnabled = false;
            }
        }

        private void RechnungSpeichern(object sender, RoutedEventArgs e)
        {
            //TODO: Pflichfelder prüfen
            //Zahlbardatum vor Rechnungsdatum

            //Zahlbartage speichern geht noch nicht

            long kundennummer;
            Int64.TryParse(rechnungKundennummer.Content.ToString(), out kundennummer);


            //TODO: evtl unsauber (Evtl Objektauswahl)
            var query = from kunden in _entities.kunden where kunden.kundennummer == kundennummer select kunden;
            long kundenId = 0;
            foreach (var result in query)
            {
                kundenId = result.id;
            }

            if (neueRechnungWirdAngelegt == true)
            {
                long rechnungsnummer;
                Int64.TryParse(rechnungRechnungsnummer.Content.ToString(), out rechnungsnummer);

                rechnungen rechnung = new rechnungen();

                rechnung.rechnungsnummer = rechnungsnummer;

                DateTime datum = (DateTime)rechnungDatum.SelectedDate;
                string stringDatum = datum.ToString("yyyy-MM-dd");
                rechnung.datum = stringDatum;

                rechnung.kunde_id = kundenId;
                foreach (rechnung_positionen element in rechnungPositionenListe)
                {
                    rechnung.rechnung_positionen.Add(element);
                }

                long zahlbartage;
                Int64.TryParse(rechnungZahlbarTage.Text, out zahlbartage);

                rechnung.zahlbartage = zahlbartage;

                //TODO: Skonto

                rechnung.bemerkung = rechnungBemerkung.Text;

                _entities.rechnungen.Add(rechnung);
            }
            else
            {
                rechnungen rechnung = (rechnungen)rechnungenListe.SelectedItem;

                rechnung.kunde_id = kundenId;

                rechnung.rechnung_positionen.Clear();
                foreach (rechnung_positionen element in rechnungPositionenListe)
                {
                    rechnung.rechnung_positionen.Add(element);
                }

                long zahlbartage;
                Int64.TryParse(rechnungZahlbarTage.Text, out zahlbartage);
                rechnung.zahlbartage = zahlbartage;

                //TODO: Skonto

                rechnung.bemerkung = rechnungBemerkung.Text;

                _entities.rechnungen.Attach(rechnung);
                _entities.Entry(rechnung).State = EntityState.Modified;
            }
            _entities.SaveChanges();
            Refresh();

            RechnungAbwaehlen();

            groupBoxRechnung.IsEnabled = false;
            groupBoxRechnungen.IsEnabled = true;

            tabKunden.IsEnabled = true;
            tabAngebote.IsEnabled = true;

            rechnungenListe.SelectedItem = null;
            rechnungslisteBearbeiten.IsEnabled = false;
            rechnungslisteLoeschen.IsEnabled = false;
            neueRechnungWirdAngelegt = false;
        }

        private void RechnungSpeichernPDF(object sender, RoutedEventArgs e)
        {
            //TODO: Erst speichern
            rechnungen rechnung = (rechnungen)rechnungenListe.SelectedItem;
            
            MemoryStream lMemoryStream = new MemoryStream();
            Package package = Package.Open(lMemoryStream, FileMode.Create);
            XpsDocument doc = new XpsDocument(package);
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
            writer.Write(new Rechnung(rechnung));
            doc.Close();
            package.Close();

            var pdfXpsDoc = PdfSharp.Xps.XpsModel.XpsDocument.Open(lMemoryStream);

            string file;
            if (rechnung.kunden.anrede == "Firma")
            {
                file = _entities.einstellungen.First().speicherortrechnungen + "\\" + rechnung.rechnungsnummer + " " + rechnung.kunden.firma + ".pdf";
            }
            else
            {
                file = _entities.einstellungen.First().speicherortrechnungen + "\\" + rechnung.rechnungsnummer + " " + rechnung.kunden.vorname + " " + rechnung.kunden.nachname + ".pdf";
            }
            PdfSharp.Xps.XpsConverter.Convert(pdfXpsDoc, file, 0);

            //TODO: Abschneiden verhindern!
        }

        private void RechnungDrucken(object sender, RoutedEventArgs e)
        {
            //TODO: Erst speichern + PDF
            rechnungen rechnung = (rechnungen)rechnungenListe.SelectedItem;

            PrintDialog druckdialog = new PrintDialog();
            if (druckdialog.ShowDialog() == true)
            {
                druckdialog.PrintVisual(new Rechnung(rechnung), "Rechnung");
            }
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

                RechnungAbwaehlen();

                rechnungenListe.SelectedItem = null;
                rechnungslisteBearbeiten.IsEnabled = false;
                rechnungslisteLoeschen.IsEnabled = false;
                neueRechnungWirdAngelegt = false;
            }
        }

        private void RechnungKundenAuswaehlen(object sender, RoutedEventArgs e)
        {
            KundenAuswahl kundenAuswahl = new KundenAuswahl();
            kundenAuswahl.ShowDialog();
        }

        private void RechnungPositionenUpdate(object sender, DataGridCellEditEndingEventArgs e)
        {
            var zelle = rechnungPositionen.CurrentItem;
            rechnungPositionen.CurrentItem = null;
            rechnungPositionen.CurrentItem = zelle;

            decimal? summe = 0;
            foreach (rechnung_positionen position in rechnungPositionenListe)
            {
                if (position.menge != null && position.einzelpreis != null)
                {
                    position.gesamtpreis = position.menge * position.einzelpreis;

                    summe = position.gesamtpreis + summe;
                }
                //TODO: Überschriften Menge und Betrag entfernen
                if (position.ueberschrift == 1)
                {
                    position.menge = null;
                    position.einzelpreis = null;
                }
            }
            rechnungNettosumme.Content = string.Format("{0:C}", summe * ((decimal)81) / 100);
            rechnungMehrwertsteuer.Content = string.Format("{0:C}", summe * ((decimal)19) / 100);
            rechnungGesamtsumme.Content = string.Format("{0:C}", summe);

            if(rechnungSkontoProzent.Text != null)
            {
                decimal temp;
                decimal? skonto = decimal.TryParse(rechnungSkontoProzent.Text, out temp) ? temp : default(decimal?);

                rechnungSkontoBetrag.Content = string.Format("{0:C}", summe * skonto / 100);
            }

            //TODO: Item Source Refresh ohne Tabelle zu leeren (Nach eingabetaste geht refresh mit neuer Zeile hinzufügen)
        }

        private void RechnungZahlbarTageGeaendert(object sender, TextChangedEventArgs e)
        {
            if (rechnungDatum.SelectedDate == null || rechnungZahlbarTage.Text == null || rechnungZahlbarTage.Text == "")
            {
                return;
            }
            DateTime? date = rechnungDatum.SelectedDate;
            //TODO: Buchstaben in Tage Feld
            double tage = double.Parse(rechnungZahlbarTage.Text, System.Globalization.CultureInfo.InvariantCulture);
            date = date.Value.AddDays(tage);
            rechnungZahlbarDatum.SelectedDate = date;
        }

        private void RechnungZahlbarDatumGeaendert(object sender, SelectionChangedEventArgs e)
        {
            if (rechnungDatum.SelectedDate == null || rechnungZahlbarDatum.SelectedDate == null)
            {
                return;
            }

            DateTime? date = rechnungDatum.SelectedDate;
            DateTime? date2 = rechnungZahlbarDatum.SelectedDate;
            rechnungZahlbarTage.Text = (date2.Value - date.Value).Days.ToString();
        }

        private void RechnungDatumGeaendert(object sender, SelectionChangedEventArgs e)
        {
            DateTime? date = rechnungDatum.SelectedDate;

            if (date == null)
            {
                rechnungZahlbarTage.Text = null;
                rechnungZahlbarDatum.SelectedDate = null;
                return;
            }

            if (rechnungZahlbarTage.Text != null || rechnungZahlbarTage.Text != "")
            {
                double tage = 0;
                Double.TryParse(rechnungZahlbarTage.Text, out tage);
                date = date.Value.AddDays(tage);
                rechnungZahlbarDatum.SelectedDate = date;
                return;
            }

            if (rechnungZahlbarDatum != null)
            {
                DateTime? date2 = rechnungZahlbarDatum.SelectedDate;
                rechnungZahlbarTage.Text = (date2.Value - date.Value).Days.ToString();
            }
        }

        private void RechnungSkontoTageGeaendert(object sender, TextChangedEventArgs e)
        {
            DateTime? date = rechnungDatum.SelectedDate;

            if (date == null)
            {
                return;
            }

            if (rechnungSkontoTage.Text == null)
            {
                return;
            }
            //TODO: Testen: Spring bei auswahl von Rechnungen an (z.B. über Filter)
            double tage = double.Parse(rechnungSkontoTage.Text, System.Globalization.CultureInfo.InvariantCulture);
            date = date.Value.AddDays(tage);
            rechnungSkontoDatum.SelectedDate = date;
        }

        private void RechnungSkontoDatumGeaendert(object sender, SelectionChangedEventArgs e)
        {
            DateTime? date = rechnungDatum.SelectedDate;

            if (date == null)
            {
                rechnungSkontoTage.Text = null;
                rechnungSkontoDatum.SelectedDate = null;
                return;
            }

            if (rechnungSkontoTage != null)
            {
                double tage = double.Parse(rechnungSkontoTage.Text, System.Globalization.CultureInfo.InvariantCulture);
                date = date.Value.AddDays(tage);
                rechnungSkontoDatum.SelectedDate = date;
                return;
            }

            if (rechnungSkontoDatum != null)
            {
                DateTime? date2 = rechnungSkontoDatum.SelectedDate;
                rechnungSkontoTage.Text = (date2.Value - date.Value).Days.ToString();
            }
        }

        private void RechnungSkontoProzentGeaendert(object sender, TextChangedEventArgs e)
        {
            //TODO: Noch nicht zuverlässig (wahrscheinlich abhängig vom Datagrid)
            if (rechnungSkontoProzent.Text != null)
            {
                if (rechnungGesamtsumme.Content != null)
                {
                    decimal temp;
                    decimal? summe = decimal.TryParse(rechnungGesamtsumme.Content.ToString(), out temp) ? temp : default(decimal?);

                    decimal temp2;
                    decimal? skonto = decimal.TryParse(rechnungSkontoProzent.Text, out temp2) ? temp2 : default(decimal?);
                    rechnungSkontoBetrag.Content = string.Format("{0:C}", summe * skonto / 100);
                }
            }
        }
    }
}