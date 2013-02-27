#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


#endregion



namespace My.Example.Web
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack) {}
        }


        protected void OnClick(object sender, EventArgs e)
        {
            cc.SetAsCount(15);
        }
    }
}