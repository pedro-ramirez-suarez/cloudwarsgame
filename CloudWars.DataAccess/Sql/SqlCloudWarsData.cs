using System;
using System.Collections.Generic;
using CloudWars.Entities.Game;
using CloudWars.Entities.Player;

namespace CloudWars.DataAccess.Sql
{
    public class SqlCloudWarsData : ICloudWarsData
    {
        public Guid CreateMatch(Guid player1, Guid player2)
        {
            return CloudWarsData.CreateMatch(player1, player2);
        }

        public Match GetMatch(Guid matchId)
        {
            return CloudWarsData.GetMatch(matchId);
        }

        public IEnumerable<MatchUnit> GetUnits(Guid matchId)
        {
            return CloudWarsData.GetUnits(matchId);
        }

        public void DeleteMatch(Guid matchId)
        {
            CloudWarsData.DeleteMatch(matchId);
        }

        public void UpdateMatch(object values, object where)
        {
            CloudWarsData.UpdateMatch(values, where);
        }

        public void UpdateMatchUnit(object values, object where)
        {
            CloudWarsData.UpdateMatchUnit(values, where);
        }

        public void UpdatePlayer(object values, object where)
        {
            CloudWarsData.UpdatePlayer(values, where);
        }

        public void PlayerWin(Guid playerId)
        {
            CloudWarsData.PlayerWin(playerId);
        }

        public void PlayerLose(Guid playerId)
        {
            CloudWarsData.PlayerLose(playerId);
        }

        public Guid ChallengePlayer(Guid fromId, Guid toId)
        {
            return CloudWarsData.ChallengePlayer(fromId, toId);
        }

        public Challenge GetChallenge(Guid challengeId)
        {
            return CloudWarsData.GetChallenge(challengeId);
        }

        public void AcceptChallenge(Guid challengeId)
        {
            CloudWarsData.AcceptChallenge(challengeId);
        }

        public void RejectChallenge(Guid challengeId)
        {
            CloudWarsData.RejectChallenge(challengeId);
        }

        public Player GetPlayer(Guid playerId)
        {
            return CloudWarsData.GetPlayer(playerId);
        }

        public void AddNotification(PlayerNotification notification)
        {
            CloudWarsData.AddNotification(notification);
        }

        public Guid GetPlayerId(string clientId)
        {
            return CloudWarsData.GetPlayerId(clientId);
        }

        public string GetClientId(Guid playerId)
        {
            return CloudWarsData.GetClientId(playerId);
        }

        public void PlayerIsOnline(string liveId, string clientId, double latitude, double longitude)
        {
            CloudWarsData.PlayerIsOnline(liveId, clientId, latitude, longitude);
        }

        public Guid? IsPlayingMatch(Guid playerId)
        {
            return CloudWarsData.IsPlayingMatch(playerId);
        }

        public string GetPlayerName(string clientId)
        {
            return CloudWarsData.GetPlayerName(clientId);
        }
    }
}
