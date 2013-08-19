using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudWars.DataAccess.Sql;
using CloudWars.Entities.Player;
//using Microsoft.IdentityModel.Claims;

namespace ClourWars.Web.Code
{
    public class ProfileHelper
    {

        public static string GetIdentity
        {
            get 
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var claim = HttpContext.Current.User.Identity.Name; //((IClaimsIdentity)HttpContext.Current.User.Identity).Claims.FirstOrDefault(c => c.ClaimType.ToLower().EndsWith("nameidentifier"));
                    if (claim != null)
                        return claim;//claim.Value;
                    else
                        return string.Empty;
                }
                else
                    return string.Empty;
            }
        }

        public static bool UserHasProfile(string user)
        {
            var u = CloudWarsDB.Players.GetSingle(new { LiveId = user } );
            return u != null;
        }


        public static Player GetPlayerProfile(string user)
        {
            return CloudWarsDB.Players.GetSingle(new { LiveId = user });
        }


        public static void UpdateProfile(Guid id, string displayName, string avatar,string user)
        {
            if (id == Guid.Empty)
            {
                //create a new record
                var p = new Player { Id = Guid.NewGuid(), Avatar = avatar, DisplayName = displayName, IsOnline = true, LastActivity = DateTime.Now, Status = PlayerStatus.OnLine,  LiveId = user };
                CloudWarsDB.Players.Insert(p);
            }
            else
            {
                CloudWarsDB.Players.UpdateWithWhere(new { DisplayName = displayName, Avatar = avatar }, new { LiveId = user });
            }
        }
    }
}