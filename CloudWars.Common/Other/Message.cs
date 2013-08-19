using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudWars.Common.Other;

namespace CloudWars.Common.Other
{

    [Serializable]
    public class Message
    {

        public Command Command { get; set; }
        public Guid PlayerId { get; set; }
        public Guid Player1 { get; set; }
        public Guid Player2 { get; set; }
        public Guid MatchId { get; set; }
        public Guid UnitId { get; set; }
        public Guid Winner { get; set; }
        public Guid Losser { get; set; }
        public int HealthAfterAttack { get; set; }
        public GameAction Action { get; set; }
        public Position Coordinates { get; set; }

    }
}
