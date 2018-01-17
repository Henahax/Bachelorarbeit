using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bachelorarbeit
{
    /// <summary>
    /// Interaction logic for Dokument.xaml
    /// </summary>
    public partial class Rechnung : Page
    {
        mainEntities _entities;
        public Rechnung(rechnungen rechnung)
        {
            InitializeComponent();
            Refresh();
            FelderFuellen(rechnung);
        }
        private void Refresh()
        {
            if (_entities != null)
            {
                _entities.Dispose();
            }
            _entities = new mainEntities();
        }

        private void FelderFuellen(rechnungen rechnung)
        {
            briefkopfFirmenname.Content = _entities.einstellungen.First().firmenname;
            briefkopfAnschrift.Content = _entities.einstellungen.First().strasse + " - " + _entities.einstellungen.First().postleitzahl + " " + _entities.einstellungen.First().ort + " - " + _entities.einstellungen.First().land;

            rechnungsdatum.Content = DateTime.Parse(rechnung.datum).ToString("dd.MM.yyy");
            rechnungsnummer.Content = rechnung.rechnungsnummer;
            kundennummer.Content = rechnung.kunden.kundennummer;

            if (rechnung.kunden.anrede == "Firma")
            {
                briefkopfAnrede.Content = "";
                anrede.Content = "Sehr geehrte Damen und Herren,";
            }
            else if (rechnung.kunden.anrede == "Herr")
            {
                briefkopfAnrede.Content = rechnung.kunden.anrede + "n";
                anrede.Content = "Sehr geehrter Herr " + rechnung.kunden.nachname + ",";
            }
            else if (rechnung.kunden.anrede == "Frau")
            {
                briefkopfAnrede.Content = rechnung.kunden.anrede;
                anrede.Content = "Sehr geehrte Frau " + rechnung.kunden.nachname + ",";
            }

            briefkopfName.Content = rechnung.kunden.vorname + " " + rechnung.kunden.nachname;
            briefkopfStrasse.Content = rechnung.kunden.strasse;
            briefkopfOrt.Content = rechnung.kunden.postleitzahl + " " + rechnung.kunden.ort;
            briefkopfLand.Content = rechnung.kunden.land;

            ueberschriftRechnungsnummer.Content = rechnung.rechnungsnummer;

            int zeile = 1;
            foreach(rechnung_positionen position in rechnung.rechnung_positionen)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength();
                positionen.RowDefinitions.Add(rowDefinition);

                Label menge = new Label();
                menge.Content = position.menge;
                menge.HorizontalAlignment = HorizontalAlignment.Center;
                positionen.Children.Add(menge);
                Grid.SetColumn(menge, 0);
                Grid.SetRow(menge, zeile);

                Label einheit = new Label();
                einheit.Content = position.einheit;
                positionen.Children.Add(einheit);
                Grid.SetColumn(einheit, 1);
                Grid.SetRow(einheit, zeile);


                Label name = new Label();
                TextBlock tbname = new TextBlock();
                tbname.Text = position.name;
                tbname.TextWrapping = TextWrapping.WrapWithOverflow;
                name.Content = tbname;
                positionen.Children.Add(name);
                Grid.SetColumn(name, 2);
                Grid.SetRow(name, zeile);

                Label beschreibung = new Label();
                TextBlock tbbeschreibung = new TextBlock();
                tbbeschreibung.Text = position.beschreibung;
                tbbeschreibung.TextWrapping = TextWrapping.WrapWithOverflow;
                beschreibung.Content = tbbeschreibung;
                positionen.Children.Add(beschreibung);
                Grid.SetColumn(beschreibung, 3);
                Grid.SetRow(beschreibung, zeile);

                Label einzelpreis = new Label();
                einzelpreis.Content = position.einzelpreis;
                einzelpreis.ContentStringFormat = "c";
                einzelpreis.HorizontalAlignment = HorizontalAlignment.Right;
                positionen.Children.Add(einzelpreis);
                Grid.SetColumn(einzelpreis, 4);
                Grid.SetRow(einzelpreis, zeile);

                Label gesamtpreis = new Label();
                gesamtpreis.Content = position.gesamtpreis;
                gesamtpreis.ContentStringFormat = "c";
                gesamtpreis.HorizontalAlignment = HorizontalAlignment.Right;
                positionen.Children.Add(gesamtpreis);
                Grid.SetColumn(gesamtpreis, 5);
                Grid.SetRow(gesamtpreis, zeile);

                zeile++;
            }

            decimal gesamtbetrag = 0;
            foreach(rechnung_positionen position in rechnung.rechnung_positionen)
            {
                decimal g = position.gesamtpreis ?? default(decimal);
                gesamtbetrag = gesamtbetrag + g;
            }
            prozentMehrwertsteuer.Content = _entities.einstellungen.First().standardmehrwertsteuersatz + "% Mehrwertsteuer:";

            rechnungsbetrag.Content = gesamtbetrag;
            nettobetrag.Content = gesamtbetrag * (100 - _entities.einstellungen.First().standardmehrwertsteuersatz) / 100;
            mehrwertsteuerbetrag.Content = gesamtbetrag * (_entities.einstellungen.First().standardmehrwertsteuersatz) / 100;

            DateTime datum = DateTime.Parse(rechnung.datum);
            DateTime datumzahlbar = datum.AddDays(Convert.ToDouble(rechnung.zahlbartage));

            if (rechnung.skontoprozent == null || rechnung.skontoprozent == 0)
            {
                zahlbar.Text = "Zahlbar innerhalb von " + rechnung.zahlbartage + " Tagen ohne Abzug bis spätestens zum " + datumzahlbar.ToString("dd.MM.yyyy") + ".";
            }
            else
            {
                DateTime datumskonto = datum.AddDays(Convert.ToDouble(rechnung.skontotage));
                zahlbar.Text = "Zahlbar mit " + rechnung.skontoprozent + "% Skonto (" + string.Format("{0:C}", (100 - rechnung.skontoprozent) / 100 * gesamtbetrag) + ") innerhalb von " + rechnung.skontotage + " Tagen bis spätestens zum " + datumskonto.ToString("dd.MM.yyyy") + " oder ohne Abzug (" + string.Format("{0:C}", gesamtbetrag) + ") innerhalb von " + rechnung.zahlbartage + " Tagen bis spätestens zum " + datumzahlbar.ToString("dd.MM.yyyy") + ".";
            }

            //TODO: Abschneiden verhindern!

            footerFirmenname.Content = _entities.einstellungen.First().firmenname;
            footerInhaber.Content = _entities.einstellungen.First().inhaber;
            footerStraße.Content = _entities.einstellungen.First().strasse;
            footerOrt.Content = _entities.einstellungen.First().postleitzahl + " " + _entities.einstellungen.First().ort;
            footerLand.Content = _entities.einstellungen.First().land;

            footerTelefon.Content = _entities.einstellungen.First().telefon;
            footerTelefax.Content = _entities.einstellungen.First().telefax;
            footerEmail.Content = _entities.einstellungen.First().email;
            footerUstidnr.Content = _entities.einstellungen.First().ustidnr;
            footerWebseite.Content = _entities.einstellungen.First().webseite;

            footerEmpfänger.Content = _entities.einstellungen.First().empfaenger;
            footerBank.Content = _entities.einstellungen.First().bank;
            footerIBAN.Content = _entities.einstellungen.First().iban;
            footerBIC.Content = _entities.einstellungen.First().bic;
        }
    }
}
