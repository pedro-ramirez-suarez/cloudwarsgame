using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudWars.Common.Other;

namespace CloudWars.Common
{
    public interface IClientFeedback
    {
        void PlayerWon(Guid playerId, Guid matchId);
        void PlayerLost(Guid playerId, Guid matchId);
        void ChallengePlayer(Guid fromPlayer, Guid toPlayer);
        void ChallengeAccepted(Guid matchId,Guid player1, Guid player2);
        void StartMatch(Guid matchId, Guid player1, Guid player2);
        void ShotMade(Guid matchId, Guid player1, Guid player2, int healthAfterAttack, Guid unitId, Position coordinates);
        void ShotMissed(Guid matchId, Guid player1, Guid player2, Position coordinates);


    }
}

