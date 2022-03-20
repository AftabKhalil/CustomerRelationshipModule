using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerRelationshipModule.Models
{
    public class DataTableResponse<T>
    {
        public int draw { get; set; }
        public bool isSuccess { get; set; }
        public string error { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered
        {
            get
            {
                return recordsTotal;
            }
        }
        public List<T> data { get; set; }
        public DataTableResponse(int _draw)
        {
            draw = _draw;
            data = new List<T>();
        }
    }
}