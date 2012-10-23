#region usings
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

using My.Common;
using My.Example.DAL;


#endregion



namespace My.Example.Web
{
    public static class DTOExtensions
    {
        static readonly Db Db = new Db();
        static readonly CacheExample Cache = new CacheExample(Db);


        public static Color Color(this UserRoleDTO r)
        {
            return Config.GetRoleColor(r.UserRoleId);
        }
    }
}