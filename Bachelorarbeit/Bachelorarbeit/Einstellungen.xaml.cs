using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bachelorarbeit
{
    /// <summary>
    /// Interaktionslogik für Einstellungen.xaml
    /// </summary>
    public partial class Einstellungen : Window
    {
        mainEntities _entities;
        public Einstellungen()
        {
            InitializeComponent();
            Refresh();
            FelderFuellen();
        }

        private void Refresh()
        {
            if (_entities != null)
            {
                _entities.Dispose();
            }
            _entities = new mainEntities();
        }

        private void FelderFuellen()
        {
            einstellungen einstellung = _entities.einstellungen.First();

            standardmehrwertsteuersatz.Text = einstellung.standardmehrwertsteuersatz.ToString();
            standardland.Text = einstellung.standardland;
            speicherortRechnungen.Text = einstellung.speicherortrechnungen;
            speicherortAngebote.Text = einstellung.speicherortangebote;

            firmenname.Text = einstellung.firmenname;
            inhaber.Text = einstellung.inhaber;
            strasse.Text = einstellung.strasse;
            postleitzahl.Text = einstellung.postleitzahl;
            ort.Text = einstellung.ort;
            land.Text = einstellung.land;
            telefon.Text = einstellung.telefon;
            telefax.Text = einstellung.telefax;
            email.Text = einstellung.email;
            webseite.Text = einstellung.webseite;
            ustidnr.Text = einstellung.ustidnr;
            empfaenger.Text = einstellung.empfaenger;
            bank.Text = einstellung.bank;
            iban.Text = einstellung.iban;
            bic.Text = einstellung.bic;
        }

        private void Speichern(object sender, RoutedEventArgs e)
        {
            //TODO: Mehrwertsteuersatz auf gültigkeit prüfen

            if (standardmehrwertsteuersatz.Text == null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Allgemein: Standardmehrwertsteuersatz muss ausgefüllt sein!");
                return;
            }

            if (Directory.Exists(speicherortRechnungen.Text) == false)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Allgemein: Der Pfad für den Speicherort für PDF Rechnungen ist ungültig!");
                return;
            }

            if (Directory.Exists(speicherortAngebote.Text) == false)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Allgemein: Der Pfad für den Speicherort für PDF Angebote ist ungültig!");
                return;
            }

            if (firmenname.Text == null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Firmendaten: Firmenname muss ausgefüllt sein!");
                return;
            }

            if (inhaber.Text == null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Firmendaten: Inhaber muss ausgefüllt sein!");
                return;
            }

            if (strasse.Text == null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Firmendaten: Straße muss ausgefüllt sein!");
                return;
            }

            if (postleitzahl.Text == null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Firmendaten: Postleitzahl muss ausgefüllt sein!");
                return;
            }

            if (ort.Text == null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Firmendaten: Ort muss ausgefüllt sein!");
                return;
            }

            if (land.Text == null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Firmendaten: Land muss ausgefüllt sein!");
                return;
            }

            //TODO: Evtl optional machen
            if (telefon.Text == null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Firmendaten: Telefon muss ausgefüllt sein!");
                return;
            }

            //TODO: Evtl optional machen
            if (telefax.Text == null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Firmendaten: Telefax muss ausgefüllt sein!");
                return;
            }

            //TODO: Evtl optional machen
            if (email.Text == null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Firmendaten: E-Mail muss ausgefüllt sein!");
                return;
            }

            //TODO: Evtl optional machen
            if (webseite.Text == null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Firmendaten: Webseite muss ausgefüllt sein!");
                return;
            }

            if (ustidnr.Text == null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Firmendaten: USt-IdNr. muss ausgefüllt sein!");
                return;
            }

            if (empfaenger.Text == null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Firmendaten: Kontoinhaber muss ausgefüllt sein!");
                return;
            }

            if (bank.Text == null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Firmendaten: Bank muss ausgefüllt sein!");
                return;
            }

            if (iban.Text == null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Firmendaten: IBAN muss ausgefüllt sein!");
                return;
            }

            if (bic.Text == null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Firmendaten: BIC muss ausgefüllt sein!");
                return;
            }

            einstellungen einstellung = _entities.einstellungen.First();

            decimal steuersatz;

            decimal.TryParse(standardmehrwertsteuersatz.Text, out steuersatz);
            einstellung.standardmehrwertsteuersatz = steuersatz;

            einstellung.standardland = standardland.Text;
            einstellung.speicherortrechnungen = speicherortRechnungen.Text;
            einstellung.speicherortangebote = speicherortAngebote.Text;

            einstellung.firmenname = firmenname.Text;
            einstellung.inhaber = inhaber.Text;
            einstellung.strasse = strasse.Text;
            einstellung.postleitzahl = postleitzahl.Text;
            einstellung.ort = ort.Text;
            einstellung.land = land.Text;
            einstellung.telefon = telefon.Text;
            einstellung.telefax = telefax.Text;
            einstellung.email = email.Text;
            einstellung.webseite = webseite.Text;
            einstellung.ustidnr = ustidnr.Text;
            einstellung.empfaenger = empfaenger.Text;
            einstellung.bank = bank.Text;
            einstellung.iban = iban.Text;
            einstellung.bic = bic.Text;

            _entities.einstellungen.Attach(einstellung);
            _entities.Entry(einstellung).State = EntityState.Modified;
            _entities.SaveChanges();
            Refresh();
            this.Close();
    }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource einstellungenViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("einstellungenViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // einstellungenViewSource.Source = [generic data source]
        }

        private void SpeicherortRechnungenDurchsuchen(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if(folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                speicherortRechnungen.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void SpeicherortAngeboteDurchsuchen(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                speicherortAngebote.Text = folderBrowserDialog.SelectedPath;
            }
        }
    }
}
