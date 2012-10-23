#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using My.Common;
using My.Common.Web;
using My.Example.DAL;
using My.WebServerControls;


#endregion



namespace My.Example.Web
{
    public partial class Users : PageBaseExample<Users.Parameters>, IPageWithOneStatFilter<UserDTOFinder>
    {
        const string DefaultUsersSortExpression = "Login";
        const SortDirection DefaultUsersSortDir = SortDirection.Ascending;
        const int DefaultPageSize = 100;
        readonly StatisticsFilterHelper<UserDTOFinder> _filterHelper;

        protected override int[] GrantedRoles { get { return new int[] {2, 3}; } }


        public Users()
        {
            _filterHelper = new StatisticsFilterHelper<UserDTOFinder>(() => Finder, Db.FindUserDTOCount, GetCurrentCount);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadFilterFromQS();

                cc.SelectedValue = DefaultPageSize;
                gvUsers.PageSize = DefaultPageSize;

                // this will do DataBind
                gvUsers.Sort(DefaultUsersSortExpression, DefaultUsersSortDir);
                if (Ps.LocateUserId.HasValue)
                    gvUsers.PageIndex = Db.FindUserDTOPageIndex(Finder, cc.SelectedValue, Ps.LocateUserId.Value,
                                                                DefaultUsersSortExpression + (DefaultUsersSortDir == SortDirection.Ascending ? "" : " desc"))
                                        ?? 0;
                SetFilterLink();
            }
        }


        public void SetFilterLink()
        {
            hlFilterLink.NavigateUrl = "~/Users.aspx?" + CalcQueryStringForUrl(Finder);
        }


        public void LoadFilterFromQS()
        {
            _filterHelper.SetInitStart();
            if (!string.IsNullOrEmpty(Ps.SearchString)) tbSearchString.Text = Ps.SearchString;

            if (Ps.CreatedDateBegin.HasValue || Ps.CreatedDateEnd.HasValue)
            {
                cbCreatedDateAny.Checked = false;
                tbBeginDate.Text = Ps.CreatedDateBegin.HasValue ? Ps.CreatedDateBegin.Value.ToString("dd.MM.yyyy") : "";
                tbEndDate.Text = Ps.CreatedDateEnd.HasValue ? Ps.CreatedDateEnd.Value.ToString("dd.MM.yyyy") : "";
            }
            else
            {
                tbBeginDate.Text = "01.01.2012";
                tbEndDate.Text = DateTime.Now.AddDays(1).Date.ToString("dd.MM.yyyy");
            }

            if (Ps.RoleId != null && Ps.RoleId.Length > 0)
            {
                cbRoleAny.Checked = false;
                cblRolesFilter.DataBind();
                cblRolesFilter.SetSelectedValues(Ps.RoleId);
            }

            ddlActivity.DataBind();
            ddlActivity.SelectedValue = Ps.IsActive.HasValue ? Ps.IsActive.ToString() : "-";

            _filterHelper.SetInitComplete();
            ReDataBindFilterCases();
        }


        public string CalcQueryStringForUrl(UserDTOFinder f)
        {
            List<string> l = new List<string>(6);

            if (!string.IsNullOrEmpty(f.SearchString)) l.Add("SearchString=" + HttpUtility.UrlEncode(f.SearchString));

            if (f.IsActive.HasValue) l.Add("IsActive=" + f.IsActive.ToString());
            if (f.Roles != null || f.SearchByNullRoles)
                l.Add("RoleId=" + (f.Roles == null ? "" : string.Join(",", f.Roles)) + (f.SearchByNullRoles ? ",0" : ""));

            if (f.CreatedDateBegin != null) l.Add("BeginDate=" + f.CreatedDateBegin.Value.ToString("dd.MM.yyyy"));
            if (f.CreatedDateEnd != null) l.Add("EndDate=" + f.CreatedDateEnd.Value.AddDays(-1).ToString("dd.MM.yyyy"));

            string qs = string.Join("&", l);
            return qs;
        }


        public void ReDataBindFilterCases()
        {
            cblRolesFilter.ReDataBind();
            ddlActivity.ReDataBind();
        }

        #region filter selectors
        IEnumerable<ForUICommon> _selectTenderTypes;


        IEnumerable<ForUICommon> _selectUserRoles;


