using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.BizTalk.Agent.Interop;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using BiztalkDbHelper.Model;

namespace BiztalkDbHelper.Utils
{
   public class MsgDecompressor
    {
        public string DecompressMsg(byte[] msg)
        {

            string message = "";
            using (MemoryStream stream = new MemoryStream(msg))
            {
                Assembly pipelineAssembly = Assembly.LoadFrom((string.Concat(@"Microsoft.BizTalk.Pipeline.dll")));

                Type compressionStreamsType = pipelineAssembly.GetType("Microsoft.BizTalk.Message.Interop.CompressionStreams", true);

                StreamReader st = new StreamReader((Stream)compressionStreamsType.InvokeMember("Decompress", BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Static, null, null, new object[] { (object)stream }));

                message = st.ReadToEnd();
            }
            return message;
        }

        public string DecompressMsgContextAsXml(byte[] msgContext)
        {
            StringBuilder xml = new StringBuilder();
            using (MemoryStream ms = new MemoryStream(msgContext))
            {
                IBaseMessageContext mc;
                mc = ((IBTMessageAgentFactory)((IBTMessageAgent)new BTMessageAgent())).CreateMessageContext();
                IPersistStream per = ((IPersistStream)mc);
                per.Load(ms);


                xml.AppendLine("<MessageInfo>");
                xml.AppendFormat("<ContextInfo PropertiesCount=\"{0}\">", mc.CountProperties);
                xml.AppendLine();
                string name;
                string ns;
                string value;
                for (int i = 0; i < mc.CountProperties; i++)
                {
                    mc.ReadAt(i, out name, out ns);
                    value = mc.Read(name, ns) as string;
                    xml.AppendFormat("<Property Name=\"{0}\" Value=\"{2}\" Namespace=\"{1}\" />", name, ns, value);
                    xml.AppendLine();
                }
                xml.AppendLine("</ContextInfo>");
                xml.Append("</MessageInfo>");
            };
                
            return xml.ToString();
        }

        public List<ContextItem> DecompressMsgContext(byte[] msgContext)
        {
            List<ContextItem> contextItems = new List<ContextItem>();

            using (MemoryStream ms = new MemoryStream(msgContext))
            {
                IBaseMessageContext mc;
                mc = ((IBTMessageAgentFactory)((IBTMessageAgent)new BTMessageAgent())).CreateMessageContext();
                IPersistStream per = ((IPersistStream)mc);
                per.Load(ms);

                string name;
                string ns;
                string value;
                for (int i = 0; i < mc.CountProperties; i++)
                {
                    mc.ReadAt(i, out name, out ns);
                    value = mc.Read(name, ns) as string;

                    contextItems.Add(new ContextItem
                    {
                        Property = name,
                        Value = value,
                        Namespace = ns
                    });
                }
            };
           
            return contextItems;
        }

    }
}
