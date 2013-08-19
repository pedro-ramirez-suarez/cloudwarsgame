using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudWars.Common;
using CloudWars.Common.Other;
using CloudWars.Common.Units;
using CloudWars.SpaceBattle.Units;
using CloudWars.DataAccess.Sql;
using CloudWars.Entities.Game;

namespace CloudWars.SpaceBattle
{
	public class SpaceBattleGame: IGame
	{
        public List<IGameUnit> Units { get; private set; }
        public Guid MatchId { get; set; }
        public Guid Player1 { get; set; }
        public Guid Player2 { get; set; }
        public Guid Turn { get; set; }
        public bool Player1Ready { get; set; }
        public bool Player2Ready { get; set; }
        bool Initialized { get; set; }
        bool MatchEnded { get; set; }
        bool PlayingNow { get; set; }
        Guid Winner { get; set; }
        

        public SpaceBattleGame(Guid matchId, bool loadMatchData)
        {
            //load the match
            if (loadMatchData)
            {
                var m = CloudWarsData.GetMatch(matchId);
                this.MatchId = m.Id;
                this.Player1 = m.Player1;
                this.Player2 = m.Player2;
                this.Player1Ready = m.Player1Ready;
                this.Player2Ready = m.Player2Ready;
                this.Turn = m.Turn;
                this.Initialized = m.Initialized;
                this.MatchEnded = m.MatchEnded;
                this.Winner = m.Winner;
                this.PlayingNow = m.PlayingNow;
                //Get the units
                var dbUnits = CloudWarsData.GetUnits(matchId);
                Units = new List<IGameUnit>();
                foreach (var u in dbUnits)
                {
                    Units.Add(new GameUnit(u));
                }
            }
            else
            {
                this.MatchId = matchId;
            }
        }

        

        public SpaceBattleGame()
        { 
        }

        /// <summary>
        /// Create the match and 
        /// </summary>
        public void CreateMatch(Guid player1, Guid player2)
        {
            Player1 = player1;
            Player2 = player2;
            //Create the match and set the matchid
            MatchId = CloudWarsData.CreateMatch(player1, player2);
            Turn = player1;
        }

        /// <summary>
        /// This is used when the Match is created
        /// </summary>
        /// <param name="initialState"></param>
        public void Initialize(object initialState)
        {
            Initialized = true;
            //save the match
            UpdateMatch();

        }

        public void PlayerReady(Guid playerId)
        {
            if (playerId == Player1)
                Player1Ready = true;
            else
                Player2Ready = true;
            if (Player1Ready && Player2Ready)
                Initialized = true;
            //save the match
            UpdateMatch();
        }

        public void PlayerMoveTo(Position coordinates,Guid unitId)
        {
            //does not allow moves after initialization
            if (Initialized)
                return;
            var unit = Units.FirstOrDefault(u => u.UnitId == unitId);
            if (unit != null)
            {
                UpdateUnit(columns: new { Row = coordinates.Row, Col = coordinates.Column }, where: new { MatchId = unit.MatchId, And_UnitId = unit.UnitId, And_PlayerId = unit.PlayerId } );
            }
        }

        public Message PlayerAttack(Position coordinates, Guid playerId)
        {
            //check if its player turn and that the game is active
            if (Turn != playerId || !this.PlayingNow)
                return new Message { Action = GameAction.ShotMissed, Player1 = Player1, Player2 = Player2, Command = Command.NotYourTurn, MatchId = MatchId };
            
            //determine if its a hit 
            var unit = Units.FirstOrDefault(u => u.Row == coordinates.Row && u.Column == coordinates.Column && u.PlayerId != playerId);
            bool wasHit;
            if (unit != null && unit.Health > 0)
            {
                //update the unit status, and queue a message
                unit.TakeDamage(1);
                UpdateUnit(columns: new { Health = unit.Health }, where: new { MatchId = unit.MatchId, And_UnitId = unit.UnitId, And_PlayerId = unit.PlayerId });
                wasHit = true;
            }
            else
            {
                wasHit = false;
            }
            //queue proper message            
            if (wasHit)
            {
                //check if player won the match
                if (Units.Count(u => u.Health > 0 && u.PlayerId != playerId) == 0)
                {
                    //inform players the match result                    
                    return MatchFinished(winner: playerId, losser: unit.PlayerId);
                }
                else
                {
                    //inform the hit
                    var msg = new Message { Action = GameAction.ShotMade,  Player1 = Player1, Player2 = Player2, Command = Command.GameAction, HealthAfterAttack = unit.Health, MatchId = MatchId, UnitId = unit.UnitId, Coordinates = coordinates };
                    return msg;
                }
            }
            else
            { 
                //inform the missing
                var msg = new Message { Action = GameAction.ShotMissed, Player1 = Player1, Player2 = Player2, Command = Command.GameAction,  MatchId = MatchId, Coordinates = coordinates };
                
                //change the turn
                Turn = Turn == Player1 ? Player2 : Player1;
                UpdateMatch();
                return msg;
            }
            
            
        }

        public void PauseMatch()
        {
            this.PlayingNow = false;
            this.Player1Ready = false;
            this.Player2Ready = false;
            UpdateMatch();
        }


        public void RestartMatch()
        {
            UpdateMatch(new { PlayingNow = true}, new { Id = this.MatchId });
        }



        public Message MatchFinished(Guid winner, Guid losser)
        {
            //update player stats
            CloudWarsData.PlayerWin(winner);
            CloudWarsData.PlayerLose(losser);
            //delete the match
            CloudWarsData.DeleteMatch(this.MatchId);
            //return the message
            return new Message { Winner = winner, Losser = losser, Command = Command.EndGame,  MatchId = this.MatchId };
        }

        private void UpdateUnit(object columns, object where)
        {
            CloudWarsData.UpdateMatchUnit(values: columns, where: where);
        }

        private void UpdateMatch()
        {
            var cols = new { Turn = this.Turn, Player1Ready = this.Player1Ready, Player2Ready = this.Player2Ready, Initialized = this.Initialized, MatchEnded = this.MatchEnded, Winner = this.Winner, PlayingNow = this.PlayingNow };
            var where = new { Id = this.MatchId };
            CloudWarsData.UpdateMatch(values: cols, where: where);
        }

        private void UpdateMatch(object columns, object where)
        {
            CloudWarsData.UpdateMatch(values: columns, where: where);
        }

        private void UpdatePlayer(object columns, object where)
        {
            CloudWarsData.UpdatePlayer(values: columns, where: where);
        }
        

        public void ChallengePlayer(Guid fromId, Guid toId)
        {
            CloudWarsData.ChallengePlayer(fromId, toId);
        }

        public Tuple<Guid, Guid, Guid> AcceptChallenge(Guid challengeId)
        {
            //Get the challenge 
            var c = CloudWarsData.GetChallenge (challengeId );
            //accept the challenge and create the match
            CloudWarsData.AcceptChallenge(challengeId);
            this.CreateMatch(c.Player1, c.Player2);
            return new Tuple<Guid, Guid, Guid>(this.MatchId, c.Player1, c.Player2);
        }


        public Tuple<string, string> RejectChallenge(Guid challengeId)
        {
            var c = CloudWarsData.GetChallenge(challengeId);
            var p1 = CloudWarsData.GetPlayer(c.Player1);
            var p2 = CloudWarsData.GetPlayer(c.Player2);
            CloudWarsData.RejectChallenge(challengeId);
            return new Tuple<string, string>(p1.ClientId, p2.DisplayName);

        }
    }
}
