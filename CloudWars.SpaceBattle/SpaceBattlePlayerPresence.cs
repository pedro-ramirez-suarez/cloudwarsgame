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
        private readonly ICloudWarsData _data;

        public SpaceBattlePlayerPresence(ICloudWarsData data)
        {
            _data = data;
        }

        public SpaceBattlePlayerPresence() : this(new SqlCloudWarsData())
        {
        }

        public Guid GetPlayerId(string clientId)
        {
            return _data.GetPlayerId(clientId);
        }


        public string GetClientId(Guid playerId)
        {
            return _data.GetClientId(playerId);
        }

        public void PlayerIsOnLine(string liveId, string clientId, double latitude, double longitude)
        {
            _data.PlayerIsOnline(liveId, clientId,latitude, longitude);
        }

        public void PlayerDisconnected(string clientId)
        {
            _data.UpdatePlayer(new { Status = PlayerStatus.OffLine, ClientId = string.Empty }, new { ClientId = clientId });
            
        }


        public Guid? IsPlayingMatch(Guid playerId)
        {
            return _data.IsPlayingMatch(playerId);
        }


        public string GetPlayerNameByClientId(string clientId)
        {
            return _data.GetPlayerName(clientId);
        }
    }
}
