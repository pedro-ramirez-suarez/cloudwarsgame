using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Needletail.DataAccess.Attributes;

namespace CloudWars.Entities.Game
{
    public class MatchUnit
    {
        [TableKeyAttribute(CanInsertKey = true)]
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid MatchId { get; set; }
        public Guid UnitId { get; set; }
        public string Name { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
    }
}
