using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiztalkDbHelper.Model;
using BiztalkDbHelper.Utils;
using System.Data.SqlClient;

namespace BiztalkDbHelper
{
    public class FakeTrackedMsgsFinder : ITrackedMsgsFinder
    {
        public List<Message> GetTrackedMessages(MsgSearchQuery query, SqlConnection sqlConnection)
        {


            List<Message> messages = new List<Message>();

            for(int i=0; i < 100; i++)
            {
                messages.Add(new Message()
                {
                    Adapter = "Adapter" + i,
                    Id = new Guid(),
                    ServiceId = new Guid(),
                    Body = "Body" + i,
                    Context = null,
                    PortDirection = i % 2 == 0 ? "Send" : "Receive",
                    PortName = "Port" + i,
                    SchemaName = "Schema" + i,
                    ServiceName = "Service" + i,
                    URL = "http://www.fake.com/" + i,
                    TimeStamp = DateTime.Now.AddMinutes(i),
                    Size = 99 * i,
                });
            }
          


            return messages;
        }

        private IEnumerable<Message> ProcessMsgBodyAndContextFiltering(IEnumerable<Message> messages, BodyAndContextDependedSearchQuery query)
        {
            if (!string.IsNullOrEmpty(query.BodyText))
            {
                messages = messages.Where(m => m.Body != null && m.Body.Contains(query.BodyText, StringComparison.InvariantCultureIgnoreCase));
            }
            if (!string.IsNullOrEmpty(query.ReceivePortName))
            {
                messages = messages.Where(m => m.ContextItems.Any(c => c.Property == "ReceivePortName" && c.Value.Contains(query.ReceivePortName, StringComparison.InvariantCultureIgnoreCase)));
            }
            if (!string.IsNullOrEmpty(query.ReceiveLocationName))
            {
                messages = messages.Where(m => m.ContextItems.Any(c => c.Property == "ReceiveLocationName" && c.Value.Contains(query.ReceiveLocationName, StringComparison.InvariantCultureIgnoreCase)));
            }
            if (!string.IsNullOrEmpty(query.ReceivedFileName))
            {
                messages = messages.Where(m => m.ContextItems.Any(c => c.Property == "ReceivedFileName" && c.Value.Contains(query.ReceivedFileName, StringComparison.InvariantCultureIgnoreCase)));
            }

            return messages;
        }
    }
}
