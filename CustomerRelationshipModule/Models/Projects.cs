using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerRelationshipModule.Models
{
    public class Project
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public int Budget { get; set; }
        public string CustomerName { get; set; }
    }
}