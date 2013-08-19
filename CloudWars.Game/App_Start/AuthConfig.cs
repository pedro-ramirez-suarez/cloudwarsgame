using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using CloudWars.Game.Models;

namespace CloudWars.Game
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            OAuthWebSecurity.RegisterTwitterClient(
                consumerKey: "gtVgY9UW2sNL6WbjZO3cQ",
                consumerSecret: "jWATZhpQJqBaJ6tcEs08JKC5hkaH6DOKcG3fTejIA");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "",
            //    appSecret: "");


            OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
