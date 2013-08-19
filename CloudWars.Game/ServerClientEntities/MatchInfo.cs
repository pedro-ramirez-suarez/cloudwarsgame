using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClourWars.Web.ServerClientEntities
{
    public class MatchInfo
    {
        public Guid MatchId { get; set; }
        public Guid Player1 { get; set; }
        public Guid Player2 { get; set; }
        public Guid PlayerId { get; set; }
        public Guid Turn { get; set; }
        public string Player1ClientId { get; set; }
        public string Player2ClientId { get; set; }
        public string OtherPlayerName { get; set; }
        public string MyPlayerName { get; set; }
        public string OponentClientId { get; set; }
        public UnitInfo[] Units { get; set; }

    }
}