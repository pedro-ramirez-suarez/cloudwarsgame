using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClourWars.Web.Code;
using CloudWars.Entities.Player;
using Microsoft.Security.Application;

namespace ClourWars.Game.Controllers
{
    public class PlayerController : Controller
    {
        /// <summary>
        /// Player's profile
        /// </summary>
        [Authorize]
        public ActionResult Index()
        {
            if (!ProfileHelper.UserHasProfile(ProfileHelper.GetIdentity))
                return RedirectToAction("CreateProfile");
            return PartialView(ProfileHelper.GetPlayerProfile(ProfileHelper.GetIdentity));
        }

        [Authorize]
        public ActionResult CreateProfile()
        {
            var profile = ProfileHelper.GetPlayerProfile(ProfileHelper.GetIdentity);
            if (profile == null)
                profile = new Player { DisplayName = string.Empty };
            return PartialView(profile);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateProfile(Guid id, string displayName, string avatar)
        {
            displayName = Encoder.HtmlEncode(displayName);
            if(displayName .Length > 50)
                displayName = displayName.Substring(0, 50);
            ProfileHelper.UpdateProfile(id, displayName, avatar, ProfileHelper.GetIdentity);
            return RedirectToAction("Index","Home");
        }


        [Authorize]
        public ActionResult EditProfile()
        {
            if (!ProfileHelper.UserHasProfile(ProfileHelper.GetIdentity))
            {
                return RedirectToAction("CreateProfile", "Player");
            }

            var profile = ProfileHelper.GetPlayerProfile(ProfileHelper.GetIdentity);
            if (profile == null)
                profile = new Player { DisplayName = string.Empty };
            return PartialView(profile);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditProfile(Guid id, string displayName,string avatar)
        {
            displayName = Encoder.HtmlEncode(displayName);
            if (displayName.Length > 50)
                displayName = displayName.Substring(0, 50);
            ProfileHelper.UpdateProfile(id, displayName, avatar, ProfileHelper.GetIdentity);
            return RedirectToAction("Index");
        }
    }
}
