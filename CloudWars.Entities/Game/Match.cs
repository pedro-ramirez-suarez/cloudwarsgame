using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Needletail.DataAccess.Attributes;

namespace CloudWars.Entities.Game
{
    public class Match
    {
        [TableKeyAttribute(CanInsertKey = true)]
        public Guid Id { get; set; }
        public Guid Player1 { get; set; }
        public Guid Player2 { get; set; }
        public Guid Turn { get; set; }
        public bool Player1Ready { get; set; }
        public bool Player2Ready { get; set; }
        public bool Initialized { get; set; }
        public bool MatchEnded { get; set; }
        public Guid Winner { get; set; }
        public bool PlayingNow { get; set; }
    }
}
