using BiztalkDbHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BiztalkTracker.Model
{
    public class SelectableMessage:Message
    {
        public bool IsSelected { get; set; }
    }
}


