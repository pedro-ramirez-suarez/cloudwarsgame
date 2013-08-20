using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Needletail.DataAccess.Attributes;
using CloudWars.Entities.Game;
using CloudWars.Entities.Player;
using Needletail.DataAccess;

namespace CloudWars.DataAccess.Sql
{
    public class CloudWarsDB
    {
        /// <summary>
        /// The connection string for the game
        /// </summary>
        public const string ConnectionString = "DefaultConnection";

        static DBTableDataSourceBase<Match, Guid> _Matches = new DBTableDataSourceBase<Match, Guid> (ConnectionString, "Match");
        public static DBTableDataSourceBase<Match, Guid> Matches
        {
            get 
            {
                return _Matches;
            }
        }


        static DBTableDataSourceBase<MatchUnit, Guid> _MatchUnits = new DBTableDataSourceBase<MatchUnit, Guid>(ConnectionString, "MatchUnit");
        public static DBTableDataSourceBase<MatchUnit, Guid> MatchUnits
        {
            get
            {
                return _MatchUnits;
            }
        }


        static DBTableDataSourceBase<PlayerUnit, Guid> _PlayerUnits = new DBTableDataSourceBase<PlayerUnit, Guid>(ConnectionString, "PlayerUnit");
        static DBTableDataSourceBase<PlayerUnit, Guid> PlayerUnits
        {
            get
            {
                return _PlayerUnits;
            }
        }


        static DBTableDataSourceBase<Player, Guid> _Players = new DBTableDataSourceBase<Player, Guid>(ConnectionString, "Player");
        public static DBTableDataSourceBase<Player, Guid> Players
        {
            get
            {
                return _Players;
            }
        }

        static IEnumerable<PlayerUnit> _VanillaUnits;
        public static IEnumerable<PlayerUnit> VanillaUnits 
        {
            get 
            {
                if (_VanillaUnits == null)
                {
                    _VanillaUnits = PlayerUnits.GetAll();
                }
                return _VanillaUnits;
            }
        }

        static DBTableDataSourceBase<PlayerNotification, Guid> _PlayerNotifications = new DBTableDataSourceBase<PlayerNotification, Guid>(ConnectionString, "PlayerNotification");
        public static DBTableDataSourceBase<PlayerNotification, Guid> PlayerNotifications
        {
            get
            {
                return _PlayerNotifications;
            }
        }


        static DBTableDataSourceBase<Challenge, Guid> _Challenges = new DBTableDataSourceBase<Challenge, Guid>(ConnectionString, "Challenge");
        public static DBTableDataSourceBase<Challenge, Guid> Challenges
        {
            get
            {
                return _Challenges;
            }
        }
    }
}
