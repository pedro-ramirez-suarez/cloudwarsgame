using System;
using System.Linq;
using CloudWars.Common.Other;
using CloudWars.DataAccess.Sql;
using CloudWars.Entities.Player;
using CloudWars.SpaceBattle;
using Xunit;

namespace CloudWars.Characterization.Db
{
    /// <summary>
    /// Database-backed characterization tests for SpaceBattleClientFeedback.
    /// Asserts what the code does TODAY against the real CloudWars database.
    /// Each feedback method inserts exactly one PlayerNotification row.
    /// </summary>
    public class SpaceBattleClientFeedbackDbTests : IDisposable
    {
        private Guid _playerId;
        private Guid _otherPlayerId;
        private Guid _notificationId;

        public void Dispose()
        {
            if (_notificationId != Guid.Empty)
            {
                try { CloudWarsDB.PlayerNotifications.Delete(new { Id = _notificationId }); }
                catch { }
            }
            if (_playerId != Guid.Empty)
            {
                try { CloudWarsDB.Players.Delete(new { Id = _playerId }); }
                catch { }
            }
            if (_otherPlayerId != Guid.Empty)
            {
                try { CloudWarsDB.Players.Delete(new { Id = _otherPlayerId }); }
                catch { }
            }
        }

        private Player CreatePlayer(string displayName, string clientId)
        {
            var p = new Player
            {
                Id = Guid.NewGuid(),
                DisplayName = displayName,
                LiveId = Guid.NewGuid().ToString(),
                Avatar = "avatar.png",
                Status = PlayerStatus.OnLine,
                Wins = 0,
                Losses = 0,
                LastActivity = DateTime.Now,
                ClientId = clientId,
                Location = null
            };
            CloudWarsDB.Players.Insert(p);
            return p;
        }

        private void CleanupNotifications(Guid playerId)
        {
            var notifications = CloudWarsDB.PlayerNotifications.GetMany(new { PlayerId = playerId }).ToList();
            foreach (var n in notifications)
            {
                CloudWarsDB.PlayerNotifications.Delete(new { Id = n.Id });
            }
        }

        [Fact]
        public void PlayerWon_InsertsOneNotification_WithCorrectType_PlayerId_OtherPlayerIsNull_MatchId()
        {
            var p1 = CreatePlayer("Winner", "client-winner");
            var p2 = CreatePlayer("Loser", "client-loser");
            _playerId = p1.Id;
            _otherPlayerId = p2.Id;

            var matchId = Guid.NewGuid();
            var feedback = new SpaceBattleClientFeedback();
            feedback.PlayerWon(p1.Id, matchId);

            var notifications = CloudWarsDB.PlayerNotifications.GetMany(new { PlayerId = p1.Id }).ToList();
            Assert.Single(notifications);
            _notificationId = notifications[0].Id;

            Assert.Equal(NotificationTypes.PlayerWon, notifications[0].NotificationType);
            Assert.Equal(p1.Id, notifications[0].PlayerId);
            Assert.Null(notifications[0].OtherPlayer);
            Assert.Equal(matchId, notifications[0].MatchId);

            CleanupNotifications(p1.Id);
        }

        [Fact]
        public void PlayerLost_InsertsOneNotification_WithCorrectType_PlayerId_OtherPlayerIsNull_MatchId()
        {
            var p1 = CreatePlayer("Loser", "client-loser");
            var p2 = CreatePlayer("Winner", "client-winner");
            _playerId = p1.Id;
            _otherPlayerId = p2.Id;

            var matchId = Guid.NewGuid();
            var feedback = new SpaceBattleClientFeedback();
            feedback.PlayerLost(p1.Id, matchId);

            var notifications = CloudWarsDB.PlayerNotifications.GetMany(new { PlayerId = p1.Id }).ToList();
            Assert.Single(notifications);
            _notificationId = notifications[0].Id;

            Assert.Equal(NotificationTypes.PlayerLost, notifications[0].NotificationType);
            Assert.Equal(p1.Id, notifications[0].PlayerId);
            Assert.Null(notifications[0].OtherPlayer);
            Assert.Equal(matchId, notifications[0].MatchId);

            CleanupNotifications(p1.Id);
        }

        [Fact]
        public void ChallengePlayer_InsertsOneNotification_ForChallengedPlayer_WithCorrectType_MatchIdIsEmpty()
        {
            var p1 = CreatePlayer("Challenger", "client-challenger");
            var p2 = CreatePlayer("Challenged", "client-challenged");
            _playerId = p1.Id;
            _otherPlayerId = p2.Id;

            var feedback = new SpaceBattleClientFeedback();
            feedback.ChallengePlayer(p1.Id, p2.Id);

            var notifications = CloudWarsDB.PlayerNotifications.GetMany(new { PlayerId = p2.Id }).ToList();
            Assert.Single(notifications);
            _notificationId = notifications[0].Id;

            Assert.Equal(NotificationTypes.ChallengePlayer, notifications[0].NotificationType);
            Assert.Equal(p2.Id, notifications[0].PlayerId);
            Assert.Equal(p1.Id, notifications[0].OtherPlayer);
            Assert.Equal(Guid.Empty, notifications[0].MatchId);

            CleanupNotifications(p1.Id);
        }

