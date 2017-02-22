﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiztalkDbHelper.Model
{
    public class BodyAndContextDependedSearchQuery
    {
        public string BodyText { get; set; }
        public string ReceivePortName { get; set; }
        public string ReceiveLocationName { get; set; }
        public string ReceiveFileName { get; set; }
    }
}