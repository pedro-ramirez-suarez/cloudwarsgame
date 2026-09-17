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
    /// Database-backed characterization tests for SpaceBattleGame.MatchFinished.
    /// Asserts what the code does TODAY against the real CloudWars database.
    /// </summary>
    public class SpaceBattleGameMatchFinishedDbTests : IDisposable
    {
        private Guid _player1Id;
        private Guid _player2Id;
        private Guid _matchId;

        public void Dispose()
        {
            if (_matchId != Guid.Empty)
            {
                try { CloudWarsData.DeleteMatch(_matchId); }
                catch { /* match may already be deleted by MatchFinished */ }
            }
            if (_player1Id != Guid.Empty)
            {
                try { CloudWarsDB.Players.Delete(new { Id = _player1Id }); }
                catch { }
            }
            if (_player2Id != Guid.Empty)
            {
                try { CloudWarsDB.Players.Delete(new { Id = _player2Id }); }
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
        public void MatchFinished_ReturnsMessage_WithEndGame_Winner_Losser_MatchId()
        {
            var p1 = CreatePlayer("Winner", "client-winner");
            var p2 = CreatePlayer("Loser", "client-loser");
            _player1Id = p1.Id;
            _player2Id = p2.Id;

            var game = new SpaceBattleGame();
            game.CreateMatch(p1.Id, p2.Id);
            _matchId = game.MatchId;

            var result = game.MatchFinished(winner: p1.Id, losser: p2.Id);

            Assert.NotNull(result);
            Assert.Equal(Command.EndGame, result.Command);
            Assert.Equal(p1.Id, result.Winner);
            Assert.Equal(p2.Id, result.Losser);
            Assert.Equal(_matchId, result.MatchId);
        }

        [Fact]
        public void MatchFinished_IncrementsWinnerWinsByOne_InPlayerTable()
        {
            var p1 = CreatePlayer("Winner", "client-winner");
            var p2 = CreatePlayer("Loser", "client-loser");
            _player1Id = p1.Id;
            _player2Id = p2.Id;

            var game = new SpaceBattleGame();
            game.CreateMatch(p1.Id, p2.Id);
            _matchId = game.MatchId;

            game.MatchFinished(winner: p1.Id, losser: p2.Id);

            var winnerAfter = CloudWarsData.GetPlayer(p1.Id);
            Assert.Equal(1, winnerAfter.Wins);
        }

        [Fact]
        public void MatchFinished_IncrementsLoserLossesByOne_InPlayerTable()
        {
            var p1 = CreatePlayer("Winner", "client-winner");
            var p2 = CreatePlayer("Loser", "client-loser");
            _player1Id = p1.Id;
            _player2Id = p2.Id;

            var game = new SpaceBattleGame();
            game.CreateMatch(p1.Id, p2.Id);
            _matchId = game.MatchId;

            game.MatchFinished(winner: p1.Id, losser: p2.Id);

            var loserAfter = CloudWarsData.GetPlayer(p2.Id);
            Assert.Equal(1, loserAfter.Losses);
        }

        [Fact]
        public void MatchFinished_DeletesMatchRow()
        {
            var p1 = CreatePlayer("Winner", "client-winner");
            var p2 = CreatePlayer("Loser", "client-loser");
            _player1Id = p1.Id;
            _player2Id = p2.Id;

            var game = new SpaceBattleGame();
            game.CreateMatch(p1.Id, p2.Id);
            _matchId = game.MatchId;

            // Verify match exists before
            var before = CloudWarsData.GetMatch(_matchId);
            Assert.NotNull(before);

            game.MatchFinished(winner: p1.Id, losser: p2.Id);

            // Match row should be deleted
            var after = CloudWarsData.GetMatch(_matchId);
            Assert.Null(after);
        }

        [Fact]
        public void MatchFinished_DeletesMatchUnits()
        {
            var p1 = CreatePlayer("Winner", "client-winner");
            var p2 = CreatePlayer("Loser", "client-loser");
            _player1Id = p1.Id;
            _player2Id = p2.Id;

            var game = new SpaceBattleGame();
            game.CreateMatch(p1.Id, p2.Id);
            _matchId = game.MatchId;

            // Verify units exist before
            var unitsBefore = CloudWarsData.GetUnits(_matchId).ToList();
            Assert.NotEmpty(unitsBefore);

            game.MatchFinished(winner: p1.Id, losser: p2.Id);

            // Units should be deleted
            var unitsAfter = CloudWarsData.GetUnits(_matchId).ToList();
            Assert.Empty(unitsAfter);
        }
    }
}
