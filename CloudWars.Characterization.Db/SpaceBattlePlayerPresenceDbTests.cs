using System;
using System.Linq;
using CloudWars.DataAccess.Sql;
using CloudWars.Entities.Player;
using CloudWars.SpaceBattle;
using Xunit;

namespace CloudWars.Characterization.Db
{
    /// <summary>
    /// Database-backed characterization tests for SpaceBattlePlayerPresence.
    /// Asserts what the code does TODAY against the real CloudWars database.
    /// Does NOT test PlayerIsOnLine (requires SQL geography).
    /// </summary>
    public class SpaceBattlePlayerPresenceDbTests : IDisposable
    {
        private Guid _playerId;
        private Guid _matchId;

        public void Dispose()
        {
            if (_matchId != Guid.Empty)
            {
                try { CloudWarsData.DeleteMatch(_matchId); }
                catch { }
            }
            if (_playerId != Guid.Empty)
            {
                try { CloudWarsDB.Players.Delete(new { Id = _playerId }); }
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

        [Fact]
        public void GetPlayerId_ReturnsPlayerId_ForGivenClientId()
        {
            var p = CreatePlayer("TestPlayer", "client-abc123");
            _playerId = p.Id;

            var presence = new SpaceBattlePlayerPresence();
            var result = presence.GetPlayerId("client-abc123");

            Assert.Equal(p.Id, result);
        }

        [Fact]
        public void GetClientId_ReturnsClientId_ForGivenPlayerId()
        {
            var p = CreatePlayer("TestPlayer", "client-xyz789");
            _playerId = p.Id;

            var presence = new SpaceBattlePlayerPresence();
            var result = presence.GetClientId(p.Id);

            Assert.Equal("client-xyz789", result);
        }

        [Fact]
        public void GetPlayerNameByClientId_ReturnsDisplayName_ForGivenClientId()
        {
            var p = CreatePlayer("DisplayNameHere", "client-name-test");
            _playerId = p.Id;

            var presence = new SpaceBattlePlayerPresence();
            var result = presence.GetPlayerNameByClientId("client-name-test");

            Assert.Equal("DisplayNameHere", result);
        }

        [Fact]
        public void PlayerDisconnected_SetsStatusToOffLine_AndClientIdToEmptyString()
        {
            var p = CreatePlayer("DisconnectMe", "client-disconnect");
            _playerId = p.Id;

            var presence = new SpaceBattlePlayerPresence();
            presence.PlayerDisconnected("client-disconnect");

            var playerAfter = CloudWarsData.GetPlayer(p.Id);
            Assert.Equal(PlayerStatus.OffLine, playerAfter.Status);
            Assert.Equal(string.Empty, playerAfter.ClientId);
        }

        [Fact]
        public void IsPlayingMatch_ReturnsNull_WhenNoMatchWithPlayingNowTrue()
        {
            var p = CreatePlayer("NoMatchPlayer", "client-nomatch");
            _playerId = p.Id;

            var presence = new SpaceBattlePlayerPresence();
            var result = presence.IsPlayingMatch(p.Id);

            Assert.Null(result);
        }

        [Fact]
        public void IsPlayingMatch_ReturnsMatchId_AfterRestartMatch()
        {
            var p1 = CreatePlayer("MatchPlayer1", "client-match1");
            var p2 = CreatePlayer("MatchPlayer2", "client-match2");
            _playerId = p1.Id;

            var game = new SpaceBattleGame();
            game.CreateMatch(p1.Id, p2.Id);
            _matchId = game.MatchId;

            game.PlayerReady(p1.Id);
            game.PlayerReady(p2.Id);
            game.RestartMatch();

            var presence = new SpaceBattlePlayerPresence();
            var result = presence.IsPlayingMatch(p1.Id);

            Assert.NotNull(result);
            Assert.Equal(_matchId, result.Value);

            // Clean up second player
            try { CloudWarsDB.Players.Delete(new { Id = p2.Id }); }
            catch { }
        }
    }
}
