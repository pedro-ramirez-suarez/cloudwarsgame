using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudWars.Common.Units;
using CloudWars.Entities.Game;

namespace CloudWars.SpaceBattle.Units
{
    public class GameUnit : IGameUnit
    {

        public GameUnit(MatchUnit matchUnit)
        { 
            Id = matchUnit.Id;
            UnitId = matchUnit.UnitId;
            PlayerId = matchUnit.PlayerId;
            MatchId = matchUnit.MatchId;
            Name = matchUnit.Name;
            MaxHealth = matchUnit.MaxHealth;
            Health = matchUnit.Health;
            Row = matchUnit.Row;
            Column = matchUnit.Col;
        }
        private Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid MatchId { get; set; }
        public Guid UnitId { get; set; }
        public string Name { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public void TakeDamage(int damage)
        {
            Health--;
        }

        public void MoveTo(int row, int column)
        {
            Row = row;
            Column = column;
        }


        public MatchUnit ToEntity()
        {
            return new MatchUnit { 
                        Id = this.Id, 
                        PlayerId = this.PlayerId, 
                        MatchId = this.MatchId, 
                        UnitId = this.UnitId, 
                        Name = this.Name, 
                        MaxHealth = this.MaxHealth, 
                        Health = this.Health, 
                        Row = this.Row, 
                        Col = this.Column 
            };
        }


    }
}
