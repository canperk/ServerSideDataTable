﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataTableServerSide.Helpers
{
    public class ClientSideModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsRequired { get; set; }
        public bool IsHidden { get; set; }
        public string FullName { get; set; }
        public bool IsArray { get; set; }
        public bool IsNumber { get; set; }
        public bool IsText { get; set; }
        public bool IsOrderable { get; set; }
    }
}
