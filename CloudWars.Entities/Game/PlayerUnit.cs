using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Needletail.DataAccess.Attributes;

namespace CloudWars.Entities.Game
{
    public class PlayerUnit
    {
        [TableKeyAttribute(CanInsertKey = true)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int MaxHealth { get; set; }        
        public string Icon { get; set; }
        public bool IsRebel { get; set; }
    }
}
