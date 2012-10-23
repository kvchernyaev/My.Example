
#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using JetBrains.Annotations;

using My.Example.DAL;
using My.Common.Web;


#endregion



namespace My.Example.Web
{
    public partial class EditUser
    {
        [Serializable]
        [UsedImplicitly]
        public partial class Parameters : PageParametersBaseExample
        {
            [NotNull] readonly HttpContext _c;
            [NotNull] readonly Db _db;        
            

            public Parameters([NotNull] HttpContext c, [NotNull] Db db)
            {
                _c = c;
                _db = db;

                Set();
            }


            // ReSharper disable PossibleInvalidOperationException
            // ReSharper disable AssignNullToNotNullAttribute
            void Set()
            {
                UserId = GetIntFromQueryString(_c, "UserId", false).Value;
                ResetObjects();
            }
            public void ResetObjects()
            {
                CurrentUser = GetUserDTOFromQueryString(_c, "UserId", _db, false);
                FillAddon();
            }
            partial void FillAddon();
            // ReSharper restore PossibleInvalidOperationException
            // ReSharper restore AssignNullToNotNullAttribute


            public int UserId { get; set; }

            [NotNull]
            public UserDTO CurrentUser { get; set; }

        }
    }
}
