#region usings
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using My.Common;
using My.Example.DAL;

using NLog;


#endregion



namespace My.Example.Web
{
    public partial class EditUser : PageBaseExample<EditUser.Parameters>
    {
        new static readonly Logger Logger = LogManager.GetLogger("EditUser");

        #region auth
        protected override void AuthDepended()
        {
            //если роль не 2, то только себя можно
            if (!AmI(2) && LoginedUser.UserId != Ps.UserId)
                throw new PageDeniedException("У вас нет доступа к другому пользователю");
        }


        protected override void AuthVisual()
        {
            if (!AmI(2))
            {
                // readonly
                changepswArea.Visible = bSaveUser.Visible = false;
                allArea.Enabled = false;

                areaAdminOnly.Visible = false;
            }
            areaDeleting.Visible = AmI(1);
        }
        #endregion

        protected void Page_Init()
        {
            this.Title = string.Format(this.Title + (LoginedUser.UserId == Ps.UserId ? " (это вы)" : ""), HttpUtility.HtmlEncode(Ps.CurrentUser.Login));
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tbEmail.Text = Ps.CurrentUser.Email;
                tbFIO.Text = Ps.CurrentUser.UserFio;
                tbFax.Text = Ps.CurrentUser.Fax;
                tbLogin.Text = Ps.CurrentUser.Login;
                tbTelephone.Text = Ps.CurrentUser.Telephone;
                cbIsActive.Checked = Ps.CurrentUser.IsActive;
                lCreatedDate.Text = Parser.DateTimeForUI(Ps.CurrentUser.CreatedDate, withSeconds: false);

                if (AmI(1, 2))
                {
                    var roles = (from a in UserRoleDTOs.Values
                                 join g in Ps.CurrentUser.Roles
                                         on a.UserRoleId equals g.UserRoleId into j
                                 from g in j.DefaultIfEmpty()
                                 select new {Role = new ForUI(a, ForUICommon.For.Cbl), Checked = g != null})
                            .Select(ur => new {ur.Role.Caption, ur.Role.ToolTip, ur.Role.Id, ur.Role.Color, ur.Checked}).ToList();
                    cblRoles.DataSource = roles;
                }
                else
                    cblRoles.DataSource = Ps.CurrentUser.Roles.Select(r => new ForUI(r, ForUICommon.For.Cbl))
                            .Select(f => new {f.Caption, f.ToolTip, f.Id, f.Color, Checked = true});
                cblRoles.DataBind();

                bChangePsw.OnClientClick = string.Format(bChangePsw.OnClientClick, Ps.CurrentUser.Login);
            }
        }


        protected void vLoginExists_ServerValidate(object source, ServerValidateEventArgs args)
        {
            UserDTO find = Db.FindUserDTO(args.Value);
            args.IsValid = find == null || find.UserId == Ps.CurrentUser.UserId;
        }


        protected void vLoginRepeat_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = tbPsw.Text == tbPswRepeat.Text;
        }


        protected void bSaveUser_Click(object sender, EventArgs e)
        {
            Page.Validate("EditUser");
            if (!Page.IsValid)
                return;
            Logger.Info("saving user");

            UserDTOUpdater uu = new UserDTOUpdater(Ps.CurrentUser.UserId)
                                {
                                        Login = tbLogin.Text.Trim(),
                                        Email = tbEmail.Text.TrimOrNullIfEmpty(),
                                        Fax = tbFax.Text.TrimOrNullIfEmpty(),
                                        Telephone = tbTelephone.Text.TrimOrNullIfEmpty(),
                                        UserFio = tbFIO.Text.TrimOrNullIfEmpty(),
                                        IsActive = cbIsActive.Checked,
                                };
            uu.ExcludeEquals(Ps.CurrentUser);
            Logger.Info("new values: " + uu.ChangedAsString);

            bool doLogout = Ps.UserId == LoginedUser.UserId && uu.ChangedProps.Any(x => x.Key == "Login");

            UsersFacade.UpdateUser(uu,
                                   cblRoles.GetSelectedIntValues().Select(i => new UserRoleDTO {UserRoleId = i}).ToList(),
                                   LoginedUser);

            if (doLogout)
                FormsAuthentication.SignOut();
            Response.Redirect("~/Users.aspx?locateUserId=" + Ps.CurrentUser.UserId);
        }


        protected void bChangePsw_Click(object sender, EventArgs e)
        {
            Page.Validate("Psw");
            if (!Page.IsValid)
                return;

            Logger.Info("change psw");
            UsersFacade.ChangePassword(Ps.CurrentUser, tbPsw.Text, LoginedUser);
        }


        protected void bDelete_OnClick(object sender, EventArgs e)
        {
            try
            {
                Db.DeleteUserDeep(Ps.UserId);
                Response.Redirect("~/Users.aspx");
            }
            catch (MyException ex)
            {
                lDeletingError.Text = ex.Message;
                lDeletingError.Visible = true;
            }
        }
    }
}