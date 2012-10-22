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
    public class UserDTOFinder : FinderBase<UserDTOFinder>
    {
        [CanBeNull] public bool? IsActive { get; set; }
        [CanBeNull] public DateTime? CreatedDateBegin { get; set; }
        [CanBeNull] public DateTime? CreatedDateEnd { get; set; }

        public bool SearchByNullRoles { get; set; }
        [CanBeNull] public List<int> Roles { get; set; }

        public bool IsEmpty { get {  return string.IsNullOrEmpty(SearchString) && IsActive==null && CreatedDateBegin==null && CreatedDateEnd==null && !SearchByNullRoles && (Roles==null || Roles.Count == 0);} }


        public override UserDTOFinder Clone()
        {
            UserDTOFinder rv = base.Clone();
            if (this.Roles != null)
                rv.Roles = new List<int>(this.Roles);
            return rv;
        }


        protected override bool InteriorEquals(UserDTOFinder other)
        {
            return 
                    (this.IsActive == other.IsActive)
                    && (this.CreatedDateBegin == other.CreatedDateBegin)
                    && (this.CreatedDateEnd == other.CreatedDateEnd)
                    && ((Roles == null || Roles.Count == 0) && (other.Roles == null || other.Roles.Count == 0) ||
                              Roles != null && other.Roles != null &&
                              Roles.Count == other.Roles.Count &&
                              (from x in Roles orderby x select x).Distinct()
                                 .SequenceEqual((from x in other.Roles orderby x select x).Distinct()))
                    && (this.SearchByNullRoles == other.SearchByNullRoles);
        }

// ReSharper restore InconsistentNaming
// ReSharper restore UnusedAutoPropertyAccessor.Global
// ReSharper restore MemberCanBePrivate.Global
    }
}
