//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Network_Editor
{
    using System;
    using System.Collections.Generic;
    
    public partial class Network_Versions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Network_Versions()
        {
            this.Network_Connections = new HashSet<Network_Connections>();
            this.Network_Hardware = new HashSet<Network_Hardware>();
            this.Network_Ports = new HashSet<Network_Ports>();
            this.Network_Settings = new HashSet<Network_Settings>();
        }
    
        public int ID { get; set; }
        public int C_Version_Number { get; set; }
        public string C_Date { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Network_Connections> Network_Connections { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Network_Hardware> Network_Hardware { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Network_Ports> Network_Ports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Network_Settings> Network_Settings { get; set; }
    }
}