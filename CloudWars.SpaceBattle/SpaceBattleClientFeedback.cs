using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudWars.Common;
using CloudWars.Entities.Game;
using CloudWars.Common.Other;
using CloudWars.DataAccess.Sql;

namespace CloudWars.SpaceBattle
{
    public class SpaceBattleClientFeedback : IClientFeedback
    {
        
        public void PlayerWon(Guid playerId, Guid matchId)
        {
            var n = new PlayerNotification { Id = Guid.NewGuid(), MatchId = matchId, PlayerId = playerId, NotificationType = NotificationTypes.PlayerWon };
            CloudWarsData.AddNotification(n);
        }

        public void PlayerLost(Guid playerId, Guid matchId)
        {
            var n = new PlayerNotification { Id = Guid.NewGuid(), MatchId = matchId, PlayerId = playerId, NotificationType = NotificationTypes.PlayerLost };
            CloudWarsData.AddNotification(n);
        }

        public void ChallengePlayer(Guid fromPlayer, Guid toPlayer)
        {
            var n = new PlayerNotification { Id = Guid.NewGuid(), PlayerId =  toPlayer, OtherPlayer = fromPlayer, NotificationType = NotificationTypes.ChallengePlayer };
            CloudWarsData.AddNotification(n);
        }

        public void ChallengeAccepted(Guid matchId,Guid fromPlayer, Guid toPlayer)
        {
            var n = new PlayerNotification { Id = Guid.NewGuid(), MatchId = matchId, PlayerId = toPlayer, OtherPlayer = fromPlayer, NotificationType = NotificationTypes.ChallengeAccepted };
            CloudWarsData.AddNotification(n);
        }

        public void StartMatch(Guid matchId, Guid player1, Guid player2)
        {
            var n = new PlayerNotification { Id = Guid.NewGuid(), MatchId = matchId, PlayerId = player1, OtherPlayer = player2, NotificationType = NotificationTypes.StartMatch };
            CloudWarsData.AddNotification(n);
        }

        public void ShotMade(Guid matchId, Guid player1, Guid player2, int healthAfterAttack, Guid unitId, Common.Other.Position coordinates)
        {
            var n = new PlayerNotification { Id = Guid.NewGuid(), MatchId = matchId, PlayerId = player1, OtherPlayer = player2, Row = coordinates.Row, Col = coordinates.Column, NotificationType = NotificationTypes.ShotMade };
            CloudWarsData.AddNotification(n);
        }

        public void ShotMissed(Guid matchId, Guid player1, Guid player2, Common.Other.Position coordinates)
        {
            var n = new PlayerNotification { Id = Guid.NewGuid(), MatchId = matchId, PlayerId = player1, OtherPlayer = player2, Row = coordinates.Row, Col = coordinates.Column, NotificationType = NotificationTypes.ShotMissed};
            CloudWarsData.AddNotification(n);
        }
    }
}
