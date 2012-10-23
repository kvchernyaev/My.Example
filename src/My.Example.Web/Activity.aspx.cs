#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using My.Common;
using My.Example.DAL;


#endregion



namespace My.Example.Web
{
    public partial class Activity : PageBaseExample<Activity.Parameters>
    {
        const string DefaultSortExpression = "CreatedDate";
        const SortDirection DefaultSortDir = SortDirection.Descending;
        const int DefaultPageSize = 1000;

        protected override int[] GrantedRoles { get { return new int[] {1, 2}; } }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadFilterFromQS();

                cc.DefaultValue = DefaultPageSize;
                gvAct.PageSize = DefaultPageSize;

                // this will do DataBind
                gvAct.Sort(DefaultSortExpression, DefaultSortDir);
                gvAct.DataBind();
                SetFilterLink();
            }
        }


        void LoadFilterFromQS()
        {
            tbEndCreatedTime.Text = DateTime.Now.AddDays(1).Date.ToString("dd.MM.yy HH:mm:ss");
            tbBeginCreatedTime.Text = DateTime.Now.Date.ToString("dd.MM.yy HH:mm:ss");

            if (Ps.FilterUsers != null && Ps.FilterUsers.Length > 0)
                cbUserAny.Checked = true;
            else
            {
                if (Ps.FilterUserAny.HasValue)
                    cbUserAny.Checked = Ps.FilterUserAny.Value;
                if (Ps.FilterExcludeMe.HasValue)
                    cbNotMe.Checked = Ps.FilterExcludeMe.Value;
                tbUserFilter.Text = Ps.FilterUserStr;
            }
            if (Ps.FilterDateAny.HasValue)
                cbCreatedTimeAny.Checked = Ps.FilterDateAny.Value;

            tbSearchString.Text = Ps.SearchString;
            if (Ps.FilterDateBegin.HasValue)
                tbBeginCreatedTime.Text = Ps.FilterDateBegin.Value.ToString("dd.MM.yy HH:mm:ss");
            if (Ps.FilterDateEnd.HasValue)
                tbEndCreatedTime.Text = Ps.FilterDateEnd.Value.ToString("dd.MM.yy HH:mm:ss");

            if (!string.IsNullOrEmpty(Ps.IsPostBack))
                ddlIsPostBack.SelectedValue = Ps.IsPostBack;
        }


        void SetFilterLink()
        {
            hlFilterLink.NavigateUrl = Url();
        }


        UserActivityDTOFinder Finder
        {
            get
            {
                List<int> users = new List<int>();
                if (Ps.FilterUsers != null && Ps.FilterUsers.Length > 0)
                    users = Ps.FilterUsers.ToList();
                else if (!cbUserAny.Checked && !string.IsNullOrWhiteSpace(tbUserFilter.Text))
                {
                    string[] ss = tbUserFilter.Text.Split(new char[] {',', ';'}, StringSplitOptions.RemoveEmptyEntries).Select(s => s.TrimOrNullIfEmpty()).Where(s => s != null).ToArray();
                    foreach (string s in ss)
                    {
                        List<UserDTO> us = Db.FindUserDTOs(new UserDTOFinder {SearchString = s});
                        users.AddRange(us.Select(u => u.UserId));
                    }
                }

                bool b;
                UserActivityDTOFinder f = new UserActivityDTOFinder
                                          {
                                                  SearchString = tbSearchString.Text.Trim(),
                                                  CreatedDateBegin = cbCreatedTimeAny.Checked || string.IsNullOrWhiteSpace(tbBeginCreatedTime.Text)
                                                                             ? null : (DateTime?) DateTime.Parse(tbBeginCreatedTime.Text.Trim()),
                                                  CreatedDateEnd = cbCreatedTimeAny.Checked || string.IsNullOrWhiteSpace(tbEndCreatedTime.Text)
                                                                           ? null : (DateTime?) DateTime.Parse(tbEndCreatedTime.Text.Trim()),
                                                  UserId = users.Distinct().ToList(),
                                                  UserIdNotIn = cbUserAny.Checked || !cbNotMe.Checked ? null : new List<int> {LoginedUser.UserId},
                                                  IsPostBack = bool.TryParse(ddlIsPostBack.SelectedValue, out b) ? b : (bool?) null
                                          };
                return f;
            }
        }


        public int SelectActCount()
        {
            int count = Db.FindUserActivityDTOCount(Finder);
            lFoundCount.Text = count.ToString();
            cc.SetAsCount(count);
            return count;
        }


        public IEnumerable<object> SelectAct(string orderBy, int count, int startIndex)
        {
            List<UserActivityDTO> uas = Db.FindUserActivityDTOs(Finder, orderBy, startIndex, count);
            DateTime now = DateTime.Now;
            return uas.Select(x => new
                                   {
                                           x.UserId,
                                           x.RawUrl,
                                           x.IsChangePsw,
                                           x.CreatedDate,
                                           BLCache.FindUserCached(x.UserId).Login,
                                           BLCache.FindUserCached(x.UserId).UserFio,
                                           SecondsAgo = now - x.CreatedDate,
                                           x.Browser,
                                           x.UserHostAddress,
                                           x.IsPostBack,
                                   });
        }


        protected void FilterChanged(object sender, EventArgs e)
        {
            Page.Validate("Filter");
            if (!Page.IsValid)
                return;

            gvAct.PageSize = cc.SelectedValue;
            gvAct.PageIndex = 0;
            gvAct.DataBind();
            SetFilterLink();
        }


        protected void bFilterByUser_OnClick(object sender, ImageClickEventArgs e)
        {
            IButtonControl b = (IButtonControl) sender;
            int userId = int.Parse(b.CommandArgument);
            Response.Redirect(Url(userId));
        }


        protected void bFilterByDateBegin_OnClick(object sender, ImageClickEventArgs e)
        {
            IButtonControl b = (IButtonControl) sender;
            string dbeg = b.CommandArgument;

            cbCreatedTimeAny.Checked = false;
            tbBeginCreatedTime.Text = dbeg;
            Response.Redirect(Url());
        }


        protected void bFilterByDateEnd_OnClick(object sender, ImageClickEventArgs e)
        {
            IButtonControl b = (IButtonControl) sender;
            string dbeg = b.CommandArgument;

            cbCreatedTimeAny.Checked = false;
            tbEndCreatedTime.Text = dbeg;
            Response.Redirect(Url());
        }


        protected void bFilterBy_SearchString_OnClick(object sender, ImageClickEventArgs e)
        {
            IButtonControl b = (IButtonControl) sender;
            string s = b.CommandArgument;
            Response.Redirect(Url(searchString: s));
        }


        string Url(int? userid = null, string searchString = null)
        {
            StringBuilder sb = new StringBuilder("~/Activity.aspx?");

            string s = Server.UrlEncode(string.IsNullOrEmpty(searchString) ? tbSearchString.Text : searchString);
            if (!string.IsNullOrEmpty(s))
                sb.Append("SearchString=" + s + "&");

            if (userid.HasValue)
                sb.Append("FilterUsers=" + userid + "&");
            else if (Ps.FilterUsers != null && Ps.FilterUsers.Length > 0)
                sb.Append("FilterUsers=" + string.Join(",", Ps.FilterUsers) + "&");

            sb.Append("FilterUserAny=" + cbUserAny.Checked.ToString() + "&");
            if (!cbUserAny.Checked)
            {
                sb.Append("FilterExcludeMe=" + cbNotMe.Checked.ToString() + "&");
                if (!string.IsNullOrEmpty(tbUserFilter.Text))
                    sb.Append("FilterUserStr=" + Server.UrlEncode(tbUserFilter.Text) + "&");
            }

            sb.Append("FilterDateAny=" + cbCreatedTimeAny.Checked.ToString() + "&");
            if (!cbCreatedTimeAny.Checked)
            {
                sb.Append("FilterDateBegin=" + Server.UrlEncode(tbBeginCreatedTime.Text) + "&");
                sb.Append("FilterDateEnd=" + Server.UrlEncode(tbEndCreatedTime.Text) + "&");
            }
            sb.Append("IsPostBack=" + ddlIsPostBack.SelectedValue + "&");

            return sb.ToString();
        }


        protected void bRefresh_OnClick(object sender, EventArgs e)
        {
            FilterChanged(null, null);
        }
    }
}