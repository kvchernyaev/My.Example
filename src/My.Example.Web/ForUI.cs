#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using JetBrains.Annotations;

using My.Common;
using My.Example.DAL;


#endregion



namespace My.Example.Web
{
    public class ForUI : ForUICommon
    {
        public ForUI(string id) : base(id) {}
        public ForUI(int id) : base(id) {}


        public ForUI(string caption, string id) : base(caption, id) {}
        public ForUI(string caption, int id) : base(caption, id) {}
        public ForUI(string caption, string tooltip, string id) : base(caption, tooltip, id) {}
        public ForUI(string caption, string tooltip, int id) : base(caption, tooltip, id) {}

        #region business constructors
        public ForUI([NotNull] UserDTO u, For f)
                : this(u.UserId.ToString())
        {
            Init(f, () =>
                        {
                            Caption = u.UserFio;
                            ToolTip = string.Format("{0} ({1})", u.Login, u.UserId);
                        },
                 () => { Caption = string.Format("{0}", u.UserFio); });
        }


        public ForUI([NotNull] UserRoleDTO ur, For f)
                : this(ur.UserRoleId)
        {
            Init(f, () =>
                        {
                            Caption = ur.Name;
                            ToolTip = ur.Description.TrimOrNullIfEmpty();
                            ForeColor = ur.Color();
                        },
                 () =>
                     {
                         string desc = ur.Description.TrimOrNullIfEmpty();
                         Caption = string.Format("{0}{1}", ur.Name, string.IsNullOrEmpty(desc) ? "" : " (" + desc + ")");
                     });
        }
        #endregion
    }
}