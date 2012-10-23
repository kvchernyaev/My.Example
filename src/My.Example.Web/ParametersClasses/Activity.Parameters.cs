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
    public partial class Activity
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
                SearchString = GetStringFromQueryString(_c, "SearchString", true);
                FilterUserStr = GetStringFromQueryString(_c, "FilterUserStr", true);
                FilterUsers = GetIntArFromQueryString(_c, "FilterUsers", true);
                FilterUserAny = GetBoolFromQueryString(_c, "FilterUserAny", true);
                FilterExcludeMe = GetBoolFromQueryString(_c, "FilterExcludeMe", true);
                FilterDateAny = GetBoolFromQueryString(_c, "FilterDateAny", true);
                FilterDateBegin = GetDateTimeFromQueryString(_c, "FilterDateBegin", true);
                FilterDateEnd = GetDateTimeFromQueryString(_c, "FilterDateEnd", true);
                IsPostBack = GetStringFromQueryString(_c, "IsPostBack", true);
                ResetObjects();
            }
            public void ResetObjects()
            {

                FillAddon();
            }
            partial void FillAddon();
            // ReSharper restore PossibleInvalidOperationException
            // ReSharper restore AssignNullToNotNullAttribute


            [CanBeNull]
            public string SearchString { get; set; }

            [CanBeNull]
            public string FilterUserStr { get; set; }

            [CanBeNull]
            public int[] FilterUsers { get; set; }

            public bool? FilterUserAny { get; set; }

            public bool? FilterExcludeMe { get; set; }

            public bool? FilterDateAny { get; set; }

            public DateTime? FilterDateBegin { get; set; }

            public DateTime? FilterDateEnd { get; set; }

            [CanBeNull]
            public string IsPostBack { get; set; }

        }
    }
}
