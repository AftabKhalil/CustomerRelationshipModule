//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class FeedBack
    {
        public int id { get; set; }
        public int task_assignment_id { get; set; }
        public string message { get; set; }
        public int sentiment { get; set; }
    
        public virtual TaskAssignment TaskAssignment { get; set; }
    }
}
