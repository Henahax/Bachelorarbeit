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
    
    public partial class angebote
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public angebote()
        {
            this.angebot_positionen = new HashSet<angebot_positionen>();
        }
    
        public long id { get; set; }
        public long kunde_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<angebot_positionen> angebot_positionen { get; set; }
        public virtual kunden kunden { get; set; }
    }
}
