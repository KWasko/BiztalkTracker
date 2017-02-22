using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BiztalkTracker.Model
{
    public class Message
    {
         public Guid Id { get; set; }
         public Guid ServiceId { get; set; }
         public string ServiceName { get; set; }
         public string SchemaName { get; set; }
         public string PortName { get; set; }
         public DateTime TimeStamp { get; set; }
         public string Adapter { get; set; }
         public string URL { get; set; }
         public string Body { get; set; }
         public string BodyFormatted
         {
             get
             {
                 if (Body == null) return null;
                 try
                 {
                     XDocument doc = XDocument.Parse(Body);
                     return doc.ToString();
                 }
                 catch (Exception)
                 {
                     return Body;
                 }
             }
         }
         public string Context { get; set; }

         public List<ContextItem> ContextItems { get; set; }
    }
}


