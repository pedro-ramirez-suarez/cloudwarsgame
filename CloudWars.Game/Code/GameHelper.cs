using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudWars.Entities.Player;
using CloudWars.DataAccess.Sql;
using ClourWars.Web.Models;

namespace ClourWars.Web.Code
{
    public class GameHelper
    {

        public static IEnumerable<Player> GetOnlinePlayers()
        {
            return CloudWarsData.GetOnlinePlayers();
        }

        public static IEnumerable<Player> GetOnlinePlayers(string liveId)
        {
            return CloudWarsData.GetOnlinePlayers(liveId);
        }

        public static IEnumerable<Player> GetNearbyPlayers(string liveId)
        {
            return CloudWarsData.GetOnlinePlayersNearBy(liveId);
        }


        public static IEnumerable<MyChallenge> GetMyChallenges(string liveId)
        {
            return CloudWarsDB.Challenges.JoinGetTyped<MyChallenge>(
                "DISTINCT Challenge.Id as ChallengeId, p1.Id as FromPlayer, p2.Id as ToPlayer, p1.DisplayName as FromPlayerName, p2.DisplayName as ToPlayerName, p1.Avatar as FromPlayerAvatar, p2.Avatar as ToPlayerAvatar ,p1.Wins as FromPlayerWins, p2.Wins as ToPlayerWins, p1.Losses as FromPlayerLosses, p2.Losses as ToPlayerLosses", 
                "Inner Join Player p1 on Challenge.Player1 = p1.Id Inner join Player p2 on Challenge.Player2 = p2.Id", 
                string.Format("p2.LiveId ='{0}'",liveId), 
                "ORDER By p1.Wins DESC", 
                new Dictionary<string, object>());
        }


        public static IEnumerable<MyMatches> GetMyMatches(string liveId)
        {
            return CloudWarsDB.Matches.JoinGetTyped<MyMatches>(
                "DISTINCT Match.Id as MatchId, p1.Id as FromPlayer, p2.Id as ToPlayer, p1.DisplayName as FromPlayerName, p2.DisplayName as ToPlayerName,p1.Avatar as FromPlayerAvatar, p2.Avatar as ToPlayerAvatar, p1.Wins as FromPlayerWins, p2.Wins as ToPlayerWins, p1.Losses as FromPlayerLosses, p2.Losses as ToPlayerLosses, p1.LiveId as FromPlayerLiveId, p2.LiveId as ToPlayerLiveId",
                "Inner Join Player p1 on Match.Player1 = p1.Id Inner join Player p2 on Match.Player2 = p2.Id",
                string.Format("p1.LiveId ='{0}' OR p2.LiveId ='{0}'", liveId),
                "ORDER By p1.Wins DESC", 
                new Dictionary<string, object>());
        }


    }
}