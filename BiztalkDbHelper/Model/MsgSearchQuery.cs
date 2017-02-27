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
        public string Url { get; set; }
        public string Location { get; set; }    
        public string Port { get; set; }
        public string ServiceName { get; set; }
        public string ServiceId { get; set; }
        public string MessageId { get; set; }
        public DateTime? DateFrom { get
            {
                try
                {
                    return DateTime.Parse(DateFromString);
                }
                catch (Exception)
                {

                    return null;
                }
            }
        }

        public string DateFromString { get; set; }
        public string DateToString { get; set; }
        public DateTime? DateTo
        {
            get
            {
                try
                {
                    return DateTime.Parse(DateToString);
                }
                catch (Exception)
                {

                    return null;
                }
            }
        }
        public int QueryLimit { get; set; } = 500;

        public MsgSearchQuery()
        {
            DateFromString = DateTime.Now.Date.ToString();
            DateToString = DateTime.Now.ToString();
        }
    }
    
}
