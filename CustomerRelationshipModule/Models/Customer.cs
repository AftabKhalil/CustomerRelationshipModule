using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerRelationshipModule.Models
{
    public class Customer
    {
        public string systemId { get; set; }

        public int id { get; set; }
        public string name { get; set; }
        public string contactNo { get; set; }
        public string emailId { get; set; }
        public string password { get; set; }
        public int budget { get; set; }
    }
}