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
            string query = "SELECT Kundennummer, Vorname, Nachname, Straße, Ort FROM Kunden WHERE Kundennummer LIKE @suche OR Vorname LIKE @suche OR Nachname LIKE @suche OR Straße LIKE @suche OR Ort LIKE @suche";
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
    }
}