        [Fact]
        public void ChallengeAccepted_InsertsOneNotification_ForOtherPlayer_WithCorrectType_MatchId()
        {
            var p1 = CreatePlayer("Player1", "client-p1");
            var p2 = CreatePlayer("Player2", "client-p2");
            _playerId = p1.Id;
            _otherPlayerId = p2.Id;

            var matchId = Guid.NewGuid();
            var feedback = new SpaceBattleClientFeedback();
            feedback.ChallengeAccepted(matchId, p1.Id, p2.Id);

            var notifications = CloudWarsDB.PlayerNotifications.GetMany(new { PlayerId = p2.Id }).ToList();
            Assert.Single(notifications);
            _notificationId = notifications[0].Id;

            Assert.Equal(NotificationTypes.ChallengeAccepted, notifications[0].NotificationType);
            Assert.Equal(p2.Id, notifications[0].PlayerId);
            Assert.Equal(p1.Id, notifications[0].OtherPlayer);
            Assert.Equal(matchId, notifications[0].MatchId);

            CleanupNotifications(p1.Id);
        }

        [Fact]
        public void StartMatch_InsertsOneNotification_WithCorrectType_MatchId()
        {
            var p1 = CreatePlayer("Player1", "client-p1");
            var p2 = CreatePlayer("Player2", "client-p2");
            _playerId = p1.Id;
            _otherPlayerId = p2.Id;

            var matchId = Guid.NewGuid();
            var feedback = new SpaceBattleClientFeedback();
            feedback.StartMatch(matchId, p1.Id, p2.Id);

            var notifications = CloudWarsDB.PlayerNotifications.GetMany(new { PlayerId = p1.Id }).ToList();
            Assert.Single(notifications);
            _notificationId = notifications[0].Id;

            Assert.Equal(NotificationTypes.StartMatch, notifications[0].NotificationType);
            Assert.Equal(p1.Id, notifications[0].PlayerId);
            Assert.Equal(p2.Id, notifications[0].OtherPlayer);
            Assert.Equal(matchId, notifications[0].MatchId);

            CleanupNotifications(p1.Id);
        }

        [Fact]
        public void ShotMade_InsertsOneNotification_WithCorrectType_RowColFromPosition()
        {
            var p1 = CreatePlayer("Shooter", "client-shooter");
            var p2 = CreatePlayer("Target", "client-target");
            _playerId = p1.Id;
            _otherPlayerId = p2.Id;

            var matchId = Guid.NewGuid();
            var unitId = Guid.NewGuid();
            var position = new CloudWars.Common.Other.Position { Row = 3, Column = 7 };
            var feedback = new SpaceBattleClientFeedback();
            feedback.ShotMade(matchId, p1.Id, p2.Id, 5, unitId, position);

            var notifications = CloudWarsDB.PlayerNotifications.GetMany(new { PlayerId = p1.Id }).ToList();
            Assert.Single(notifications);
            _notificationId = notifications[0].Id;

            Assert.Equal(NotificationTypes.ShotMade, notifications[0].NotificationType);
            Assert.Equal(p1.Id, notifications[0].PlayerId);
            Assert.Equal(p2.Id, notifications[0].OtherPlayer);
            Assert.Equal(matchId, notifications[0].MatchId);
            Assert.Equal(3, notifications[0].Row);
            Assert.Equal(7, notifications[0].Col);

            CleanupNotifications(p1.Id);
        }

        [Fact]
        public void ShotMissed_InsertsOneNotification_WithCorrectType_RowColFromPosition()
        {
            var p1 = CreatePlayer("Shooter", "client-shooter");
            var p2 = CreatePlayer("Target", "client-target");
            _playerId = p1.Id;
            _otherPlayerId = p2.Id;

            var matchId = Guid.NewGuid();
            var position = new CloudWars.Common.Other.Position { Row = 5, Column = 2 };
            var feedback = new SpaceBattleClientFeedback();
            feedback.ShotMissed(matchId, p1.Id, p2.Id, position);

            var notifications = CloudWarsDB.PlayerNotifications.GetMany(new { PlayerId = p1.Id }).ToList();
            Assert.Single(notifications);
            _notificationId = notifications[0].Id;

            Assert.Equal(NotificationTypes.ShotMissed, notifications[0].NotificationType);
            Assert.Equal(p1.Id, notifications[0].PlayerId);
            Assert.Equal(p2.Id, notifications[0].OtherPlayer);
            Assert.Equal(matchId, notifications[0].MatchId);
            Assert.Equal(5, notifications[0].Row);
            Assert.Equal(2, notifications[0].Col);

            CleanupNotifications(p1.Id);
        }
    }
}
