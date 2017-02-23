using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiztalkDbHelper;
using BiztalkDbHelper.Model;

namespace BiztalkTracker.ViewModel
{
    public class MainViewModel
    {
      
        public List<string> ConnectionStringsList { get; set; }
        public string SelectedConnectionString { get; set;}

        public ObservableCollection<Message> Messages { get; set; }
        public MainViewModel()
        {
            Messages = new ObservableCollection<Message>();
            ConnectionStringsList = new List<string> ();
            ConnectionStringsList.AddRange(Properties.Settings.Default.ConnectionStrings.Cast<string>().ToList());
            SelectedConnectionString = ConnectionStringsList.FirstOrDefault();
            GetMessages();
        }
        public void GetMessages()
        {
            using (var con = new SqlConnection(SelectedConnectionString))
            {
                con.Open();

            
            ITrackedMsgsFinder trMsgFinder = new TrackedMsgsFinder();    

            MsgSearchQuery query = new MsgSearchQuery()
            {               
                SchemaName = "Document-Order",
                ServiceName = "OrderToAx2012",
                DateFrom = DateTime.Parse("2017-02-20 00:00:00"),
                // DateTo = new DateTime(2017, 12, 1, 0, 0, 0),
                Location = null,
                //Port = "prtEcodOrder",
                QueryLimit = 500,
                MsgBodyAndContextDependedSearchQuery = new BiztalkDbHelper.Model.BodyAndContextDependedSearchQuery()
                {
                    BodyText = "<OrderNumber>00597717</OrderNumber",
                    ReceiveLocationName=null,
                    ReceivePortName=null                   
                }
            };

            List<Message> messages = trMsgFinder.GetTrackedMessages(query, con);
                Messages.Clear();
             foreach(var msg in messages)
                {
                    Messages.Add(msg);
                }
            }
        }
    }
}
