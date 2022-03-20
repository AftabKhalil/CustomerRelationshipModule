﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerRelationshipModule.Models
{
    public class AjaxResponse<T>
    {
        public bool isSuccess { get; set; }
        public string error { get; set; }

        public T data { get; set; }
    }
}