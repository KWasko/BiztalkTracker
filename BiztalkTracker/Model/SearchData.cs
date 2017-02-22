using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiztalkTracker.Model
{
    public class SearchData
    {
        public string SchemaName { get; set; }
        public string Location { get; set; }
        //public string ReceivePort { get; set; }
        public string ReceivePortName { get; set; }
        public string ReceiveLocationName { get; set; }
        public string Port { get; set; }
        public string ServiceName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string BodyText { get; set; }
        public int QueryLimit { get; set; }
    }
}
