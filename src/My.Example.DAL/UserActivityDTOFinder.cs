#region usings
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

using JetBrains.Annotations;
using My.Common.DAL;
using My.Common;
#endregion

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global


namespace My.Example.DAL
{
    [Serializable]
    [DebuggerDisplay("IsEmpty={IsEmpty}, SearchString=[{SearchString}]")]
    public class UserActivityDTOFinder : FinderBase<UserActivityDTOFinder>
    {
        [CanBeNull] public List<int> UserId { get; set; }
        [CanBeNull] public bool? IsChangePsw { get; set; }
        [CanBeNull] public DateTime? CreatedDateBegin { get; set; }
        [CanBeNull] public DateTime? CreatedDateEnd { get; set; }
        [CanBeNull] public bool? IsPostBack { get; set; }
        [CanBeNull] public List<int?> ImpersonatedByUserId { get; set; }

        [CanBeNull] public List<int> NotUserId { get; set; }

        public bool IsEmpty { get {  return string.IsNullOrEmpty(SearchString) && (UserId==null || UserId.Count == 0) && IsChangePsw==null && CreatedDateBegin==null && CreatedDateEnd==null && IsPostBack==null && (ImpersonatedByUserId==null || ImpersonatedByUserId.Count == 0) && (NotUserId==null || NotUserId.Count == 0);} }


        public override UserActivityDTOFinder Clone()
        {
            UserActivityDTOFinder rv = base.Clone();
            if (this.UserId != null)
                rv.UserId = new List<int>(this.UserId);
            if (this.ImpersonatedByUserId != null)
                rv.ImpersonatedByUserId = new List<int?>(this.ImpersonatedByUserId);
            if (this.NotUserId != null)
                rv.NotUserId = new List<int>(this.NotUserId);
            return rv;
        }


        protected override bool InteriorEquals(UserActivityDTOFinder other)
        {
            return 
                    ((UserId == null || UserId.Count == 0) && (other.UserId == null || other.UserId.Count == 0) ||
                              UserId != null && other.UserId != null &&
                              UserId.Count == other.UserId.Count &&
                              (from x in UserId orderby x select x).Distinct()
                                .SequenceEqual((from x in other.UserId orderby x select x).Distinct()))
                    && (this.IsChangePsw == other.IsChangePsw)
                    && (this.CreatedDateBegin == other.CreatedDateBegin)
                    && (this.CreatedDateEnd == other.CreatedDateEnd)
                    && (this.IsPostBack == other.IsPostBack)
                    && ((ImpersonatedByUserId == null || ImpersonatedByUserId.Count == 0) && (other.ImpersonatedByUserId == null || other.ImpersonatedByUserId.Count == 0) ||
                              ImpersonatedByUserId != null && other.ImpersonatedByUserId != null &&
                              ImpersonatedByUserId.Count == other.ImpersonatedByUserId.Count &&
                              (from x in ImpersonatedByUserId orderby x select x).Distinct()
                                .SequenceEqual((from x in other.ImpersonatedByUserId orderby x select x).Distinct()))
                    && ((NotUserId == null || NotUserId.Count == 0) && (other.NotUserId == null || other.NotUserId.Count == 0) ||
                              NotUserId != null && other.NotUserId != null &&
                              NotUserId.Count == other.NotUserId.Count &&
                              (from x in NotUserId orderby x select x).Distinct()
                                 .SequenceEqual((from x in other.NotUserId orderby x select x).Distinct()));
        }

// ReSharper restore InconsistentNaming
// ReSharper restore UnusedAutoPropertyAccessor.Global
// ReSharper restore MemberCanBePrivate.Global
    }
}
