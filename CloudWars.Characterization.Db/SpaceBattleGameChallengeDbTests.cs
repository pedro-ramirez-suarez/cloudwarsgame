using System;
using System.Linq;
using CloudWars.DataAccess.Sql;
using CloudWars.Entities.Game;
using CloudWars.Entities.Player;
using CloudWars.SpaceBattle;
using Xunit;

namespace CloudWars.Characterization.Db
{
    /// <summary>
    /// Database-backed characterization tests for ChallengePlayer, AcceptChallenge, and RejectChallenge.
    /// Asserts what the code does TODAY against the real CloudWars database.
    /// </summary>
    public class SpaceBattleGameChallengeDbTests : IDisposable
    {
        private Guid _player1Id;
        private Guid _player2Id;
        private Guid _challengeId;
        private Guid _matchId;

        public void Dispose()
        {
            if (_matchId != Guid.Empty)
            {
                try { CloudWarsData.DeleteMatch(_matchId); }
                catch { }
            }
            if (_challengeId != Guid.Empty)
            {
                try { CloudWarsDB.Challenges.Delete(new { Id = _challengeId }); }
                catch { }
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

        // ─────────────────────────────────────────────────────────────────────
        // CloudWarsData.ChallengePlayer
        // ─────────────────────────────────────────────────────────────────────

        [Fact]
        public void CloudWarsData_ChallengePlayer_StoresChallengeRow_WithPlayer1_Player2_AcceptedFalse()
        {
            var p1 = CreatePlayer("Challenger", "client-challenger");
            var p2 = CreatePlayer("Challenged", "client-challenged");
            _player1Id = p1.Id;
            _player2Id = p2.Id;

            var challengeId = CloudWarsData.ChallengePlayer(p1.Id, p2.Id);
            _challengeId = challengeId;

            var challenge = CloudWarsData.GetChallenge(challengeId);
            Assert.NotNull(challenge);
            Assert.Equal(p1.Id, challenge.Player1);
            Assert.Equal(p2.Id, challenge.Player2);
            Assert.False(challenge.Accepted);
        }

        // ─────────────────────────────────────────────────────────────────────
        // SpaceBattleGame.ChallengePlayer
        // ─────────────────────────────────────────────────────────────────────

        [Fact]
        public void SpaceBattleGame_ChallengePlayer_StoresChallengeRow_WithPlayer1_Player2_AcceptedFalse()
        {
            var p1 = CreatePlayer("Challenger", "client-challenger");
            var p2 = CreatePlayer("Challenged", "client-challenged");
            _player1Id = p1.Id;
            _player2Id = p2.Id;

            var game = new SpaceBattleGame();
            game.ChallengePlayer(p1.Id, p2.Id);

            // Read back all challenges for these players to find the one we just created
            var challenges = CloudWarsDB.Challenges.GetMany(new { Player1 = p1.Id, Player2 = p2.Id }).ToList();
            Assert.Single(challenges);
            var challenge = challenges[0];
            _challengeId = challenge.Id;
            Assert.Equal(p1.Id, challenge.Player1);
            Assert.Equal(p2.Id, challenge.Player2);
            Assert.False(challenge.Accepted);
        }

        // ─────────────────────────────────────────────────────────────────────
        // SpaceBattleGame.AcceptChallenge
        // ─────────────────────────────────────────────────────────────────────

        [Fact]
        public void AcceptChallenge_DeletesChallenge_CreatesMatch_TurnIsPlayer1_ReturnsTuple()
        {
            var p1 = CreatePlayer("Challenger", "client-challenger");
            var p2 = CreatePlayer("Challenged", "client-challenged");
            _player1Id = p1.Id;
            _player2Id = p2.Id;

            // Create a challenge
            var challengeId = CloudWarsData.ChallengePlayer(p1.Id, p2.Id);
            _challengeId = challengeId;

            var game = new SpaceBattleGame();
            var result = game.AcceptChallenge(challengeId);

            // Challenge is deleted
            var challengeAfter = CloudWarsData.GetChallenge(challengeId);
            Assert.Null(challengeAfter);

            // Tuple returned: (MatchId, Player1, Player2)
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Item1);
            Assert.Equal(p1.Id, result.Item2);
            Assert.Equal(p2.Id, result.Item3);

            // Match was created with Turn = Player1
            _matchId = result.Item1;
            var match = CloudWarsData.GetMatch(_matchId);
            Assert.NotNull(match);
            Assert.Equal(p1.Id, match.Player1);
            Assert.Equal(p2.Id, match.Player2);
            Assert.Equal(p1.Id, match.Turn);
        }

        // ─────────────────────────────────────────────────────────────────────
        // SpaceBattleGame.RejectChallenge
        // ─────────────────────────────────────────────────────────────────────

        [Fact]
        public void RejectChallenge_DeletesChallenge_ReturnsTupleOfClientIds()
        {
            var p1 = CreatePlayer("Challenger", "client-challenger");
            var p2 = CreatePlayer("Challenged", "client-challenged");
            _player1Id = p1.Id;
            _player2Id = p2.Id;

            // Create a challenge
            var challengeId = CloudWarsData.ChallengePlayer(p1.Id, p2.Id);
            _challengeId = challengeId;

            var game = new SpaceBattleGame();
            var result = game.RejectChallenge(challengeId);

            // Challenge is deleted
            var challengeAfter = CloudWarsData.GetChallenge(challengeId);
            Assert.Null(challengeAfter);

            // Tuple returned: (player1.ClientId, player2.DisplayName)
            Assert.NotNull(result);
            Assert.Equal("client-challenger", result.Item1);
            Assert.Equal("Challenged", result.Item2);
        }
    }
}
