using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClourWars.Web.ServerClientEntities
{
    public class Damage
    {
        public Guid UnitId { get; set; }
        public int HealthAfterAttack { get; set; }
    }
}