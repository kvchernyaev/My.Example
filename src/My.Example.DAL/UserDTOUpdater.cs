#region usings
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

using JetBrains.Annotations;
using My.Common.DAL;
#endregion

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global


namespace My.Example.DAL
{
    [Serializable]
    public class UserDTOUpdater : UpdateBase
    {
        public UserDTOUpdater(int userid_)
        {
            UserId = userid_;
        }


        
        /// <summary>
        ///      PK. 
        /// </summary>
		[NotNull]
        public int UserId { get; private set; }

        [NotNull]
        string _login;
        [NotNull]
        public string Login { get { return _login; } set { 
                if (value == null)
                    throw new ArgumentNullException("Login");
                Changed["Login"] = _login = value; } }


        [NotNull]
        string _passwordHash;
        [NotNull]
        public string PasswordHash { get { return _passwordHash; } set { 
                if (value == null)
                    throw new ArgumentNullException("PasswordHash");
                Changed["PasswordHash"] = _passwordHash = value; } }


        [CanBeNull]
        string _userFio;
        /// <summary>
        ///      dbName=UserFIO. 
        /// </summary>
        [CanBeNull]
        public string UserFio { get { return _userFio; } set { Changed["UserFIO"] = _userFio = value; } }


        [CanBeNull]
        string _telephone;
        [CanBeNull]
        public string Telephone { get { return _telephone; } set { Changed["Telephone"] = _telephone = value; } }


        [CanBeNull]
        string _fax;
        [CanBeNull]
        public string Fax { get { return _fax; } set { Changed["Fax"] = _fax = value; } }


        [CanBeNull]
        string _email;
        [CanBeNull]
        public string Email { get { return _email; } set { Changed["Email"] = _email = value; } }


        bool _isActive;
        public bool IsActive { get { return _isActive; } set { Changed["IsActive"] = _isActive = value; } }


        /// <summary>
        ///     Returns IsEmpty value after operation
        /// </summary>
        public bool ExcludeEquals([NotNull] UserDTO x)
        {
            if (Changed.ContainsKey("Login") && _login == x.Login)
                Changed.Remove("Login");
            if (Changed.ContainsKey("PasswordHash") && _passwordHash == x.PasswordHash)
                Changed.Remove("PasswordHash");
            if (Changed.ContainsKey("UserFIO") && _userFio == x.UserFio)
                Changed.Remove("UserFIO");
            if (Changed.ContainsKey("Telephone") && _telephone == x.Telephone)
                Changed.Remove("Telephone");
            if (Changed.ContainsKey("Fax") && _fax == x.Fax)
                Changed.Remove("Fax");
            if (Changed.ContainsKey("Email") && _email == x.Email)
                Changed.Remove("Email");
            if (Changed.ContainsKey("IsActive") && _isActive == x.IsActive)
                Changed.Remove("IsActive");
            return this.IsEmpty;
        }

// ReSharper restore InconsistentNaming
// ReSharper restore UnusedAutoPropertyAccessor.Global
// ReSharper restore MemberCanBePrivate.Global
    }
}
