using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClourWars.Web.ServerClientEntities
{
    public class UnitInfo
    {
        public Guid PlayerId { get; set; }
        public Guid UnitId { get; set; }
        public string Name { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
    }
}