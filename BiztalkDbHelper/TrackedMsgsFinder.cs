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
	public class TrackedMsgsFinder: ITrackedMsgsFinder
    {
		public List<Message> GetTrackedMessages(MsgSearchQuery query, SqlConnection sqlConnection)
		{
			string sqlQuery = @"SELECT TOP " + query.QueryLimit + @" trackData.[MessageInstance/InstanceID]
	  ,trackData.[ServiceInstance/InstanceID]
	--  ,trackData.[ServiceInstance/ActivityID]
	--  ,trackData.[Service/ServiceClassGUID]
	  ,trackData.[MessageInstance/SchemaName]
	  ,trackData.[Event/Direction]
	  ,trackData.[Event/Port]
	  ,trackData.[Event/Timestamp]
	  ,trackData.[MessageInstance/Size]
	  ,trackData.[Event/Adapter]
	  ,trackData.[Event/URL]
	  ,trackData.[ServiceInstance/ServiceName]
	  , msgContextData.imgContext
   	  ,msgData.imgPart
  FROM [BizTalkDTADb].[dbo].[dtav_FindMessageFacts] as trackData
  left join [BizTalkDTADb].[dbo].[btsv_Tracking_Spool] msgContextData on trackData.[MessageInstance/InstanceID] = msgContextData.uidMsgID
  left join [BizTalkDTADb].[dbo].[btsv_Tracking_Parts] msgData on trackData.[MessageInstance/InstanceID] = msgData.uidMessageID
  where 1=1
";
			if (query.DateFrom.HasValue)
				sqlQuery += string.Format("\nAND trackData.[Event/Timestamp]>='{0}'", query.DateFrom.Value.ToString("yyyy-MM-dd HH:mm:ss"));
			if (query.DateTo.HasValue)
				sqlQuery += string.Format("\nAND trackData.[Event/Timestamp]<='{0}'", query.DateTo.Value.ToString("yyyy-MM-dd HH:mm:ss"));
			if (!string.IsNullOrWhiteSpace(query.SchemaName))
				sqlQuery += string.Format("\nAND trackData.[MessageInstance/SchemaName] LIKE'%{0}%'", query.SchemaName.Replace("'", "''"));
			if (!string.IsNullOrWhiteSpace(query.Location))
				sqlQuery += string.Format("\nAND trackData.[Event/URL] LIKE'%{0}%'", query.Location.Replace("'", "''"));
			if (!string.IsNullOrWhiteSpace(query.Port))
				sqlQuery += string.Format("\nAND trackData.[Event/Port] LIKE'%{0}%'", query.Port.Replace("'", "''"));
			if (!string.IsNullOrWhiteSpace(query.ServiceName))
				sqlQuery += string.Format("\nAND trackData.[ServiceInstance/ServiceName] LIKE'%{0}%'", query.ServiceName.Replace("'", "''"));
			sqlQuery += "\n order by trackData.[Event/Timestamp] desc";

			IEnumerable<Message> messages = new List<Message>();
		   
			SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);
			using (SqlDataReader reader = cmd.ExecuteReader())
			{
				messages = GetMessages(reader);
			}

            if (query.MsgBodyAndContextDependedSearchQuery != null && query.QueryLimit == messages.Count())
            {
                throw new Exception("query result reach its limit, cannot filter by body text because of possibility lack of data");
            }else
            {
                messages = ProcessMsgBodyAndContextFiltering(messages, query.MsgBodyAndContextDependedSearchQuery);
            }
                
            return messages.ToList();
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
        private List<Message> GetMessages(SqlDataReader sqlReader)
		{
			List<Message> messages = new List<Message>();
			MsgDecompressor decompressor = new MsgDecompressor();
			while (sqlReader.Read())
			{
				var message = new Message()
				{
					Id = new Guid(sqlReader["MessageInstance/InstanceID"].ToString()),
					ServiceId = new Guid(sqlReader["ServiceInstance/InstanceID"].ToString()),
					ServiceName = (string)sqlReader["ServiceInstance/ServiceName"],
					SchemaName = (string)sqlReader["MessageInstance/SchemaName"],
					PortName = (string)sqlReader["Event/Port"],
                    PortDirection = (string)sqlReader["Event/Direction"],
                    Adapter = (string)sqlReader["Event/Adapter"],
					TimeStamp = ((DateTime)sqlReader["Event/Timestamp"]).AddHours(2),
					URL = ((string)sqlReader["Event/URL"]),
					Context = sqlReader["imgContext"] != DBNull.Value ? decompressor.DecompressMsgContextAsXml((byte[])sqlReader["imgContext"]) : null,
					ContextItems = sqlReader["imgContext"] != DBNull.Value ? decompressor.DecompressMsgContext((byte[])sqlReader["imgContext"]) : new List<ContextItem>(),
					Body = sqlReader["imgPart"] != DBNull.Value ? decompressor.DecompressMsg((byte[])sqlReader["imgPart"]) : null
				};
				messages.Add(message);
			}

			return messages;
		}
	}
}
