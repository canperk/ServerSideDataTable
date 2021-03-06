﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataTableServerSide.Helpers
{
    public class ViewConfiguration
    {
        public string GetAddress { get; set; }
        public string ContainerName { get; set; }
        public string FormId { get; set; }
        public string TableName { get; set; }
        public string SaveAction { get; set; }
        public string ViewTabName { get; set; }
        public string InputTabName { get; set; }
    }
}
