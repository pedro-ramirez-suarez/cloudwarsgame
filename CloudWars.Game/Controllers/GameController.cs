using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClourWars.Web.Code;
using CloudWars.Common;
using CloudWars.SpaceBattle;
using CloudWars.Common.Other;
using ClourWars.Web.ServerClientEntities;
using Microsoft.Security.Application;
using CloudWars.Web.Code;
using Needletail.Mvc;
using Needletail.Mvc.Communications;

namespace ClourWars.Game.Controllers
{
    public class GameController : Controller
    {
        
        public ActionResult Play(Guid matchId)
        {
            if (!ProfileHelper.UserHasProfile(ProfileHelper.GetIdentity))
            {
                return RedirectToAction("CreateProfile", "Player");
            }

            ViewBag.MatchId = matchId;
            return PartialView();
        }


        [Authorize]
        public ActionResult OnlinePlayers()
        {
            if (!ProfileHelper.UserHasProfile(ProfileHelper.GetIdentity))
            {
                return RedirectToAction("CreateProfile", "Player");
            }
            return PartialView(GameHelper.GetOnlinePlayers(ProfileHelper.GetIdentity));
        }

        [Authorize]
        public ActionResult GetNearByPlayers()
        {
            if (!ProfileHelper.UserHasProfile(ProfileHelper.GetIdentity))
            {
                return RedirectToAction("CreateProfile", "Player");
            }
            return PartialView(GameHelper.GetNearbyPlayers(ProfileHelper.GetIdentity));
        }


        [Authorize]
        public ActionResult GetAllPlayers()
        {
            if (!ProfileHelper.UserHasProfile(ProfileHelper.GetIdentity))
            {
                return RedirectToAction("CreateProfile", "Player");
            }
            return PartialView(GameHelper.GetOnlinePlayers(ProfileHelper.GetIdentity));
        }


        [Authorize]
        [HttpPost]
        public ActionResult MyChallenges()
        {
            if (!ProfileHelper.UserHasProfile(ProfileHelper.GetIdentity))
            {
                return RedirectToAction("CreateProfile", "Player");
            }
            return PartialView(GameHelper.GetMyChallenges(ProfileHelper.GetIdentity));
        }

        
        [Authorize]
        [HttpPost]
        public ActionResult MyMatches()
        {
            if (!ProfileHelper.UserHasProfile(ProfileHelper.GetIdentity))
            {
                return RedirectToAction("CreateProfile", "Player");
            }
            return PartialView(GameHelper.GetMyMatches(ProfileHelper.GetIdentity));
        }


        #region Game play code, these methods are called from the game


        //use DI for this or resolve this using web.config
        IFactory factory = new SpaceBattleFactory();


        /// <summary>
        /// Challenge a player
        /// </summary>
        [HttpPost]
        public TwoWayResult ChallengePlayer(Guid playerId)
        {
            string clientId = factory.GetPlayerPresence().GetClientId(playerId);
            Guid fromId = factory.GetPlayerPresence().GetPlayerId(Context.ConnectionId);
            factory.GetGame().ChallengePlayer(fromId, playerId);
            //inform the player that has a new challenge
            //Clients[clientId].newChallenge();
            dynamic call = new ClientCall { CallerId = Context.ConnectionId, ClientId = clientId };
            call.game.newChallenge(); 
            return new TwoWayResult(call);
        }

        /// <summary>
        /// Initialize a player in the server
        /// </summary>
        /// <param name="liveId">id of the player</param>
        /// <param name="latitude">latitude position</param>
        /// <param name="longitude">longitude position</param>
        [HttpPost]
        public void Initialize(string liveId, string latitude, string longitude)
        {
            if (string.IsNullOrWhiteSpace(liveId))
                return;
            latitude = string.IsNullOrWhiteSpace(latitude) ? "1" : latitude;
            longitude = string.IsNullOrWhiteSpace(longitude) ? "1" : longitude;
            factory.GetPlayerPresence().PlayerIsOnLine(liveId, Context.ConnectionId, double.Parse(latitude), double.Parse(longitude));
        }

