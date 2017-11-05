using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Data.Entity;
using System.Data;
using System.Linq;

using System.Collections.Generic;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Diagnostics;

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
        List<rechnung_positionen> rechnungPositionenListe = new List<rechnung_positionen>();

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

            rechnungKundenname.Content = null;
            rechnungKundenstraße.Content = null;
            rechnungKundenort.Content = null;
            rechnungKundenland.Content = null;

            //TODO:
            rechnungPositionenListe.Clear();
            rechnungPositionen.ItemsSource = null;
            //TODO:
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
            if (rechnungsListe.SelectedCells != null)
            {
                try
                {
                    rechnungslisteBearbeiten.IsEnabled = true;
                    rechnungslisteLoeschen.IsEnabled = true;

                    rechnungen rechnung = (rechnungen)rechnungsListe.SelectedItem;

                    RechnungAbwaehlen();

                    rechnungRechnungsnummer.Content = rechnung.rechnungsnummer;

                    rechnungDatum.SelectedDate = null;
                    String stringDatum = rechnung.datum;
                    DateTime datum = DateTime.Parse(stringDatum);
                    rechnungDatum.SelectedDate = datum;

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

        private void RechnungslisteAnlegen(object sender, RoutedEventArgs e)
        {
            //TODO: Direkt Kundenauswahl Popup

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

            rechnungPositionen.ItemsSource = rechnungPositionenListe;

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
            rechnungen rechnung = (rechnungen)rechnungsListe.SelectedItem;

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Sind sie sicher, dass sie diesen Kunden löschen wollen?", "Kunden Löschen Bestätigung", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {

                _entities.rechnungen.Remove(rechnung);
                _entities.SaveChanges();
                Refresh();

                RechnungAbwaehlen();

                rechnungsListe.SelectedItem = null;
                rechnungslisteBearbeiten.IsEnabled = false;
                rechnungslisteLoeschen.IsEnabled = false;
            }
        }

        private void RechnungSpeichern(object sender, RoutedEventArgs e)
        {
            //TODO: Pflichfelder prüfen

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
                Int64.TryParse(kundeKundennummer.Content.ToString(), out rechnungsnummer);

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
                //TODO:

                _entities.rechnungen.Add(rechnung);
            }
            else
            {
                rechnungen rechnung = (rechnungen)rechnungsListe.SelectedItem;

                rechnung.kunde_id = kundenId;

                rechnung.rechnung_positionen.Clear();
                foreach (rechnung_positionen element in rechnungPositionenListe)
                {
                    rechnung.rechnung_positionen.Add(element);
                }
                //TODO


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

            rechnungsListe.SelectedItem = null;
            rechnungslisteBearbeiten.IsEnabled = false;
            rechnungslisteLoeschen.IsEnabled = false;
            neueRechnungWirdAngelegt = false;
        }

        private void RechnungSpeichernPDF(object sender, RoutedEventArgs e)
        {
            //TODO

            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont font = new XFont("Verdana", 10, XFontStyle.Bold);

            // Draw the text
            gfx.DrawString(rechnungKundenname.Content.ToString(), font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormat.TopLeft);
            gfx.DrawString(rechnungKundenstraße.Content.ToString(), font, XBrushes.Black, new XRect(0, 10, page.Width, page.Height), XStringFormat.TopLeft);
            gfx.DrawString(rechnungKundenort.Content.ToString(), font, XBrushes.Black, new XRect(0, 20, page.Width, page.Height), XStringFormat.TopLeft);
            gfx.DrawString(rechnungKundenland.Content.ToString(), font, XBrushes.Black, new XRect(0, 30, page.Width, page.Height), XStringFormat.TopLeft);

            // Save the document...
            string filename = "HelloWorld.pdf";
            document.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);
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

                RechnungAbwaehlen();

                rechnungsListe.SelectedItem = null;
                rechnungslisteBearbeiten.IsEnabled = false;
                rechnungslisteLoeschen.IsEnabled = false;
                neueRechnungWirdAngelegt = false;
            }
        }

        private void RechnungKundenDurchsuchen(object sender, RoutedEventArgs e)
        {
            KundenAuswahl kundenAuswahl = new KundenAuswahl();
            kundenAuswahl.ShowDialog();
        }
    }
}