using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudWars.Common;

namespace CloudWars.SpaceBattle
{
    public class SpaceBattleFactory : IFactory
    {
        public IGame GetGame()
        {
            return new SpaceBattleGame();
        }

        public IGame GetGame(Guid matchId, bool loadMatchData)
        {
            return new SpaceBattleGame(matchId, loadMatchData);
        }

        public IPlayerPresence GetPlayerPresence()
        {
            return new SpaceBattlePlayerPresence();
        }

        public IClientFeedback GetClientFeedback()
        {
            return new SpaceBattleClientFeedback();
        }
    }
}