        /// <summary>
        /// The challenge was accepted
        /// </summary>
        [HttpPost]
        public TwoWayResult AcceptChallenge(Guid challengeId)
        {
            //get the challenge
            var match = factory.GetGame().AcceptChallenge(challengeId);
            //inform the challenger that the challenge was accepted
            string clientId = factory.GetPlayerPresence().GetClientId(match.Item2);
            
            if (!string.IsNullOrWhiteSpace(clientId))
            {
                //Clients[clientId].challengeAccepted(match.Item1);
                //The client accepted the challenge                
                dynamic call1 = new ClientCall { CallerId = Context.ConnectionId, ClientId = clientId };
                call1.game.challengeAccepted(match.Item1);
                RemoteExecution.ExecuteOnClient(call1);

                //Caller.goToGame(match.Item1);
                //Tell the caller to go to the match
                dynamic call2 = new ClientCall { CallerId = Context.ConnectionId, ClientId = Context.ConnectionId };
                call2.game.goToGame(match.Item1); 
                return new TwoWayResult(call2);
            }
            else
            {
                //Caller.otherPlayerNotOnline();
                dynamic call = new ClientCall { CallerId = Context.ConnectionId, ClientId = Context.ConnectionId };
                call.game.otherPlayerNotOnline();
                return call;
            }

        }

        [HttpPost]
        public TwoWayResult RejectChallenge(Guid challengeId)
        {
            var data = factory.GetGame().RejectChallenge(challengeId);
            if (!string.IsNullOrWhiteSpace(data.Item1))
            {
                //the challenge was not accepted
                dynamic call = new ClientCall { CallerId = Context.ConnectionId, ClientId = data.Item1 };
                call.game.challengeRejected(data.Item2);
                ///RemoteExecution.ExecuteOnClient(call);
                return new TwoWayResult(call);
                //Clients[data.Item1].challengeRejected(data.Item2);
            }

            return null;
        }

        /// <summary>
        /// Player is ready to start match
        /// </summary>
        [HttpPost]
        public void PlayerReady(Guid matchId, Guid playerId)
        {
            var m = factory.GetGame(matchId, true);
            m.PlayerReady(playerId);
            string clientId1 = factory.GetPlayerPresence().GetClientId(playerId);
            string clientId2 = factory.GetPlayerPresence().GetClientId(playerId == m.Player1 ? m.Player2 : m.Player1);
            //start the match if both are ready
            if (m.Player1Ready && m.Player2Ready)
            {
                dynamic call = new ClientCall { CallerId = Context.ConnectionId, ClientId = clientId1 };
                call.game.startMatch();
                RemoteExecution.ExecuteOnClient(call);

                dynamic call2 = new ClientCall { CallerId = Context.ConnectionId, ClientId = clientId2 };
                call2.game.startMatch();
                RemoteExecution.ExecuteOnClient(call2);

                //Clients[string.Format("match_", matchId)].startMatch();
            }
            else
            {
                //inform the other player that the player is ready
                if (RemoteExecution.ClientIsOnLine(clientId2))
                {
                    dynamic call = new ClientCall { CallerId = Context.ConnectionId, ClientId = clientId2 };
                    //Clients[clientId].otherPlayerIsReady();
                    call.game.otherPlayerIsReady();
                    RemoteExecution.ExecuteOnClient(call, false);
                }
                else //TODO:tell the client that the other player is not online
                { 
                    dynamic call = new ClientCall { CallerId = Context.ConnectionId, ClientId = clientId1};
                    call.game.otherPlayerNotOnline();
                    RemoteExecution.ExecuteOnClient(call);
                }
            }
        }


        /// <summary>
        /// This method is called when the player enters the match
        /// </summary>
        /// <param name="matchId"></param>
        [HttpPost]
        public TwoWayResult PlayMatch(Guid matchId)
        {
            var m = factory.GetGame(matchId, true);
            m.RestartMatch();
            //send the match info
            string player1ClientId = factory.GetPlayerPresence().GetClientId(m.Player1);
            string player2ClientId = factory.GetPlayerPresence().GetClientId(m.Player2);
            //verify the client id 
            if (Context.ConnectionId != player1ClientId && Context.ConnectionId != player2ClientId)
            {
                //cannot determine the ID of the caller
                dynamic call = new ClientCall { CallerId = Context.ConnectionId, ClientId = Context.ConnectionId };
                call.game.identityError();
                //Caller.identityError();
                return new TwoWayResult(call);
            }
            //create a group for this match
            //base.GroupManager.AddToGroup(player1ClientId, string.Format("match_", id));
            //base.GroupManager.AddToGroup(player2ClientId, string.Format("match_", id));

            string player1Name = factory.GetPlayerPresence().GetPlayerNameByClientId(player1ClientId);
            string player2Name = factory.GetPlayerPresence().GetPlayerNameByClientId(player2ClientId);
            var mi = new MatchInfo
            {
                MatchId = m.MatchId,
                Player1 = m.Player1,
                Player2 = m.Player2,
                Turn = m.Turn,
                Player1ClientId = player1ClientId,
                Player2ClientId = player2ClientId,
                OponentClientId = player1ClientId == Context.ConnectionId ? player2ClientId : player1ClientId,
                PlayerId = player1ClientId == Context.ConnectionId ? m.Player1 : m.Player2,
                MyPlayerName = player1ClientId == Context.ConnectionId ? player1Name : player2Name,
                OtherPlayerName = player1ClientId == Context.ConnectionId ? player2Name : player1Name
            };
            Guid iam = player1ClientId == Context.ConnectionId ? m.Player1 : m.Player2;
            mi.Units = m.Units.Where(u => u.PlayerId == iam).Select(u =>
                        new UnitInfo
                        {
                            UnitId = u.UnitId,
                            PlayerId = u.PlayerId,
                            Name = u.Name,
                            MaxHealth = u.MaxHealth,
                            Health = u.Health,
                            Col = u.Column,
                            Row = u.Row
                        }
                        ).ToArray();
            //send the match info to the player
            //Caller.matchReady(mi);
            dynamic call2 = new ClientCall { CallerId = Context.ConnectionId, ClientId = Context.ConnectionId };
            call2.game.matchReady(mi);
            return new TwoWayResult(call2);
        }


