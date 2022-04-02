using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerRelationshipModule.Models
{
    public class Task
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
    }
}