
#region usings
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using JetBrains.Annotations;

using My.Example.DAL;

#endregion


namespace My.Example.Web
{
    public abstract partial class PageBaseExample<TParameters>
    {
        #region select all methods
        #region UserRoleDTO
        public static void SelectUserRoleDTOsAddonEx([NotNull]List<UserRoleDTO> l){ l.ForEach(x=>SelectUserRoleDTOsAddon(x));}
        public static void SelectUserRoleDTOsAddonEx(UserRoleDTO x){SelectUserRoleDTOsAddon(x);}
        static partial void SelectUserRoleDTOsAddon(UserRoleDTO x);
        static Dictionary<int, UserRoleDTO> _userRoleDTOs;
        [NotNull]
        public static Dictionary<int, UserRoleDTO> UserRoleDTOs
        {
            get
            {
                if (_userRoleDTOs == null)
                {
                    List<UserRoleDTO> l = Db.GetAllUserRoleDTO();
                    SelectUserRoleDTOsAddonEx(l);
                    _userRoleDTOs = l.ToDictionary(x => x.UserRoleId);
                }
                return _userRoleDTOs;
            }
        }
        public static List<UserRoleDTO> SelectAllUserRoleDTOs()
        {
            List<UserRoleDTO> rv = UserRoleDTOs.Values.ToList(); 
            return rv;
        }
        public static ForUI[] SelectAllUserRoleDTOsForCbl()
        {
            List<UserRoleDTO> l = UserRoleDTOs.Values.ToList();
            ForUI[] rv = l.Select(x => new ForUI(x, ForUI.For.Cbl)).ToArray();
            return rv;
        }
        public static ForUI[] SelectAllUserRoleDTOsForDdl()
        {
            List<UserRoleDTO> l = UserRoleDTOs.Values.ToList();
            ForUI[] rv = l.Select(x => new ForUI(x, ForUI.For.Ddl)).ToArray();
            return rv;
        }
        #endregion


        #endregion
    }
}
