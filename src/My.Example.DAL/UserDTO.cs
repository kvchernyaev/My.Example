#region usings
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

using JetBrains.Annotations;
using My.Common.DAL;
using My.Common;
#endregion

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global


namespace My.Example.DAL
{
    /// <summary>
    ///      Updateable.  tableName=Users u. 
    /// </summary>
    [DebuggerDisplay("{Login} ({UserId})")]
    [Serializable]
    public class UserDTO : DtoDbBase<UserDTO>
    {
        public UserDTO(){}


        /// <summary>
        /// For insert
        /// </summary>
        public UserDTO([NotNull] string login, [NotNull] string passwordhash, [CanBeNull] string userfio, [CanBeNull] string telephone, [CanBeNull] string fax, [CanBeNull] string email, bool isactive, int? creatoruserid)
        {
            Login = login;
            PasswordHash = passwordhash;
            UserFio = userfio;
            Telephone = telephone;
            Fax = fax;
            Email = email;
            IsActive = isactive;
            CreatorUserId = creatoruserid;

        }


        /// <summary>
        ///      PK. 
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///      Updateable. 
        /// </summary>
        [NotNull]
        public string Login { get; set; }

        /// <summary>
        ///      Updateable. 
        /// </summary>
        [NotNull]
        public string PasswordHash { get; set; }

        /// <summary>
        ///      dbName=UserFIO.  Updateable. 
        /// </summary>
        [CanBeNull]
        public string UserFio { get; set; }

        /// <summary>
        ///      Updateable. 
        /// </summary>
        [CanBeNull]
        public string Telephone { get; set; }

        /// <summary>
        ///      Updateable. 
        /// </summary>
        [CanBeNull]
        public string Fax { get; set; }

        /// <summary>
        ///      Updateable. 
        /// </summary>
        [CanBeNull]
        public string Email { get; set; }

        /// <summary>
        ///      Updateable. 
        /// </summary>
        public bool IsActive { get; set; }

        public int? CreatorUserId { get; set; }

        public DateTime CreatedDate { get; set; }

        /// <summary>
        ///      Not from db. 
        /// </summary>
        public DateTime LastActivityDate { get; set; }

        /// <summary>
        ///      Not from db. 
        /// </summary>
        public DateTime LastPasswordChangeDate { get; set; }

        /// <summary>
        ///     Объединение RolesByUser и RolesByGroups. Not from db. 
        /// </summary>
        public List<UserRoleDTO> Roles { get; set; }

        protected override bool InteriorEquals(UserDTO other)
        {
            return this.UserId == other.UserId;
        }
        public override int GetHashCode()
        {
            return this.UserId.GetHashCode();
        }

// ReSharper restore InconsistentNaming
// ReSharper restore UnusedAutoPropertyAccessor.Global
// ReSharper restore MemberCanBePrivate.Global
    }
}
