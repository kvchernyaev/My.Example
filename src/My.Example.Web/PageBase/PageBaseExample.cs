#region usings
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI.WebControls;

using JetBrains.Annotations;

using My.Common;
using My.Common.DAL;
using My.Common.Web;
using My.Example.DAL;

using NLog;


#endregion



namespace My.Example.Web
{
    public partial class PageBaseExample<TParameters> : PageBaseLogined<TParameters>
            where TParameters : PageParametersBase
    {
        // ReSharper disable StaticFieldInGenericType
        [NotNull] protected static readonly Db Db;
        [NotNull] protected static readonly CacheExample BLCache;
        [NotNull] protected static readonly Logger Logger = LogManager.GetLogger("PageBaseAuctions");
        // ReSharper restore StaticFieldInGenericType

        static PageBaseExample()
        {
            Db = new Db();
            BLCache = new CacheExample(Db);
        }


        protected override DbBase GetDb()
        {
            return Db;
        }


        protected override IEnumerable<int> MyRoles { get { return LoginedUser.Roles.Select(r => r.UserRoleId); } }


        // ReSharper disable MemberCanBeProtected.Global
        [NotNull]
        public static UserDTO LoginedUser
        {
            get
            {
                UserDTO u = HttpContext.Current.Cache[HttpContext.Current.Session.SessionID + "LoginedUser"] as UserDTO;
                string login = HttpContext.Current.User.Identity.Name;
                if (u == null || login != u.Login)
                {
                    u = Db.FindUserDTO(login);
                    if (u == null)
                    {
                        // это происходит, когда в БД меняется логин текущего пользователя
                        try
                        {
                            FormsAuthentication.SignOut();
                        }
                        catch (HttpException ex)
                        {
                            Logger.ErrorException("FormsAuthentication.SignOut()", ex);
                        }
                        Logger.Error("No user with login=[{0}], so signed out", login);

                        HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
                        HttpContext.Current.Response.Flush();
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        HttpContext.Current.Response.End();
                        throw new DislogoutException(string.Format("No user with login=[{0}], so signed out", login));
                    }

                    HttpContext.Current.Cache.Add(HttpContext.Current.Session.SessionID + "LoginedUser", u,
                                                  BLCache.GetUserAggregateCacheDependency(), DateTime.Today.AddDays(1), TimeSpan.Zero, CacheItemPriority.Normal, null);
                }
                return u;
            }
        }


        // ReSharper restore MemberCanBeProtected.Global


        public static string TelForHtml(string tel)
        {
            // (499) 197-23-21,  197-38-02
            // (499) 973-53-87/53-95/53-75
            // тел. (495) 974-01-09,  +7916 6789048
            // моб. 8 (905) 175-57-54
            tel = tel.TrimOrNullIfEmpty();
            if (tel == null)
                return null;

            Regex r = new Regex(@"(\+?\s*\d+\s*)?(\(\s*\d+\s*\)\s*)?[\d\-\s\\\/]{4,}");
            string s = r.Replace(tel, m => string.Format("<span style='white-space: nowrap;'>{0}</span> ", m.Captures[0].Value));
            return s;
        }


        protected void gv_Users_Set_RolesGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // раскрасим строки в разные цвета от роли
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //    if (e.Row.DataItem != null && ((dynamic)e.Row.DataItem).IsEndPoint)
            //        e.Row.BackColor = Color.BlanchedAlmond;

            ListView lvRoles = (ListView) e.Row.FindControl("lvRoles");
            if (lvRoles != null && e.Row.DataItem != null)
            {
                dynamic u = e.Row.DataItem;
                IEnumerable<UserRoleDTO> roles = ((List<UserRoleDTO>) u.Roles);
                lvRoles.DataSource = roles.Select(r => new ForUI(r, ForUICommon.For.Cbl)).ToArray();
                lvRoles.DataBind();
            }
        }
    }
}