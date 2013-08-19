using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudWars.Common
{
    public interface IPlayerPresence
    {
         Guid GetPlayerId(string clientId);
         string  GetClientId(Guid playerId);
         string GetPlayerNameByClientId(string clientId);
         void PlayerIsOnLine(string liveId, string clientId, double latitude, double longitude);
         void PlayerDisconnected(string clientId);
         Guid? IsPlayingMatch(Guid playerId);
    }
}
