using System;
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
        public string ReceivedFileName { get; set; }

        public string CustomContextFieldName { get; set; }
        public string CustomContextFieldValue { get; set; }


        public bool IsAnyFilteringSet { get
            {
                return !string.IsNullOrEmpty(BodyText)
                    || !string.IsNullOrEmpty(ReceivePortName)
                    || !string.IsNullOrEmpty(ReceiveLocationName)
                    || !string.IsNullOrEmpty(ReceivedFileName)
                    || !string.IsNullOrEmpty(CustomContextFieldName);
            }
        }
    }
}
