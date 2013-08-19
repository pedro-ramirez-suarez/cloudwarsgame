using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Needletail.DataAccess.Attributes;

namespace CloudWars.Entities.Game
{
    public class PlayerNotification
    {
        [TableKeyAttribute(CanInsertKey = true)]
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid MatchId { get; set; }
        public string NotificationType { get; set; }
        public Guid? OtherPlayer { get; set; }
        public int? Row { get; set; }
        public int? Col { get; set; }
    }
}
