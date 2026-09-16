using System;
using System.Collections.Generic;
using System.Linq;
using CloudWars.Common;
using CloudWars.Common.Other;
using CloudWars.Common.Units;
using CloudWars.DataAccess.Sql;
using CloudWars.Entities.Game;
using CloudWars.SpaceBattle;
using Xunit;

namespace CloudWars.Characterization.Db
{
    /// <summary>
    /// Database-backed characterization tests for SpaceBattleGame.
    /// These tests assert what the code does TODAY against the real database.
    /// Private properties (Initialized, PlayingNow) are asserted indirectly
    /// through observable behaviour or by reading the persisted Match row.
    /// </summary>
    public class SpaceBattleGameDbTests : IDisposable
    {
        private Guid _matchId;

        public void Dispose()
        {
            if (_matchId != Guid.Empty)
            {
                try { CloudWarsData.DeleteMatch(_matchId); }
                catch { /* match may already be deleted by MatchFinished */ }
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // Behaviour 1: CreateMatch sets Player1, Player2, Turn and stores Match
        // ─────────────────────────────────────────────────────────────────────

        [Fact]
        public void CreateMatch_SetsPlayersAndTurn_StoresMatchRow_WithMatchingId()
        {
            var player1 = Guid.NewGuid();
            var player2 = Guid.NewGuid();
            var game = new SpaceBattleGame();

            game.CreateMatch(player1, player2);

            _matchId = game.MatchId;

            Assert.Equal(player1, game.Player1);
            Assert.Equal(player2, game.Player2);
            Assert.Equal(player1, game.Turn);
            Assert.NotEqual(Guid.Empty, game.MatchId);

            // Verify the persisted Match row
            var dbMatch = CloudWarsData.GetMatch(game.MatchId);
            Assert.NotNull(dbMatch);
            Assert.Equal(game.MatchId, dbMatch.Id);
            Assert.Equal(player1, dbMatch.Player1);
            Assert.Equal(player2, dbMatch.Player2);
            Assert.Equal(player1, dbMatch.Turn);
        }

        // ─────────────────────────────────────────────────────────────────────
        // Behaviour 2: PlayerReady flags and Initialized (indirect via DB)
        // ─────────────────────────────────────────────────────────────────────

        [Fact]
        public void PlayerReady_SinglePlayer_InitializedFalse_IndirectViaDb()
        {
            var player1 = Guid.NewGuid();
            var player2 = Guid.NewGuid();
            var game = new SpaceBattleGame();
            game.CreateMatch(player1, player2);
            _matchId = game.MatchId;

            game.PlayerReady(player1);

            var dbMatch = CloudWarsData.GetMatch(_matchId);
            Assert.True(dbMatch.Player1Ready);
            Assert.False(dbMatch.Player2Ready);
            // Initialized is private; assert via the persisted column
            Assert.False(dbMatch.Initialized);
        }

        [Fact]
        public void PlayerReady_BothPlayers_InitializedTrue_IndirectViaDb()
        {
            var player1 = Guid.NewGuid();
            var player2 = Guid.NewGuid();
            var game = new SpaceBattleGame();
            game.CreateMatch(player1, player2);
            _matchId = game.MatchId;

            game.PlayerReady(player1);
            game.PlayerReady(player2);

            var dbMatch = CloudWarsData.GetMatch(_matchId);
            Assert.True(dbMatch.Player1Ready);
            Assert.True(dbMatch.Player2Ready);
            Assert.True(dbMatch.Initialized);
        }

        [Fact]
        public void PlayerReady_BothPlayers_ReloadReflectsPersistedReadyFlags()
        {
            var player1 = Guid.NewGuid();
            var player2 = Guid.NewGuid();
            var game = new SpaceBattleGame();
            game.CreateMatch(player1, player2);
            _matchId = game.MatchId;

            game.PlayerReady(player1);
            game.PlayerReady(player2);

            var reloaded = new SpaceBattleGame(_matchId, true);
            Assert.True(reloaded.Player1Ready);
            Assert.True(reloaded.Player2Ready);
            Assert.Equal(player1, reloaded.Player1);
            Assert.Equal(player2, reloaded.Player2);
            Assert.Equal(player1, reloaded.Turn);
        }

        // ─────────────────────────────────────────────────────────────────────
        // Behaviour 3: PlayerAttack before RestartMatch returns NotYourTurn
        // (indirect: PlayingNow is false)
        // ─────────────────────────────────────────────────────────────────────

        [Fact]
        public void PlayerAttack_BeforeRestart_ReturnsNotYourTurn_IndirectPlayingNowFalse()
        {
            var player1 = Guid.NewGuid();
            var player2 = Guid.NewGuid();
            var game = new SpaceBattleGame();
            game.CreateMatch(player1, player2);
            _matchId = game.MatchId;

            game.PlayerReady(player1);
            game.PlayerReady(player2);

            // PlayingNow is still false (never set by CreateMatch or PlayerReady)
            var result = game.PlayerAttack(new Position { Row = 0, Column = 0 }, player1);

            Assert.Equal(Command.NotYourTurn, result.Command);
            Assert.Equal(GameAction.ShotMissed, result.Action);
            Assert.Equal(_matchId, result.MatchId);
            Assert.Equal(player1, result.Player1);
            Assert.Equal(player2, result.Player2);
        }

        // ─────────────────────────────────────────────────────────────────────
        // Behaviour 4: RestartMatch persists PlayingNow=true; reloaded game
        // reads it; original in-memory instance still returns NotYourTurn
        // ─────────────────────────────────────────────────────────────────────

        [Fact]
        public void RestartMatch_PersistsPlayingNowTrue_ReloadReadsTrue_OriginalStillNotYourTurn()
        {
            var player1 = Guid.NewGuid();
            var player2 = Guid.NewGuid();
            var game = new SpaceBattleGame();
            game.CreateMatch(player1, player2);
            _matchId = game.MatchId;

            game.PlayerReady(player1);
            game.PlayerReady(player2);
            game.RestartMatch();

            // DB now has PlayingNow = true
            var dbMatch = CloudWarsData.GetMatch(_matchId);
            Assert.True(dbMatch.PlayingNow);

            // Reloaded game reads PlayingNow = true from DB
            var reloaded = new SpaceBattleGame(_matchId, true);
            // Indirect: if PlayingNow were false, PlayerAttack would return NotYourTurn
            var reloadedResult = reloaded.PlayerAttack(new Position { Row = 5, Column = 5 }, player1);
            Assert.NotEqual(Command.NotYourTurn, reloadedResult.Command);
            Assert.Equal(Command.GameAction, reloadedResult.Command);

            // Original in-memory instance still has PlayingNow = false
            var originalResult = game.PlayerAttack(new Position { Row = 5, Column = 5 }, player1);
            Assert.Equal(Command.NotYourTurn, originalResult.Command);
            Assert.Equal(GameAction.ShotMissed, originalResult.Action);
        }

        // ─────────────────────────────────────────────────────────────────────
        // Behaviour 5: CreateMatch stores one MatchUnit per vanilla unit
        // ─────────────────────────────────────────────────────────────────────

        [Fact]
        public void CreateMatch_StoresOneMatchUnitPerVanillaUnit_HealthEqualsMaxHealth_RowColZero()
        {
            var player1 = Guid.NewGuid();
            var player2 = Guid.NewGuid();
            var game = new SpaceBattleGame();
            game.CreateMatch(player1, player2);
            _matchId = game.MatchId;

            var matchUnits = CloudWarsData.GetUnits(_matchId).ToList();
            var vanillaUnits = CloudWarsDB.VanillaUnits.ToList();

            // One MatchUnit per vanilla unit
            Assert.Equal(vanillaUnits.Count, matchUnits.Count);

            // Every unit starts at Row 0, Col 0 with Health == MaxHealth
            foreach (var mu in matchUnits)
            {
                Assert.Equal(0, mu.Row);
                Assert.Equal(0, mu.Col);
                Assert.Equal(mu.MaxHealth, mu.Health);
                Assert.Equal(_matchId, mu.MatchId);

                // Find the corresponding vanilla unit
                var vu = vanillaUnits.FirstOrDefault(v => v.Id == mu.UnitId);
                Assert.NotNull(vu);
                Assert.Equal(vu.MaxHealth, mu.MaxHealth);
                Assert.Equal(vu.Name, mu.Name);

                // Rebel units belong to player1, empire units to player2
                if (vu.IsRebel)
                    Assert.Equal(player1, mu.PlayerId);
                else
                    Assert.Equal(player2, mu.PlayerId);
            }

            // Verify counts per side
            var rebelCount = vanillaUnits.Count(v => v.IsRebel);
            var empireCount = vanillaUnits.Count(v => !v.IsRebel);
            Assert.Equal(rebelCount, matchUnits.Count(mu => mu.PlayerId == player1));
            Assert.Equal(empireCount, matchUnits.Count(mu => mu.PlayerId == player2));
        }

        // ─────────────────────────────────────────────────────────────────────
        // Behaviour 6: PlayerAttack at empty position → ShotMissed, GameAction,
        // Turn switches to player2, Turn persisted
        // ─────────────────────────────────────────────────────────────────────

        [Fact]
        public void PlayerAttack_EmptyPosition_ShotMissed_GameAction_TurnSwitchesToPlayer2_Persisted()
        {
            var player1 = Guid.NewGuid();
            var player2 = Guid.NewGuid();
            var game = new SpaceBattleGame();
            game.CreateMatch(player1, player2);
            _matchId = game.MatchId;

            game.PlayerReady(player1);
            game.PlayerReady(player2);
            game.RestartMatch();

            // Reload so PlayingNow is true
            var reloaded = new SpaceBattleGame(_matchId, true);

            // Attack at a position where no unit stands (all units are at 0,0)
            var result = reloaded.PlayerAttack(new Position { Row = 3, Column = 3 }, player1);

            Assert.Equal(GameAction.ShotMissed, result.Action);
            Assert.Equal(Command.GameAction, result.Command);
            Assert.Equal(_matchId, result.MatchId);

            // Turn switched to player2 in-memory
            Assert.Equal(player2, reloaded.Turn);

            // Turn persisted to DB
            var dbMatch = CloudWarsData.GetMatch(_matchId);
            Assert.Equal(player2, dbMatch.Turn);
        }

        // ─────────────────────────────────────────────────────────────────────
        // Behaviour 7: PlayerAttack at Row 0 Col 0 → ShotMade, GameAction,
        // HealthAfterAttack = MaxHealth - 1
        // ─────────────────────────────────────────────────────────────────────

        [Fact]
        public void PlayerAttack_Row0Col0_ShotMade_GameAction_HealthAfterAttackIsMaxHealthMinusOne()
        {
            var player1 = Guid.NewGuid();
            var player2 = Guid.NewGuid();
            var game = new SpaceBattleGame();
            game.CreateMatch(player1, player2);
            _matchId = game.MatchId;

            game.PlayerReady(player1);
            game.PlayerReady(player2);
            game.RestartMatch();

            // Reload so PlayingNow is true and Units are populated
            var reloaded = new SpaceBattleGame(_matchId, true);

            // The code hits the FIRST enemy unit (player2) at Row 0, Col 0
            var targetUnit = reloaded.Units
                .FirstOrDefault(u => u.Row == 0 && u.Column == 0 && u.PlayerId == player2);
            Assert.NotNull(targetUnit);

            var result = reloaded.PlayerAttack(new Position { Row = 0, Column = 0 }, player1);

            Assert.Equal(GameAction.ShotMade, result.Action);
            Assert.Equal(Command.GameAction, result.Command);
            Assert.Equal(_matchId, result.MatchId);
            Assert.Equal(targetUnit.UnitId, result.UnitId);

            // TakeDamage(1) does Health-- regardless of the damage parameter,
            // so HealthAfterAttack = MaxHealth - 1
            Assert.Equal(targetUnit.MaxHealth - 1, result.HealthAfterAttack);

            // NOTE: Turn does NOT switch on a hit (only on a miss).
            // This is the current behaviour; pinning it here.
            Assert.Equal(player1, reloaded.Turn);
        }
    }
}
