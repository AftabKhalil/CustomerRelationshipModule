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
    
    public partial class TaskAssignment
    {
        public int id { get; set; }
        public int task_id { get; set; }
        public int employee_id { get; set; }
        public int task_type { get; set; }
        public string message { get; set; }
        public int sentiment { get; set; }
        public bool is_completed { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Task Task { get; set; }
    }
}
