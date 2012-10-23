#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using My.Common.DAL;
using My.Common.Web;
using My.Example.DAL;

using NLog;


#endregion



namespace My.Example.Web.Anonym
{
    public partial class Login : PageBaseAnonym<NoParams>
    {
        static readonly Logger Logger = LogManager.GetLogger("Login");


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.User.Identity.IsAuthenticated)
                Response.Redirect("~/");
            else
                LoginComponent.Focus();
        }


        protected void LoginComponent_Authenticate(object sender, AuthenticateEventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid)
                return;

            UserDTO u = UsersFacade.Auth(LoginComponent.UserName, LoginComponent.Password);
            if (u == null)
                Logger.Warn("auth failed: {0} host {1}", LoginComponent.UserName, HttpContext.Current.Request.UserHostName);
            else
                Logger.Info("auth success: {0}, isactive {1}  host {2}", u.Login, u.IsActive, HttpContext.Current.Request.UserHostName);

            if (u != null)
            {
                if (u.IsActive)
                {
                    string str = Request.QueryString["ReturnUrl"];
                    if (string.IsNullOrEmpty(str) || Regex.IsMatch(str, @"/?default\.aspx\??", RegexOptions.IgnoreCase))
                        FormsAuthentication.SetAuthCookie(LoginComponent.UserName, LoginComponent.RememberMeSet);
                    FormsAuthentication.RedirectFromLoginPage(LoginComponent.UserName, LoginComponent.RememberMeSet);
                }
                else
                    LoginComponent.FailureText = "Ваш пользователь неактивен";
            }
        }


        protected override DbBase GetDb()
        {
            return new Db();
        }
    }
}