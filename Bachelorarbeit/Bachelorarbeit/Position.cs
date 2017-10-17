using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bachelorarbeit
{
    class Position
    {
        public int rechnungId { set; get; }
        public bool istUeberschrift { set; get; }
        public float menge { set; get; }
        public string einheit { set; get; }
        public string name { set; get; }
        public string beschreibung { set; get; }
        public float einzelpreis { set; get; }

        public Position()
        {
        }
    }
}
