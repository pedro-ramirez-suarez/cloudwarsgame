using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudWars.Common.Other
{
    public enum Command 
    { 
        NotYourTurn,
        ChallengePlayer,
        ChallengeAccepted,
        StartGame,
        EndGame,
        PlayerOnline,
        PlayerOffLine,
        GameAction
    }


    public enum GameAction
    { 
        PlayerReady,
        UnitMoved,
        Attack,
        ShotMade,
        ShotMissed
    }
        
}
