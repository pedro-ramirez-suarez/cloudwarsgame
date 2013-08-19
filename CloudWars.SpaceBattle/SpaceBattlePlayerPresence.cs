using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudWars.Common;
using CloudWars.DataAccess.Sql;
using CloudWars.Entities.Player;

namespace CloudWars.SpaceBattle
{
    public class SpaceBattlePlayerPresence : IPlayerPresence
    {

        public Guid GetPlayerId(string clientId)
        {
            return CloudWarsData.GetPlayerId(clientId);
        }


        public string GetClientId(Guid playerId)
        {
            return CloudWarsData.GetClientId(playerId);
        }

        public void PlayerIsOnLine(string liveId, string clientId, double latitude, double longitude)
        {
            CloudWarsData.PlayerIsOnline(liveId, clientId,latitude, longitude);
        }

        public void PlayerDisconnected(string clientId)
        {
            CloudWarsData.UpdatePlayer(new { Status = PlayerStatus.OffLine, ClientId = string.Empty }, new { ClientId = clientId });
            
        }


        public Guid? IsPlayingMatch(Guid playerId)
        {
            return CloudWarsData.IsPlayingMatch(playerId);
        }


        public string GetPlayerNameByClientId(string clientId)
        {
            return CloudWarsData.GetPlayerName(clientId);
        }
    }
}
