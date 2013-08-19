using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudWars.Common.Units
{
    public interface IGameUnit
    {
        Guid UnitId { get; set; }
        Guid PlayerId { get; set; }
        Guid MatchId { get; set; }
        string Name { get; set; }
        int MaxHealth { get; set; }
        int Health { get; set; }
        int Column { get; set; }
        int Row { get; set; }
        void TakeDamage(int damage);
        void MoveTo(int row, int column);
    }
}
