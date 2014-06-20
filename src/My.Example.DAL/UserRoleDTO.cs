






















#region usings
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml.Serialization;

using JetBrains.Annotations;

using My.Common.DAL;

#endregion

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global


namespace My.Example.DAL
{
    /// <summary>
    ///      tableName=UserRoles ur. 
    /// </summary>
    [DebuggerDisplay("{Name} ({UserRoleId})")]
    [Serializable]
    public partial class UserRoleDTO : DtoDbBase<UserRoleDTO>
    {
        /// <summary>
        ///      PK. 
        /// </summary>
        [NotNull]
        public int UserRoleId { get; set; }

        [NotNull]
        public string Name { get; set; }

        [CanBeNull]
        public string Description { get; set; }

        protected override bool InteriorEquals(UserRoleDTO other)
        {
            return this.UserRoleId == other.UserRoleId;
        }
        public override int GetHashCode()
        {
            return this.UserRoleId.GetHashCode();
        }

// ReSharper restore InconsistentNaming
// ReSharper restore UnusedAutoPropertyAccessor.Global
// ReSharper restore MemberCanBePrivate.Global
    }
}
