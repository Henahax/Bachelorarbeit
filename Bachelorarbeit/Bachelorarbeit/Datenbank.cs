using System.IO;
using System.Data.SQLite;

namespace Bachelorarbeit
{
    class Datenbank
    {
        public SQLiteConnection sqliteConnection;

        public Datenbank()
        {
            sqliteConnection = new SQLiteConnection("Data Source=datenbank.sqlite3");

            if (!File.Exists("./datenbank.sqlite3"))
            {
                SQLiteConnection.CreateFile("datenbank.sqlite3");
                string query = "CREATE TABLE [Kunden] (  [Id] INTEGER NOT NULL, [Kundennummer] bigint NOT NULL, [Anrede] text NOT NULL, [Vorname] text NULL, [Nachname] text NULL, [Firma] text NULL, [Straße] text NULL, [Postleitzahl] text NULL, [Ort] text NULL, [Land] text NULL, [Telefon] text NULL, [Telefax] text NULL, [Mobiltelefon] text NULL, [E-Mail] text NULL, [Webseite] text NULL, [Notizen] text NULL, CONSTRAINT [sqlite_master_PK_Kunden] PRIMARY KEY ([Id]));";
                SQLiteCommand sqliteCommand = new SQLiteCommand(query, this.sqliteConnection);
                this.VerbindungOeffnen();
                sqliteCommand.ExecuteNonQuery();
                this.VerbindungSchliessen();
            }
        }

        public void VerbindungOeffnen()
        {
            if(sqliteConnection.State != System.Data.ConnectionState.Open)
            {
                sqliteConnection.Open();
            }
        }

        public void VerbindungSchliessen()
        {
            if(sqliteConnection.State != System.Data.ConnectionState.Closed)
            {
                sqliteConnection.Close();
            }
        }
    }
}
