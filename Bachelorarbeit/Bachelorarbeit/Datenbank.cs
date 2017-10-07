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