        [HttpPost]
        public  TwoWayResult InvitePlayer(Guid matchId, string clientId)
        {
            var player = factory.GetPlayerPresence().GetPlayerNameByClientId(Context.ConnectionId);
            
            dynamic call = new ClientCall { CallerId = Context.ConnectionId, ClientId = clientId };
            call.game.inviteToMatch(matchId, player);
            return new TwoWayResult(call);

            //Clients[clientId].inviteToMatch(matchId, player);
        }

        /// <summary>
        /// Player is setting up the game
        /// </summary>
        [HttpPost]
        public void PlayerMoveTo(Guid matchId, string unitId, int row, int col)
        {
            var m = factory.GetGame(matchId, true);
            m.PlayerMoveTo(new Position { Row = row, Column = col }, Guid.Parse(unitId));
        }

        /// <summary>
        /// Launch an attack to the other player
        /// </summary>
        [HttpPost]
        public void PlayerAttack(Guid matchId, Guid playerId, int row, int col)
        {
            var m = factory.GetGame(matchId, true);
            string clientId = factory.GetPlayerPresence().GetClientId(m.Player1 == playerId ? m.Player2 : m.Player1);
            var result = m.PlayerAttack(new Position { Row = row, Column = col }, playerId);

            if (result.Command == Command.GameAction && result.Action == GameAction.ShotMade)
            {
                dynamic call = new ClientCall { CallerId = Context.ConnectionId, ClientId = Context.ConnectionId };
                call.game.sucessfulAttack(row, col);
                RemoteExecution.ExecuteOnClient(call);

                dynamic call2 = new ClientCall { CallerId = Context.ConnectionId, ClientId = clientId };
                call2.game.takeDamage(new Damage { UnitId = result.UnitId, HealthAfterAttack = result.HealthAfterAttack });
                RemoteExecution.ExecuteOnClient(call2);

                //Caller.sucessfulAttack(row, col);
                //Clients[clientId].takeDamage(new Damage { UnitId = result.UnitId, HealthAfterAttack = result.HealthAfterAttack });
            }
            else if (result.Command == Command.GameAction && result.Action == GameAction.ShotMissed)
            {
                dynamic call = new ClientCall { CallerId = Context.ConnectionId, ClientId = Context.ConnectionId };
                call.game.attackMissed(row, col);
                RemoteExecution.ExecuteOnClient(call);

                dynamic call2 = new ClientCall { CallerId = Context.ConnectionId, ClientId = clientId };
                call2.game.shotMissed();
                RemoteExecution.ExecuteOnClient(call2);

                //Caller.attackMissed(row, col);
                //Clients[clientId].shotMissed();
            }
            else if (result.Command == Command.EndGame)
            {
                dynamic call = new ClientCall { CallerId = Context.ConnectionId, ClientId = Context.ConnectionId };
                call.game.endMatch(result.Winner);
                RemoteExecution.ExecuteOnClient(call);

                dynamic call2 = new ClientCall { CallerId = Context.ConnectionId, ClientId = clientId };
                call2.game.endMatch(result.Winner);
                RemoteExecution.ExecuteOnClient(call2);

                //Clients[string.Format("match_", matchId)].endMatch(result.Winner);
            }
        }

        /// <summary>
        /// chat
        /// </summary>
        [HttpPost]
        public TwoWayResult PlayerChat(string message, string clientId)
        {
            //sanitize the message
            message = Encoder.HtmlEncode(message);
            dynamic call = new ClientCall { CallerId = Context.ConnectionId, ClientId = clientId };
            call.game.receiveMessage(message);
            return new TwoWayResult(call);

            //Clients[clientId].receiveMessage(message);
        }


        

        #endregion

    }
}
