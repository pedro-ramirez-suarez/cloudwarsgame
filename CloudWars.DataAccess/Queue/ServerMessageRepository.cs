using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Impulso.Azure.Storage.Repositories;
using CloudWars.Entities.Queue;
using System.Xml.Serialization;
using System.IO;

namespace CloudWars.DataAccess.Queue
{
    internal class ServerMessageRepository : MessageRepositoryBase
    {

        public override string QueueName
        {
            get
            {
                return "ServerMessages";
            }
            set
            {
                
            }
        }


        /// <summary>
        /// Constructor, initialize storage accounts
        /// </summary>
        /// <param name="connectionStringSection">the connection string</param>
        public ServerMessageRepository(string connectionStringSection)
            : base(connectionStringSection)
        { 
        }
        
    }
}
