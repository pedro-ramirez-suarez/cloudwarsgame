using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Needletail.DataAccess.Attributes;


namespace CloudWars.Entities.Game
{
    public class Challenge
    {

        [TableKeyAttribute(CanInsertKey = true)]
        public Guid Id { get; set; }
        public Guid Player1 { get; set; }
        public Guid Player2 { get; set; }
        public bool Accepted { get; set; }

    }
}
