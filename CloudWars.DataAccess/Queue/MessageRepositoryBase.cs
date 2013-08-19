using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Impulso.Azure.Storage.Repositories;
using System.Xml.Serialization;
using CloudWars.Entities.Queue;
using System.IO;

namespace CloudWars.DataAccess.Queue
{
    internal abstract class MessageRepositoryBase : AzureQueueRepositoryBase
    {

        public abstract override string QueueName { get; set; }
        
        /// <summary>
        /// Constructor, initialize storage accounts
        /// </summary>
        /// <param name="connectionStringSection">the connection string</param>
        public MessageRepositoryBase(string connectionStringSection)
            : base(connectionStringSection)
        { 
        }


        /// <summary>
        /// Add a new email to the queue
        /// </summary>
        /// <param name="email">email to add</param>
        public void AddMessageToQueue(Message msg)
        {
            //serialize the message
            XmlSerializer x = new XmlSerializer(typeof(Message));
            var ms = new MemoryStream();
            x.Serialize(ms, msg);
            
            ms.Position = 0;
            AddMessage(new StreamReader(ms).ReadToEnd());
        }

        public List<Message> GetMessages()
        {
            var messages = new List<Message>();
            Message m;
            var qms = this.GetAllMessages();
            foreach (var q in qms)
            {
                XmlSerializer x = new XmlSerializer(typeof(Message));
                var ms = new MemoryStream(q.AsBytes);
                ms.Position = 0;
                m = x.Deserialize(ms) as Message;
                messages.Add(m);
                DeleteMessage(q);
            }

            return messages;
        }
    }
}
