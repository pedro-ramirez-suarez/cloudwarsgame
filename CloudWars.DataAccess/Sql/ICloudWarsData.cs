using System;
using System.Collections.Generic;
using CloudWars.Entities.Game;
using CloudWars.Entities.Player;

namespace CloudWars.DataAccess.Sql
{
    public interface ICloudWarsData
    {
        Guid CreateMatch(Guid player1, Guid player2);
        Match GetMatch(Guid matchId);
        IEnumerable<MatchUnit> GetUnits(Guid matchId);
        void DeleteMatch(Guid matchId);
        void UpdateMatch(object values, object where);
        void UpdateMatchUnit(object values, object where);
        void UpdatePlayer(object values, object where);
        void PlayerWin(Guid playerId);
        void PlayerLose(Guid playerId);
        Guid ChallengePlayer(Guid fromId, Guid toId);
        Challenge GetChallenge(Guid challengeId);
        void AcceptChallenge(Guid challengeId);
        void RejectChallenge(Guid challengeId);
        Player GetPlayer(Guid playerId);
        void AddNotification(PlayerNotification notification);
        Guid GetPlayerId(string clientId);
        string GetClientId(Guid playerId);
        void PlayerIsOnline(string liveId, string clientId, double latitude, double longitude);
        Guid? IsPlayingMatch(Guid playerId);
        string GetPlayerName(string clientId);
    }
}
