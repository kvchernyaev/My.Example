#region usings
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

using JetBrains.Annotations;

using My.Common;
using My.Example.DAL;

using NLog;


#endregion



namespace My.Example.Web
{
    public static class UsersFacade
    {
        static readonly Logger Logger = LogManager.GetLogger("UsersFacade");
        static readonly Db Db = new Db();


        [CanBeNull]
        public static UserDTO Auth([NotNull] string login, [NotNull] string psw)
        {
            string hash = ComputeHash(psw);
            return Db.FindUserDTO(login, hash);
        }


        public static bool CheckPsw([NotNull] UserDTO loginedUser, [NotNull] string psw)
        {
            string hash = ComputeHash(psw);
            return string.Compare(loginedUser.PasswordHash, hash, StringComparison.OrdinalIgnoreCase) == 0;
        }


        [CanBeNull]
        public static List<UserDTO> SearchByLoginOrEmail([NotNull] string login, [NotNull] string email)
        {
            if (string.IsNullOrEmpty(email))
                return Db.FindUserDTOByLoginOrEmail(login, email);
            return Db.FindUserDTOByLoginOrEmail(login, email)
                    .Where(u => (u.Email ?? "").Split(new[] {";", ","},
                                                      StringSplitOptions.RemoveEmptyEntries)
                                        .Any(s => s.Trim().ToLower() == email.Trim().ToLower())).ToList();
        }


        [NotNull]
        public static UserDTO Add([NotNull] UserDTO newUser, [NotNull] string psw, bool needCustomSelect)
        {
            newUser.PasswordHash = ComputeHash(psw);
            UserDTO newu = Db.InsertNewUser(newUser, needCustomSelect);
            Logger.Info("inserted new user, id=" + newu.UserId);
            return newu;
        }


        public static UserDTO ChangePassword([NotNull] UserDTO u, [NotNull] string newPsw, [CanBeNull] UserDTO byWhom)
        {
            string hash = ComputeHash(newPsw);
            Logger.Warn("change psw: {0} ({1}), byWhom {4} ({5}), hash was {2} new {3}", u.Login, u.UserId, u.PasswordHash, hash,
                        byWhom == null ? "-" : byWhom.Login, byWhom == null ? "-" : byWhom.UserId.ToString(CultureInfo.InvariantCulture));
            u = Db.Update(new UserDTOUpdater(u.UserId) {PasswordHash = hash});
            Db.Insert(new UserActivityDTO(u.UserId, true, null, null, null, true));
            return u;
        }


        public static UserDTO UpdateUser([NotNull] UserDTOUpdater uu, [NotNull] List<UserRoleDTO> roles, UserDTO updater)
        {
            if (uu.UserId == 1 && !roles.Exists(r => r.UserRoleId == 1))
                throw new MyException(string.Format("Нельзя у пользователя с UserId=1 убрать роль Суперадмин"));
            if (uu.UserId == 1 && !uu.IsActive)
                throw new MyException(string.Format("Нельзя пользователя с UserId=1 сделать неактивным"));
            if (!updater.Roles.Exists(r => r.UserRoleId == 1) && roles.Exists(r => r.UserRoleId == 1))
                throw new MyException(string.Format("Нельзя делать пользователя суперадминистратором, не являясь таким"));

            Logger.Info("updating user: " + uu.ChangedAsString);
            return Db.UpdateUser(uu, roles, updater);
        }


        static readonly HashAlgorithm Crypto = SHA256.Create();


        [NotNull]
        static string ComputeHash([NotNull] string message, string salt = null)
        {
            if (message == "")
                return "";

            if (!string.IsNullOrEmpty(salt))
                message += salt;
            byte[] sourceBytes = Encoding.UTF8.GetBytes(message);
            byte[] hashBytes = Crypto.ComputeHash(sourceBytes);

            StringBuilder sb = new StringBuilder();
            foreach (byte t in hashBytes)
                sb.AppendFormat("{0:x2}", t);

            return sb.ToString();
        }
    }
}