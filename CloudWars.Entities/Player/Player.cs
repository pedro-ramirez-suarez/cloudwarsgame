using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Needletail.DataAccess.Attributes;
using Microsoft.SqlServer.Types;

namespace CloudWars.Entities.Player
{
    public class Player
    {
        [TableKeyAttribute(CanInsertKey = true)]
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public string Status { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastActivity { get; set; }
        public SqlGeography Location { get;set; }
        public string ClientId { get; set; }
        public string LiveId { get; set; }
    }
}