        public IEnumerable<ForUICommon> SelectUserRoles()
        {
            return _filterHelper.GetForUIListAndSetStats(ref _selectUserRoles,
                                                         (f, l) =>
                                                             {
                                                                 f.Roles = l == null ? null : l.Where(x => x.IdInt > 0).Select(x => x.IdInt.Value).ToList();
                                                                 f.SearchByNullRoles = l != null && l.Any(x => x.IdInt == 0);
                                                             },
                                                         cbRoleAny,
                                                         () => UserRoleDTOs.Values.Select(r => new ForUI(r, ForUICommon.For.Cbl))
                                                                       .Concat(new[] {new ForUI("без роли", 0)}).ToList(),
                                                         forui => forui,
                                                         new ForUICommon("любая", ""));
        }


        public IEnumerable<ForUICommon> SelectActivity()
        {
            return _filterHelper.GetForUIListAndSetStats((f, l) => f.IsActive = l == null ? (bool?) null : bool.Parse(l[0].Id),
                                                         null, () => new List<ForUI>
                                                                     {
                                                                             new ForUI("Да", "True"),
                                                                             new ForUI("Нет", "False")
                                                                     },
                                                         forui => forui,
                                                         new ForUI("Не важно", "-")
                    );
        }
        #endregion

        protected void bLocateMe_OnClick(object sender, EventArgs e)
        {
            _locatingMe = true;
            gvUsers.PageIndex = Db.FindUserDTOPageIndex(Finder, cc.SelectedValue, LoginedUser.UserId,
                                                        gvUsers.SortExpression + (gvUsers.SortDirection == SortDirection.Ascending ? "" : " desc"))
                                ?? 0;
            gvUsers.DataBind();
        }


        public UserDTOFinder Finder
        {
            get
            {
                bool b;
                UserDTOFinder f = new UserDTOFinder
                                  {
                                          SearchString = tbSearchString.Text.TrimOrNullIfEmpty(),
                                          IsActive = bool.TryParse(ddlActivity.SelectedValue, out b) ? (bool?) b : null,
                                          CreatedDateBegin = cbCreatedDateAny.Checked ? null : (DateTime?) DateTime.Parse(tbBeginDate.Text),
                                          CreatedDateEnd = cbCreatedDateAny.Checked ? null : (DateTime?) DateTime.Parse(tbEndDate.Text).AddDays(1),
                                          Roles = cbRoleAny.Checked ? null : cblRolesFilter.GetSelectedIntValues().Where(i => i > 0).ToList(),
                                          SearchByNullRoles = !cbRoleAny.Checked && cblRolesFilter.GetSelectedIntValues().Any(i => i == 0),
                                  };

                return f;
            }
        }


        public int SelectCount()
        {
            int count = GetCurrentCount();
            lFoundCount.Text = count.ToString();
            cc.SetAsCount(count);
            return count;
        }


        int? _getCurrentCount;


        public int GetCurrentCount()
        {
            return _getCurrentCount ?? (_getCurrentCount = Db.FindUserDTOCount(Finder)).Value;
        }


        public IEnumerable<object> Select(string orderBy, int count, int startIndex)
        {
            return Db.FindUserDTOs(Finder, string.IsNullOrEmpty(orderBy) ? DefaultUsersSortExpression : orderBy,
                                   startIndex, count).Select(u => BLCache.FindUserCached(u.UserId)).ToArray();
        }


        public void FilterChanged(object sender, EventArgs e)
        {
            Page.Validate("UseCreatedDateFilter");
            if (!Page.IsValid)
                return;

            gvUsers.PageSize = cc.SelectedValue;
            gvUsers.PageIndex = 0;
            gvUsers.DataBind();
            SetFilterLink();

            ReDataBindFilterCases();
        }


        public void bRefresh_OnClick(object sender, EventArgs e)
        {
            FilterChanged(null, null);
        }


        protected void gvUsers_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            gv_Users_Set_RolesGroups_RowDataBound(sender, e);
        }


        bool _locatingMe;


        protected void gvUsers_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
            {
                int? userid = _locatingMe ? LoginedUser.UserId : Ps.LocateUserId;
                if (userid.HasValue && ((dynamic) e.Row.DataItem).UserId == userid.Value)
                    e.Row.Style["background-color"] = "orange";
            }
        }
    }
}