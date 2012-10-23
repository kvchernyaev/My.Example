#region usings
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using My.Common;


#endregion



namespace My.Example.DAL
{
    public partial class Db
    {
        #region users support
        const string FindUserDTOSelectTemplate = @"
                    select u.* from dbo.Users u where {0}
                    -- roles
                    select ur.* from dbo.UserRoles ur join dbo.UsersByRoles ubr on ur.UserRoleId = ubr.UserRoleId 
                        join dbo.Users u on ubr.UserId=u.UserId 
                        where {0}
;

select top 1 ua.CreatedDate from UserActivities ua join dbo.Users u on ua.UserId = u.UserId where {0} and ua.IsChangePsw=0 order by ua.CreatedDate desc;
select top 1 ua.CreatedDate from UserActivities ua join dbo.Users u on ua.UserId = u.UserId where {0} and ua.IsChangePsw=1 order by ua.CreatedDate desc;
                    ";


        [CanBeNull]
        UserDTO ExecUserDTOCustomOne([NotNull] SqlCommand com)
        {
            using (SqlDataReader r = com.ExecuteReader())
                if (r.Read())
                {
                    UserDTO u = ReadUserDTO(r);
                    r.NextResult();
                    u.Roles = ReadUserRoleDTOList(r);

                    r.NextResult();
                    if (r.Read())
                        u.LastActivityDate = (DateTime) r["CreatedDate"];
                    r.NextResult();
                    if (r.Read())
                        u.LastPasswordChangeDate = (DateTime) r["CreatedDate"];
                    return u;
                }
            return null;
        }


        [NotNull]
        public UserDTO InsertNewUser([NotNull] UserDTO u, bool needCustomSelect)
        {
            UserDTO i = this.Insert(u, false);

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                if (u.Roles != null)
                    foreach (UserRoleDTO r in u.Roles)
                        ExecuteNonQuery(new Q(@"insert into dbo.UsersByRoles(UserId, UserRoleId, CreatorUserId)values(@UserId, @UserRoleId, @CreatorUserId)",
                                              new P("UserId", i.UserId),
                                              new P("UserRoleId", r.UserRoleId),
                                              new P("CreatorUserId", u.CreatorUserId)
                                                ), con);
            }

            return needCustomSelect ? FindUserDTOById(i.UserId) : i;
        }


        public UserDTO UpdateUser([NotNull] UserDTOUpdater uu,
                                  [CanBeNull] List<UserRoleDTO> roles, [NotNull] UserDTO updater)
        {
            UserDTO current = Update(uu, true);

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                if (roles != null)
                {
                    IEnumerable<UserRoleDTO> toDelete = current.Roles.Where(curr => !roles.Exists(mustg => mustg.UserRoleId == curr.UserRoleId));
                    string toDelRoles = string.Join(", ", toDelete.Select(r => r.UserRoleId.ToString(CultureInfo.InvariantCulture)));
                    if (!string.IsNullOrEmpty(toDelRoles))
                        ExecuteNonQuery(new Q(@"delete from dbo.UsersByRoles where UserId=@UserId and UserroleId in (" + toDelRoles + ")",
                                              new P("UserId", uu.UserId)), con);

                    foreach (UserRoleDTO r in roles.Where(mustr => !current.Roles.Exists(curr => curr.UserRoleId == mustr.UserRoleId)))
                        ExecuteNonQuery(new Q(@"insert into dbo.UsersByRoles(UserId, UserRoleId, CreatorUserId)values(@UserId, @UserRoleId, @CreatorUserId)",
                                              new P("UserId", uu.UserId),
                                              new P("UserRoleId", r.UserRoleId),
                                              new P("CreatorUserId", updater.UserId)), con);
                }
            }

            return FindUserDTOById(current.UserId);
        }


        static partial void GetWhereClauseAddon(UserDTOFinder f, List<string> wheres, SqlCommand com)
        {
            if (f.Roles != null && f.Roles.Count > 0 || f.SearchByNullRoles)
            {
                List<string> w = new List<string>(2);
                if (f.Roles != null && f.Roles.Count > 0)
                    w.Add(string.Format(@" exists(select * from dbo.UsersByRoles ubr where ubr.UserId=u.UserId and ubr.UserRoleId in ({0}) ) ",
                                        string.Join(", ", f.Roles)));

                if (f.SearchByNullRoles)
                    w.Add("not exists(select * from dbo.UsersByRoles ubr where ubr.UserId=u.UserId)");
                wheres.Add(string.Join(" or ", w));
            }
        }


        public void DeleteUserDeep(int uid)
        {
            try
            {
                DeleteUserDTODeep(uid);
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("FK_"))
                    throw new MyException(ex.Message);
                throw;
            }
        }
        #endregion

        static partial void GetWhereClauseAddon(UserActivityDTOFinder f, List<string> wheres, SqlCommand com)
        {
            if (f.UserIdNotIn != null && f.UserIdNotIn.Count > 0)
                wheres.Add(@" ua.UserId not in (" + string.Join(", ", f.UserIdNotIn) + ") ");
        }
    }
}