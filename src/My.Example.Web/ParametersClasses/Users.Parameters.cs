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
    public partial class Users
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
                LocateUserId = GetIntFromQueryString(_c, "LocateUserId", true);
                SearchString = GetStringFromQueryString(_c, "SearchString", true);
                RoleId = GetIntArFromQueryString(_c, "RoleId", true);
                RoleCanBeNull = GetBoolFromQueryString(_c, "RoleCanBeNull", true);
                CreatedDateBegin = GetDateTimeFromQueryString(_c, "CreatedDateBegin", true);
                CreatedDateEnd = GetDateTimeFromQueryString(_c, "CreatedDateEnd", true);
                IsActive = GetBoolFromQueryString(_c, "IsActive", true);
                ResetObjects();
            }
            public void ResetObjects()
            {

                FillAddon();
            }
            partial void FillAddon();
            // ReSharper restore PossibleInvalidOperationException
            // ReSharper restore AssignNullToNotNullAttribute


            public int? LocateUserId { get; set; }

            [CanBeNull]
            public string SearchString { get; set; }

            [CanBeNull]
            public int[] RoleId { get; set; }

            public bool? RoleCanBeNull { get; set; }

            public DateTime? CreatedDateBegin { get; set; }

            public DateTime? CreatedDateEnd { get; set; }

            public bool? IsActive { get; set; }

        }
    }
}
