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
using System.Windows.Shapes;

namespace Bachelorarbeit
{
    /// <summary>
    /// Interaktionslogik für Einstellungen.xaml
    /// </summary>
    public partial class Einstellungen : Window
    {
        public Einstellungen()
        {
            InitializeComponent();
        }

        private void Speichern(object sender, RoutedEventArgs e)
        {
            if (firmenname.Text == null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Firmenname muss ausgefüllt sein!");
                return;
            }
            /*
            _entities.kunden.Attach(kunde);
            _entities.Entry(kunde).State = EntityState.Modified;
            _entities.SaveChanges();*/
        }
    }
}
