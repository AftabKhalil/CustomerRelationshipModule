using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerRelationshipModule.Models
{
    public class Employee
    {
        public string systemId { get; set; }

        public int id { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public int salary { get; set; }
        public int expirence { get; set; }
        public string contactNo { get; set; }
        public string emailId { get; set; }
        public string password { get; set; }
    }
}