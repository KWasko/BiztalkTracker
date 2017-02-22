using BiztalkDbHelper.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiztalkDbHelper
{
    interface ITrackedMsgsFinder
    {
        List<Message> GetTrackedMessages(MsgSearchQuery query, SqlConnection sqlConnection)
    }
}
