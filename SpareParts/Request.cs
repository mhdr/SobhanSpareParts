//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SpareParts
{
    using System;
    using System.Collections.Generic;
    
    public partial class Request
    {
        public int RequestId { get; set; }
        public string PartNo { get; set; }
        public string ResolutionPartNo { get; set; }
        public string PartNoOrignal { get; set; }
        public int Quantity { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> EntranceDate { get; set; }
        public byte[] TimeStamp { get; set; }
        public string Description { get; set; }
    }
}
