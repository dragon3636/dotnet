using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common
{
    public static class CommonConstants
    {
        //Role
        public static string MEMBER_GROUP = "MEMBER";
        public static string MOD_GROUP = "MOD";
        public static string ADMIN_GROUP = "ADMIN";
        // login
        public static  string USER_SESSION = "USER_SESSION";
        public static string CartSession = "CartSession";
        public static string SESSION_CREDENTIALS = "SESSION_CREDENTIALS";
        public static string CurrentCulture { get; set; }
    }
}