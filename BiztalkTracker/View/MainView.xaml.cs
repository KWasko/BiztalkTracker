using BiztalkTracker.Model;
using BiztalkTracker.View;
using BiztalkTracker.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BiztalkTracker.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        const bool _isProd = true;

        public MainView()
        {
          
            InitializeComponent();

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

            var search = new SearchData()
            {
                BodyText = "<OrderNumber>00597717</OrderNumber",
                SchemaName = "Document-Order",
                ServiceName="OrderToAx2012",
                DateFrom =  DateTime.Parse("2017-02-20 00:00:00"),
               // DateTo = new DateTime(2017, 12, 1, 0, 0, 0),
                Location = null,               
                //ReceivePortName="ECODOrder_RP",
                //ReceiveLocationName="null",
                //Port = "prtEcodOrder",
                QueryLimit=500

            };
            //var messages = GetTrackedMessages(search);
            //dataGrid.ItemsSource = messages;
        }

        

        private void dataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0) return;
            contextTextBox.Text = (e.AddedCells[0].Item as Message).Context;
            bodyTextBox.Text = (e.AddedCells[0].Item as Message).BodyFormatted;
        }
    }
}
