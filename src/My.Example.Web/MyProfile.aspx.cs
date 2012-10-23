#region usings
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using My.Common;
using My.Common.Web;
using My.Example.DAL;

using NLog;


#endregion



namespace My.Example.Web
{
    public partial class MyProfile : PageBaseExample<NoParams>
    {
        static readonly Logger Logger = LogManager.GetLogger("MyProfile");


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserDTO u = LoginedUser;
                hpUserId.Text = u.UserId.ToString();
                hpUserId.NavigateUrl = "~/EditUser.aspx?UserId=" + u.UserId;
                lLogin.Text = u.Login;
                lFIO.Text = string.IsNullOrEmpty(u.UserFio) ? "-" : u.UserFio;
                lFax.Text = string.IsNullOrEmpty(u.Fax) ? "-" : u.Fax;
                lTelephone.Text = string.IsNullOrEmpty(u.Telephone) ? "-" : u.Telephone;
                lRegistrationDate.Text = Parser.DateTimeForUI(u.CreatedDate, true);
                lEmail.Text = string.IsNullOrEmpty(u.Email) ? "-"
                                      : string.Format("<a href='mailto:{0}'>{0}</a>", u.Email);
            }
        }


        public ForUI[] SelectUserRoles()
        {
            ForUI[] l = LoginedUser.Roles.Select(r => new ForUI(r, ForUICommon.For.Cbl)).ToArray();
            return l;
        }


        protected void bSubmitNewPass_Click(object sender, EventArgs e)
        {
            Page.Validate("ChangePswValGroup");
            if (!Page.IsValid)
                return;

            UsersFacade.ChangePassword(LoginedUser, tbNewPsw.Text, LoginedUser);
            Cache.Remove(Session.SessionID + "LoginedUser");

            Response.Redirect(Request.RawUrl);
        }


        protected void vOlPsw_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = UsersFacade.CheckPsw(LoginedUser, args.Value);
        }
    }
}