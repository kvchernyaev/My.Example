#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using My.Common.Web;
using My.Example.DAL;

using NLog;


#endregion



namespace My.Example.Web
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        static readonly Logger Logger = LogManager.GetLogger("SiteMaster");
        static readonly Db Db = new Db();


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Logger.Info("sessionid {2}, host {0}, url {1}, login {3} ({4})", Request.UserHostName, Request.RawUrl, Session.SessionID,
                        LoginedUser.Login, LoginedUser.UserId);
            Db.Insert(new UserActivityDTO(LoginedUser.UserId, false, Request.RawUrl, Request.Browser.Type, Request.UserHostAddress, IsPostBack));
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                menuTable.DataBind();
            }
            HeadLoginView.DataBind(); // i dont know why it must be outside this if

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "time",
                                                        "var timeServer; var timeClient; $(document).ready(function () { timeServer = new Date("
                                                        + DateTime.Now.AddMonths(-1).AddSeconds(5).ToString("yyyy, MM, d, HH, mm, ss, fff")
                                                        + @"); timeClient = new Date(); wr_hours(); setInterval(""wr_hours();"", 1000); }); ", true);
        }


        protected static UserDTO LoginedUser { get { return PageBaseExample<NoParams>.LoginedUser; } }


        protected void Page_PreRender()
        {
            lCaption.Text = Page.Title;
            Page.Title = Config.GetPageName(Page.Title, Page.User.Identity.Name);
        }


        protected void MainScriptManager_OnAsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            if (Request.IsLocal && Config.LogAjaxErrorsFromLocalhost || !Request.IsLocal)
                Global.Fatal(e.Exception, HttpContext.Current, Page.User.Identity);
        }


        protected void HeadLoginStatus_OnLoggedOut(object sender, EventArgs e)
        {
            Logger.Info("HeadLoginStatus_OnLoggedOut");
        }


        protected void HeadLoginStatus_OnLoggingOut(object sender, LoginCancelEventArgs e)
        {
            Logger.Info("HeadLoginStatus_OnLoggingOut");
            Cache.Remove(Session.SessionID + "LoginedUser");
        }

        #region ami
        /// <summary>
        ///     Am I in given roles?
        /// </summary>
        public bool AmI(IEnumerable<int> roles)
        {
            return LoginedUser.Roles.Select(r => r.UserRoleId).Intersect(roles).FirstOrDefault() != 0;
        }


        /// <summary>
        ///     Am I in given roles?
        /// </summary>
        public bool AmI(params int[] roles)
        {
            return LoginedUser.Roles.Select(r => r.UserRoleId).Intersect(roles).FirstOrDefault() != 0;
        }


        /// <summary>
        ///     Am I in given role?
        /// </summary>
        public bool AmI(int roleId)
        {
            return LoginedUser.Roles.Select(r => r.UserRoleId).Any(rid => rid == roleId);
        }
        #endregion
    }
}