using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiztalkDbHelper;

namespace BiztalkTracker.ViewModel
{


    class MainViewModel
    {
        public ObservableCollection<string> Messages { get; set; } = new ObservableCollection<string>();
        public void GetMessages()
        {
            const bool _isProd = true;
            string connectionString = "";
            if (_isProd)
            {
                // connectionString = "Data Source=" + "TPCCLBIZSQLS" + ";Initial Catalog=" + "BizTalkDTADb" + ";Integrated Security=True;User Id=exsalwo;Password=Tikkurila123;"; ;
                connectionString = "Data Source=" + "TPCCLBIZSQLS" + ";Initial Catalog=" + "BizTalkDTADb" + ";Integrated Security=True";
            }
            else
            {
                connectionString = "Data Source=" + "FIS10157V" + ";Initial Catalog=" + "BizTalkDTADb" + ";Integrated Security=True";
            }
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();

            }
            TrackedMsgsFinder trMsgFinder = new TrackedMsgsFinder();

          

            BiztalkDbHelper.Model.MsgSearchQuery query = new BiztalkDbHelper.Model.MsgSearchQuery()
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
        }
    }
}
