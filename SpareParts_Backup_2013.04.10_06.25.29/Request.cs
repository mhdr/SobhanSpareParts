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
        public string ResolutionPartNo { get; set; }
        public string PartNo { get; set; }
        public string PartNoOriginal { get; set; }
        public System.DateTime RequestDate { get; set; }
        public int Qty { get; set; }
        public Nullable<System.DateTime> EntranceDate { get; set; }
        public string Description { get; set; }
        public byte[] TimeStamp { get; set; }
        public int RequestStatus { get; set; }
    }
}
