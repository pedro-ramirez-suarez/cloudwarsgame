using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudWars.Common
{
    public interface IFactory
    {
        IGame GetGame();
        IGame GetGame(Guid matchId,bool loadMatchData);
        IPlayerPresence GetPlayerPresence();
        IClientFeedback GetClientFeedback();
        
    }
}
