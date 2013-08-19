using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class CDNHelper
{
    //Use this for a CDN
    //private const string blobBaseUrl = "http://retostarwarscloudwars.blob.core.windows.net/";

    //use this for local urls
    private static string blobBaseUrl
    {
        get 
        {
            return string.Format("{0}", HttpContext.Current.Request.Url.Authority); 
        }
    }


    public static string CssUrl
    {
        get 
        {
            return string.Format("{0}/Content/",blobBaseUrl);
        }
    }


    public static string ImagesUrl
    {
        get
        {
            //return string.Format("~/img/");
            return string.Format("{0}/img/", blobBaseUrl);
        }
    }


    public static string ScriptsUrl
    {
        get
        {
            return string.Format("{0}/Scripts/", blobBaseUrl);
        }
    }


    public static string SoundsUrl
    {
        get
        {
            return string.Format("{0}/sounds/", blobBaseUrl);
        }
    }


    public static string StaticContentUrl
    {
        get
        {
            return string.Format("{0}content/", blobBaseUrl);
        }
    }
}
