using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClourWars.Web.Models
{
    public class MyChallenge
    {


        public Guid ChallengeId { get; set; }
        public Guid FromPlayer { get; set; }
        public Guid ToPlayer { get; set; }
        public string FromPlayerName { get; set; }
        public string ToPlayerName { get; set; }
        public string FromPlayerAvatar { get; set; }
        public string ToPlayerAvatar { get; set; }
        public int FromPlayerWins { get; set; }
        public int ToPlayerWins { get; set; }
        public int FromPlayerLosses { get; set; }
        public int ToPlayerLosses { get; set; }
    }
}