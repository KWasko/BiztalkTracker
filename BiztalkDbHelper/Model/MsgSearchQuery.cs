using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiztalkDbHelper.Model
{
    public class MsgSearchQuery
    {
        public BodyAndContextDependedSearchQuery MsgBodyAndContextDependedSearchQuery{ get; set; }
        public string SchemaName { get; set; }
        public string Location { get; set; }    
        public string Port { get; set; }
        public string ServiceName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
       
        public int QueryLimit { get; set; }
    }
    
}
