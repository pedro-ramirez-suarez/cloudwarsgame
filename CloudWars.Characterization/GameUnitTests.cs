using System;
using CloudWars.Entities.Game;
using CloudWars.SpaceBattle.Units;
using Xunit;

namespace CloudWars.Characterization
{
    public class GameUnitTests
    {
        private static MatchUnit CreateMatchUnit()
        {
            return new MatchUnit
            {
                Id = Guid.NewGuid(),
                UnitId = Guid.NewGuid(),
                PlayerId = Guid.NewGuid(),
                MatchId = Guid.NewGuid(),
                Name = "TestUnit",
                MaxHealth = 10,
                Health = 7,
                Row = 3,
                Col = 5
            };
        }

        [Fact]
        public void Constructor_CopiesPublicFieldsFromMatchUnit_ColMapsToColumn()
        {
            var mu = CreateMatchUnit();
            var unit = new GameUnit(mu);

            Assert.Equal(mu.UnitId, unit.UnitId);
            Assert.Equal(mu.PlayerId, unit.PlayerId);
            Assert.Equal(mu.MatchId, unit.MatchId);
            Assert.Equal(mu.Name, unit.Name);
            Assert.Equal(mu.MaxHealth, unit.MaxHealth);
            Assert.Equal(mu.Health, unit.Health);
            Assert.Equal(mu.Row, unit.Row);
            Assert.Equal(mu.Col, unit.Column);
        }

        [Fact]
        public void Constructor_CopiesPrivateId_VerifiedViaToEntity()
        {
            var mu = CreateMatchUnit();
            var unit = new GameUnit(mu);

            // Id is private on GameUnit; verify through ToEntity
            var entity = unit.ToEntity();
            Assert.Equal(mu.Id, entity.Id);
        }

        [Fact]
        public void TakeDamage_Zero_SubtractsExactlyOneFromHealth()
        {
            var mu = CreateMatchUnit();
            mu.Health = 5;
            var unit = new GameUnit(mu);

            unit.TakeDamage(0);

            Assert.Equal(4, unit.Health);
        }

        [Fact]
        public void TakeDamage_Five_SubtractsExactlyOneFromHealth()
        {
            var mu = CreateMatchUnit();
            mu.Health = 5;
            var unit = new GameUnit(mu);

            unit.TakeDamage(5);

            Assert.Equal(4, unit.Health);
        }

        [Fact]
        public void MoveTo_SetsRowAndColumn()
        {
            var mu = CreateMatchUnit();
            var unit = new GameUnit(mu);

            unit.MoveTo(7, 9);

            Assert.Equal(7, unit.Row);
            Assert.Equal(9, unit.Column);
        }

        [Fact]
        public void ToEntity_ReturnsMatchUnit_WithCurrentValues_IncludingOriginalId()
        {
            var mu = CreateMatchUnit();
            var unit = new GameUnit(mu);

            // Mutate some values to confirm ToEntity reflects current state
            unit.Health = 3;
            unit.Row = 1;
            unit.Column = 2;

            var entity = unit.ToEntity();

            Assert.Equal(mu.Id, entity.Id);
            Assert.Equal(unit.PlayerId, entity.PlayerId);
            Assert.Equal(unit.MatchId, entity.MatchId);
            Assert.Equal(unit.UnitId, entity.UnitId);
            Assert.Equal(unit.Name, entity.Name);
            Assert.Equal(unit.MaxHealth, entity.MaxHealth);
            Assert.Equal(3, entity.Health);
            Assert.Equal(1, entity.Row);
            Assert.Equal(2, entity.Col);
        }
    }
}
