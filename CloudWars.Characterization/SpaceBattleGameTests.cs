using System;
using CloudWars.Common;
using CloudWars.Common.Other;
using CloudWars.SpaceBattle;
using Xunit;

namespace CloudWars.Characterization
{
    public class SpaceBattleGameTests
    {
        [Fact]
        public void ParameterlessConstructor_Units_IsNull()
        {
            var game = new SpaceBattleGame();

            Assert.Null(game.Units);
        }

        [Fact]
        public void TwoParamConstructor_LoadFalse_SetsMatchId_LeavesTurnAndReadyFlagsAtDefaults()
        {
            var matchId = Guid.NewGuid();
            var game = new SpaceBattleGame(matchId, false);

            Assert.Equal(matchId, game.MatchId);
            Assert.Equal(Guid.Empty, game.Turn);
            Assert.False(game.Player1Ready);
            Assert.False(game.Player2Ready);
        }

        [Fact]
        public void PlayerAttack_PlayerIdNotCurrentTurn_ReturnsNotYourTurn_ShotMissed_WithoutTouchingUnits()
        {
            var game = new SpaceBattleGame();
            var player1 = Guid.NewGuid();
            var player2 = Guid.NewGuid();
            var attacker = Guid.NewGuid(); // deliberately not the current turn

            game.Player1 = player1;
            game.Player2 = player2;
            game.Turn = player1;

            var unitsBefore = game.Units; // null

            var result = game.PlayerAttack(new Position { Row = 0, Column = 0 }, attacker);

            Assert.Equal(Command.NotYourTurn, result.Command);
            Assert.Equal(GameAction.ShotMissed, result.Action);
            Assert.Equal(game.MatchId, result.MatchId);
            Assert.Equal(player1, result.Player1);
            Assert.Equal(player2, result.Player2);
            Assert.Same(unitsBefore, game.Units);
        }
    }
}
