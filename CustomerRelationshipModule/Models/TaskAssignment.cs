using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerRelationshipModule.Models
{
    public class TaskAssignment
    {
        public int Id { get; set; }

        public string AssignmentType { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Review { get; set; }
        public string Rating { get; set; }
    }
}