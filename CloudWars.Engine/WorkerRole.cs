using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using System.ServiceModel;
using CloudWars.DataAccess.Sql;
using CloudWars.Entities.Player;
using CloudWars.Common;
using CloudWars.DataAccess.Queue;

namespace CloudWars.Engine
{
    public class WorkerRole : RoleEntryPoint
    {
        private Timer timer;
        private IFactory _Factory;
        private IClientFeedback _ClientFeedback;
        
        /// <summary>
        /// The service will take care of:
        /// - Tracking player's activity to determine if is still online
        /// - Tracking games
        /// </summary>
        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("Cloud Wars engine started", "Information");
            //TODO:instantiate the proper factory
            
            timer = new Timer(CheckUserStatus, null, 5000, 5000);

            while (true)
            {
                //check every half of second if there are active matches and process the queue
                Thread.Sleep(500);
                //if no active matches are found, and no logged players  are found, avoid access to the queue
                int onlinePlayers = CloudWarsData.OnlinePlayers();
                int activeMatches = CloudWarsData.ActiveMatches();
                //use a thread pool to process the messages
                if (activeMatches > 0 || onlinePlayers >0)
                {
                    //process messages from matches
                    var msgs = QueueHelper.GetMessagesFromServerQueue();
                    foreach (var m in msgs)
                    {
                        switch (m.Command)
                        { 
                            case Common.Other.Command.GameAction:
                                    switch (m.Action)
                                    {
                                        case Common.Other.GameAction.Attack:
                                            _Factory.GetGame(m.MatchId,true).PlayerAttack(m.Coordinates, m.PlayerId);
                                            break;
                                        case Common.Other.GameAction.PlayerReady:
                                            _Factory.GetGame(m.MatchId, true).PlayerReady(m.PlayerId);
                                            break;
                                        case Common.Other.GameAction.UnitMoved:
                                            _Factory.GetGame().PlayerMoveTo(m.Coordinates, m.UnitId);
                                            break;
                                    }
                                break;                           
                            
                        }
                    }
                    
                }

                //do not process this queue if there are not players logged in
                if (onlinePlayers > 0)
                {
                    //process messages from matches
                    var msgs = QueueHelper.GetMessagesFromClientQueue();
                    foreach (var m in msgs)
                    {
                        switch (m.Command)
                        {
                            case Common.Other.Command.GameAction:
                                switch (m.Action)
                                {
                                    case Common.Other.GameAction.ShotMade:
                                        _Factory.GetClientFeedback().ShotMade(m.MatchId, m.Player1, m.Player2, m.HealthAfterAttack, m.UnitId, m.Coordinates);
                                        break;
                                    case Common.Other.GameAction.ShotMissed:
                                        _Factory.GetClientFeedback().ShotMissed(m.MatchId, m.Player1, m.Player2, m.Coordinates);
                                        break;
                                }
                                break;
                            case Common.Other.Command.ChallengePlayer:
                                _Factory.GetClientFeedback().ChallengePlayer(m.Player1, m.Player2);
                                break;
                            case Common.Other.Command.ChallengeAccepted:
                                //Create the match
                                var match = _Factory.GetGame();
                                match.CreateMatch(m.Player1, m.Player2);
                                _Factory.GetClientFeedback().ChallengeAccepted(match.MatchId, m.Player1, m.Player2);
                                break;
                            case Common.Other.Command.StartGame:
                                _Factory.GetClientFeedback().StartMatch(m.MatchId, m.Player1, m.Player2);
                                break;
                            case Common.Other.Command.EndGame:
                                _Factory.GetClientFeedback().PlayerWon(m.Winner, m.MatchId);
                                _Factory.GetClientFeedback().PlayerLost(m.Losser, m.MatchId);
                                break;

                        }
                    }
                }
                

            }
        }


        /// <summary>
        /// Check if there are users that have not been active for some period of time
        /// </summary>
        public void CheckUserStatus(object state)
        {
            //get the list of users
            var players = CloudWarsData.GetPlayers();
            var matches = CloudWarsData.GetMatches();
            //process the list
            foreach (var p in players)
            {
                if (!p.IsOnline)
                    continue;
                //check the last time this user was active
                TimeSpan difference = DateTime.UtcNow - p.LastActivity;
                if (difference.TotalSeconds > 30)
                {
                    //check if the user is part of an active match
                    var match = matches.FirstOrDefault(m => m.Player1 == p.Id || m.Player2 == p.Id);
                    //do whatever is needed
                    if (match != null)
                    {
                        //the match is lost
                        var game = _Factory.GetGame(match.Id, false);
                        game.MatchFinished(p.Id == match.Player1 ? match.Player2 : match.Player1, p.Id != match.Player1 ? match.Player2 : match.Player1);
    
                    }
                    //update the player
                    CloudWarsData.UpdatePlayer(values: new { IsOnline = false, Status = PlayerStatus.OffLine}, where: new { Id = p.Id });
                }
                
            }
        }


        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }

        
    }
}
