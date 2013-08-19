using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudWars.DataAccess.Queue
{
    internal class ClientMessageRepository : MessageRepositoryBase
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
        public ClientMessageRepository (string connectionStringSection)
            : base(connectionStringSection)
        { 
        }

    }
}
