//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bachelorarbeit
{
    using System;
    using System.Collections.Generic;
    
    public partial class kunden
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public kunden()
        {
            this.angebote = new HashSet<angebote>();
            this.rechnungen = new HashSet<rechnungen>();
        }
    
        public long id { get; set; }
        public long kundennummer { get; set; }
        public string anrede { get; set; }
        public string titel { get; set; }
        public string vorname { get; set; }
        public string nachname { get; set; }
        public string firma { get; set; }
        public string strasse { get; set; }
        public string postleitzahl { get; set; }
        public string ort { get; set; }
        public string land { get; set; }
        public string telefon { get; set; }
        public string telefax { get; set; }
        public string mobiltelefon { get; set; }
        public string email { get; set; }
        public string webseite { get; set; }
        public string notizen { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<angebote> angebote { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rechnungen> rechnungen { get; set; }
    }
}
