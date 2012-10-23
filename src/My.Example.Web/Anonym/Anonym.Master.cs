#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using NLog;


#endregion



namespace My.Example.Web.Anonym
{
    public partial class Anonym : System.Web.UI.MasterPage
    {
        static readonly Logger Logger = LogManager.GetLogger("AnonymMaster");


        protected void Page_Load(object sender, EventArgs e)
        {
            Logger.Info("sessionid {2}, host {0}, url {1}", Request.UserHostName, Request.RawUrl, Session.SessionID);
        }


        protected void Page_PreRender()
        {
            lCaption.Text = Page.Title;
            Page.Title = Config.GetPageName(Page.Title, null);
        }
    }
}