using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudWars.Common.Units;
using CloudWars.Common.Other;

namespace CloudWars.Common
{
    public interface IGame
    {
        void ChallengePlayer(Guid fromId, Guid toId);
        
        /// <summary>
        /// Accept the challenge 
        /// </summary>
        /// <returns>MatchId, player 1 id, player 2 id</returns>
        Tuple<Guid,Guid,Guid> AcceptChallenge(Guid challengeId);
        Tuple<string, string> RejectChallenge(Guid challengeId);
        void CreateMatch(Guid player1, Guid player2);
        void Initialize(object initialState);
        void PlayerMoveTo(Position coordinates, Guid unitId);
        Message PlayerAttack(Position coordinates, Guid playerId);
        void PlayerReady(Guid playerId);
        Message MatchFinished(Guid winner, Guid losser);
        Guid MatchId { get; set; }
        Guid Player1 { get; set; }
        Guid Player2 { get; set; }
        Guid Turn { get; set; }
        bool Player1Ready { get; set; }
        bool Player2Ready { get; set; }
        void PauseMatch();
        void RestartMatch();
        List<IGameUnit> Units { get; }
    }

}
