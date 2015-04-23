using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Needletail.Mvc;
using Needletail.Mvc.Communications;

namespace $rootnamespace$.Controllers
{
    public class RemoteController : RemoteExecutionController
    {

        public RemoteController()
        {
            this.IncommingConnectionIdAssigned += new IncommingConnectionIdAssignedDelegate(ApiTestController_IncommingConnectionIdAssigned);
            this.IncommingConnectionAssigningId += new IncommingConnectionAssigningIdDelegate(ApiTestController_IncommingConnectionAssigningId);
            RemoteExecutionController.ConnectionLost += new ConnectionLostDelegate(TwoWayController_ConnectionLost);
            
        }


        void TwoWayController_ConnectionLost(ClientCall call)
        {
            //Implement code that needs to be executed when a connection is lost
        }

        string ApiTestController_IncommingConnectionAssigningId()
        {
            //return an ID to be assigned to the incoming connection
			//a common thing is to return "User.Identity.Name"
			return Guid.NewGuid().ToString();
        }

        void ApiTestController_IncommingConnectionIdAssigned(string newId)
        {
            //code that needs to run after a connection has been succesfully configured
        }

		       
        
    }
}
