#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using JetBrains.Annotations;

using My.Common.Web;
using My.Example.DAL;


#endregion



namespace My.Example.Web
{
    public class PageParametersBaseExample : PageParametersBase
    {
        [CanBeNull]
        protected UserDTO GetUserDTOFromQueryString([NotNull] HttpContext c, [NotNull] string name, [NotNull] Db db, bool canOmit)
        {
            int? id = GetIntFromQueryString(c, name, canOmit);
            if (id == null)
                return null;
            UserDTO u = db.FindUserDTOById(id.Value);
            if (u == null)
                RequestParamsException.Throw("В параметре {0} указан неправильный id пользователя ({1})", name, id.Value);
            return u;
        }
    }
}