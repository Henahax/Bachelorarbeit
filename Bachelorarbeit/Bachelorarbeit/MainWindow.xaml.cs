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

using System.Data.SQLite;
using System.Data;

namespace Bachelorarbeit
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            KundenlisteFuellen();

            /*
            //Datenbank Eintrag anlegen
            Datenbank databaseObject = new Datenbank();
            string query = "INSERT INTO Kunden (`Vorname`, `Nachname`) VALUES (@Vorname, @Nachname)";
            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            databaseObject.OpenConnection();
            myCommand.Parameters.AddWithValue("@Vorname", "Gundula");
            myCommand.Parameters.AddWithValue("@Nachname", "Gause");
            var result = myCommand.ExecuteNonQuery();
            databaseObject.CloseConnection();
            Console.WriteLine("Zeilen hinzugefügt : {0}", result);
            */

            /*
            //Datenbank Einträge anzeigen
            Datenbank databaseObject = new Datenbank();
            string query = "SELECT * FROM Kunden";
            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            databaseObject.OpenConnection();
            SQLiteDataReader result = myCommand.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    Console.WriteLine("Vorname : {0} - Nachname : {1}", result["Vorname"], result["Nachname"]);
                }
            }
            databaseObject.CloseConnection();
            */
        }

        private void EinstellungenOeffnen(object sender, RoutedEventArgs e)
        {
            Einstellungen einstellungen = new Einstellungen();
            einstellungen.ShowDialog();
        }

        private void KundenlisteFuellen()
        {
            Datenbank datenbank = new Datenbank();
            string suche = "%" + kundenSuche.Text + "%";
            string query = "SELECT Id, Kundennummer, Vorname, Nachname, Straße, Ort FROM Kunden WHERE Id LIKE @suche OR Kundennummer LIKE @suche OR Vorname LIKE @suche OR Nachname LIKE @suche OR Straße LIKE @suche OR Ort LIKE @suche";
            SQLiteCommand sqliteCommand = new SQLiteCommand(query, datenbank.sqliteConnection);
            sqliteCommand.Parameters.AddWithValue("@suche", suche);
            datenbank.VerbindungOeffnen();
            SQLiteDataAdapter sqliteDataAdapter = new SQLiteDataAdapter(sqliteCommand);
            datenbank.VerbindungSchliessen();
            DataTable dataTable = new DataTable("Kunden");
            sqliteDataAdapter.Fill(dataTable);
            kundenListe.ItemsSource = dataTable.DefaultView;
        }

        private void SucheEingeben(object sender, TextChangedEventArgs e)
        {
            KundenlisteFuellen();
        }

        private void KundeAusgewaehlt(object sender, SelectedCellsChangedEventArgs e)
        {
            if (kundenListe.SelectedCells != null)
            {
                kundenlisteBearbeiten.IsEnabled = true;
                kundenlisteLoeschen.IsEnabled = true;
            }
            else
            {
                kundenlisteBearbeiten.IsEnabled = false;
                kundenlisteLoeschen.IsEnabled = false;
            }
        }

        private void KundenlisteBearbeiten(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)kundenListe.SelectedItems[0];

            int id = 0;
            if (Int32.TryParse(row["Id"].ToString(), out id) == false)
            {
                return;
            }

            groupBoxKunde.IsEnabled = true;
            groupBoxKunden.IsEnabled = false;

            Datenbank datenbank = new Datenbank();
            string query = "SELECT Id, Kundennummer, Anrede, Vorname, Nachname, Firma, Straße, Postleitzahl, Ort, Land, Telefon, Telefax, Mobiltelefon, `E-Mail`, Webseite, Notizen FROM Kunden WHERE Id = @id";
            SQLiteCommand sqliteCommand = new SQLiteCommand(query, datenbank.sqliteConnection);
            sqliteCommand.Parameters.AddWithValue("@id", id);
            datenbank.VerbindungOeffnen();
            SQLiteDataAdapter sqliteDataAdapter = new SQLiteDataAdapter(sqliteCommand);
            datenbank.VerbindungSchliessen();
            DataSet dataSet = new DataSet();
            sqliteDataAdapter.Fill(dataSet);
            kundeKundennummer.Content = dataSet.Tables[0].Rows[0]["Kundennummer"];

            //TODO: Anrede

            kundeVorname.Text = dataSet.Tables[0].Rows[0]["Vorname"].ToString();
            kundeNachname.Text = dataSet.Tables[0].Rows[0]["Nachname"].ToString();
            kundeFirma.Text = dataSet.Tables[0].Rows[0]["Firma"].ToString();
            kundeStraße.Text = dataSet.Tables[0].Rows[0]["Straße"].ToString();
            kundePostleitzahl.Text = dataSet.Tables[0].Rows[0]["Postleitzahl"].ToString();
            kundeOrt.Text = dataSet.Tables[0].Rows[0]["Ort"].ToString();
            kundeLand.Text = dataSet.Tables[0].Rows[0]["Land"].ToString();
            kundeTelefon.Text = dataSet.Tables[0].Rows[0]["Telefon"].ToString();
            kundeTelefax.Text = dataSet.Tables[0].Rows[0]["Telefax"].ToString();
            kundeMobiltelefon.Text = dataSet.Tables[0].Rows[0]["Mobiltelefon"].ToString();
            kundeEmail.Text = dataSet.Tables[0].Rows[0]["E-Mail"].ToString();
            kundeWebseite.Text = dataSet.Tables[0].Rows[0]["Webseite"].ToString();

            //TODO: Notizen
        }

        private void KundenlisteLoeschen(object sender, RoutedEventArgs e)
        {
            //TODO: Überprüfung ob Angebote/Rechnungen zu diesem Kunden existieren
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Sind sie sicher, dass sie diesen Kunden löschen wollen?", "Kunden Löschen Bestätigung", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                DataRowView row = (DataRowView)kundenListe.SelectedItems[0];

                int id = 0;
                if (Int32.TryParse(row["Id"].ToString(), out id) == false)
                {
                    return;
                }

                Datenbank datenbank = new Datenbank();
                string query = "DELETE FROM Kunden WHERE Id = @id";
                SQLiteCommand sqliteCommand = new SQLiteCommand(query, datenbank.sqliteConnection);
                sqliteCommand.Parameters.AddWithValue("@id", id);
                datenbank.VerbindungOeffnen();
                sqliteCommand.ExecuteNonQuery();
                datenbank.VerbindungSchliessen();
                KundenlisteFuellen();

                kundenListe.SelectedCells.Clear();
                kundenListe.SelectedItem = null;

                kundenlisteBearbeiten.IsEnabled = false;
                kundenlisteLoeschen.IsEnabled = false;
            }
        }

        private void KundeSpeichern(object sender, RoutedEventArgs e)
        {
            int kundennummer = 0;
            Int32.TryParse(kundeKundennummer.Content.ToString(), out kundennummer);
            //TODO: Exception behandeln falls false

            string anrede = kundeAnrede.Text;
            string vorname = kundeVorname.Text;
            string nachname =  kundeNachname.Text;
            string firma = kundeFirma.Text;
            string straße = kundeStraße.Text;
            string postleitzahl = kundePostleitzahl.Text;
            string ort = kundeOrt.Text;
            string land = kundeLand.Text;
            string telefon = kundeTelefon.Text;
            string telefax = kundeTelefax.Text;
            string mobiltelefon = kundeMobiltelefon.Text;
            string email = kundeEmail.Text;
            string webseite = kundeWebseite.Text;
            string notizen = new TextRange(kundeNotizen.Document.ContentStart, kundeNotizen.Document.ContentEnd).Text;

            if (anrede == "")
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Anrede muss ausgefüllt sein!");
                return;
            }
            if (nachname == "" && firma == "")
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Nachname oder Firma muss ausgefüllt sein!");
                return;
            }

            //Prüfen ob Kundennummer existiert
            Datenbank datenbank = new Datenbank();
            string query = "SELECT COUNT(Id) FROM Kunden WHERE Kundennummer = @kundennummer";
            SQLiteCommand sqliteCommand = new SQLiteCommand(query, datenbank.sqliteConnection);
            sqliteCommand.Parameters.AddWithValue("@kundennummer", kundennummer);
            datenbank.VerbindungOeffnen();
            SQLiteDataReader result = sqliteCommand.ExecuteReader();
            
            //TODO: Schöner?
            int count = 0;
            if (result.HasRows)
            {
                while (result.Read())
                {
                    Int32.TryParse(result[0].ToString(), out count);
                }
            }

            datenbank.VerbindungSchliessen();

            //TODO: Anrede und Notizen Testen

            if(count == 0)
            {
                //ANLEGEN
                Datenbank datenbank2 = new Datenbank();
                string query2 = "INSERT INTO Kunden (`Kundennummer`, `Anrede`, `Vorname`, `Nachname`, `Firma`, `Straße`, `Postleitzahl`, `Ort`, `Land`, `Telefon`, `Telefax`, `Mobiltelefon`, `E-Mail`, `Webseite`, `Notizen`) VALUES (@kundennummer, @anrede, @vorname, @nachname, @firma, @straße, @postleitzahl, @ort, @land, @telefon, @telefax, @mobiltelefon, @email, @webseite, @notizen)";
                SQLiteCommand sqliteCommand2 = new SQLiteCommand(query2, datenbank2.sqliteConnection);
                sqliteCommand2.Parameters.AddWithValue("@kundennummer", kundennummer);
                sqliteCommand2.Parameters.AddWithValue("@anrede", anrede);
                sqliteCommand2.Parameters.AddWithValue("@vorname", vorname);
                sqliteCommand2.Parameters.AddWithValue("@nachname", nachname);
                sqliteCommand2.Parameters.AddWithValue("@firma", firma);
                sqliteCommand2.Parameters.AddWithValue("@straße", straße);
                sqliteCommand2.Parameters.AddWithValue("@postleitzahl", postleitzahl);
                sqliteCommand2.Parameters.AddWithValue("@ort", ort);
                sqliteCommand2.Parameters.AddWithValue("@land", land);
                sqliteCommand2.Parameters.AddWithValue("@telefon", telefon);
                sqliteCommand2.Parameters.AddWithValue("@telefax", telefax);
                sqliteCommand2.Parameters.AddWithValue("@mobiltelefon", mobiltelefon);
                sqliteCommand2.Parameters.AddWithValue("@email", email);
                sqliteCommand2.Parameters.AddWithValue("@webseite", webseite);
                sqliteCommand2.Parameters.AddWithValue("@notizen", notizen);

                datenbank2.VerbindungOeffnen();

                sqliteCommand2.ExecuteNonQuery();

                datenbank2.VerbindungSchliessen();
            }
            else
            {
                //Ändern
                Datenbank datenbank2 = new Datenbank();
                string query2 = "UPDATE Kunden SET `Kundennummer` = @kundennummer, `Anrede` = @anrede, `Vorname` = @vorname, `Nachname` = @nachname, `Firma` = @firma, `Straße` = @straße, `Postleitzahl` = @postleitzahl, `Ort` = @ort, `Land` = @land, `Telefon` = @telefon, `Telefax` = @telefax, `Mobiltelefon` = @mobiltelefon, `E-Mail` = @email, `Webseite` = @webseite, `Notizen` = @notizen WHERE `Kundennummer` = @kundennummer";
                SQLiteCommand sqliteCommand2 = new SQLiteCommand(query2, datenbank2.sqliteConnection);
                sqliteCommand2.Parameters.AddWithValue("@kundennummer", kundennummer);
                sqliteCommand2.Parameters.AddWithValue("@anrede", anrede);
                sqliteCommand2.Parameters.AddWithValue("@vorname", vorname);
                sqliteCommand2.Parameters.AddWithValue("@nachname", nachname);
                sqliteCommand2.Parameters.AddWithValue("@firma", firma);
                sqliteCommand2.Parameters.AddWithValue("@straße", straße);
                sqliteCommand2.Parameters.AddWithValue("@postleitzahl", postleitzahl);
                sqliteCommand2.Parameters.AddWithValue("@ort", ort);
                sqliteCommand2.Parameters.AddWithValue("@land", land);
                sqliteCommand2.Parameters.AddWithValue("@telefon", telefon);
                sqliteCommand2.Parameters.AddWithValue("@telefax", telefax);
                sqliteCommand2.Parameters.AddWithValue("@mobiltelefon", mobiltelefon);
                sqliteCommand2.Parameters.AddWithValue("@email", email);
                sqliteCommand2.Parameters.AddWithValue("@webseite", webseite);
                sqliteCommand2.Parameters.AddWithValue("@notizen", notizen);

                datenbank2.VerbindungOeffnen();

                sqliteCommand2.ExecuteNonQuery();

                datenbank2.VerbindungSchliessen();

            }

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
            kundeNotizen.Document.Blocks.Clear();
            KundenlisteFuellen();
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
                kundeNotizen.Document.Blocks.Clear();
            }
        }

        private void KundenlisteAnlegen(object sender, RoutedEventArgs e)
        {
            groupBoxKunde.IsEnabled = true;
            groupBoxKunden.IsEnabled = false;

            Datenbank datenbank = new Datenbank();
            string query = "SELECT Kundennummer FROM Kunden ORDER BY Kundennummer DESC LIMIT 1";
            SQLiteCommand sqliteCommand = new SQLiteCommand(query, datenbank.sqliteConnection);
            datenbank.VerbindungOeffnen();
            SQLiteDataAdapter sqliteDataAdapter = new SQLiteDataAdapter(sqliteCommand);
            datenbank.VerbindungSchliessen();

            DataSet dataSet = new DataSet();
            sqliteDataAdapter.Fill(dataSet);

            int kundennummer = 0;

            try
            {
                Int32.TryParse(dataSet.Tables[0].Rows[0]["Kundennummer"].ToString(), out kundennummer);
            }
            catch (Exception)
            {

            }
            kundennummer++;
            kundeKundennummer.Content = kundennummer;
        }
    }
}