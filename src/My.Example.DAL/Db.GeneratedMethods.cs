#region usings
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
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
    public partial class Db : DbBase
    {
        public Db(string connectionStringName = null)
        {
            ConnectionString = string.IsNullOrEmpty(connectionStringName) ? ConfigurationManager.ConnectionStrings["My.Example"].ConnectionString
                 :  ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            Init();
        }
        /// <summary>ConnectionString is already configured. Here you can init CommandTimeout field.</summary>
        partial void Init();


        #region UserRoleDTO methods
        [NotNull]
        static UserRoleDTO ReadUserRoleDTO([NotNull] IDataRecord r)
        {
            UserRoleDTO x = new UserRoleDTO();
            x.UserRoleId = (int) r["UserRoleId"];
            x.Name = (string) r["Name"];
            x.Description = GetNullableRef<string>(r["Description"]);
            return x;
        }
        [NotNull]
        static List<UserRoleDTO> ReadUserRoleDTOList([NotNull] IDataReader r)
        {
            List<UserRoleDTO> l = new List<UserRoleDTO>();
            while (r.Read())
                l.Add(ReadUserRoleDTO(r));
            return l;
        }
        [CanBeNull]
        static UserRoleDTO ExecUserRoleDTOOne([NotNull] SqlCommand com)
        {
            UserRoleDTO rv = null;
            using (SqlDataReader r = com.ExecuteReader())
                if (r.Read())
                    rv = ReadUserRoleDTO(r);
            return rv;
        }
        [NotNull]
        static List<UserRoleDTO> ExecUserRoleDTOList([NotNull] SqlCommand com)
        {
            using (SqlDataReader r = com.ExecuteReader())
                return ReadUserRoleDTOList(r);
        }
        static KeyValuePair<string, object>[] GetUserRoleDTOTableIdValues(int userroleid_)
        { return new[] { new KeyValuePair<string, object>("UserRoleId", userroleid_) }; }
        [NotNull]
        public List<UserRoleDTO> GetAllUserRoleDTO()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand com = PrepareCommand(con))
            {
                com.CommandText = @"select * from dbo.[UserRoles]  order by UserRoleId desc";

                List<UserRoleDTO> rv = ExecUserRoleDTOList(com);

                return rv;
            }
        }
        public int? FindUserRoleDTOPageIndex(int pageSize, int userroleid_, string orderBy = null)
        {
            if (string.IsNullOrEmpty(orderBy)) orderBy = "[UserRoleId]";
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand com = PrepareCommand(con))
            {
                com.CommandText = string.Format(@"select r/@pageSize from (
                    select *, cast(row_number() over(order by {0}) as int) r
                    from dbo.[UserRoles] ur
                    )t where t.[UserRoleId]=@UserRoleId", orderBy);

                com.Parameters.AddWithValue("UserRoleId", userroleid_);
                
                com.Parameters.AddWithValue("pageSize", pageSize);
                object o = com.ExecuteScalar();
                return o==null? (int?)null: (int?)(int)o;
            }
        }
        #endregion


        #region UserDTO methods
        [NotNull]
        static UserDTO ReadUserDTO([NotNull] IDataRecord r)
        {
            UserDTO x = new UserDTO();
            x.UserId = (int) r["UserId"];
            x.Login = (string) r["Login"];
            x.PasswordHash = (string) r["PasswordHash"];
            x.UserFio = GetNullableRef<string>(r["UserFIO"]);
            x.Telephone = GetNullableRef<string>(r["Telephone"]);
            x.Fax = GetNullableRef<string>(r["Fax"]);
            x.Email = GetNullableRef<string>(r["Email"]);
            x.IsActive = (bool) r["IsActive"];
            x.CreatorUserId = GetNullableVal<int>(r["CreatorUserId"]);
            x.CreatedDate = (DateTime) r["CreatedDate"];
            return x;
        }
        [NotNull]
        static List<UserDTO> ReadUserDTOList([NotNull] IDataReader r)
        {
            List<UserDTO> l = new List<UserDTO>();
            while (r.Read())
                l.Add(ReadUserDTO(r));
            return l;
        }
        [CanBeNull]
        static UserDTO ExecUserDTOOne([NotNull] SqlCommand com)
        {
            UserDTO rv = null;
            using (SqlDataReader r = com.ExecuteReader())
                if (r.Read())
                    rv = ReadUserDTO(r);
            return rv;
        }
        [NotNull]
        static List<UserDTO> ExecUserDTOList([NotNull] SqlCommand com)
        {
            using (SqlDataReader r = com.ExecuteReader())
                return ReadUserDTOList(r);
        }
        static KeyValuePair<string, object>[] GetUserDTOTableIdValues(int userid_)
        { return new[] { new KeyValuePair<string, object>("UserId", userid_) }; }
        [CanBeNull]
        public UserDTO FindUserDTOById(int userid_)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand com = PrepareCommand(con))
            {
                com.CommandText = string.Format(FindUserDTOSelectTemplate, @" u.[UserId]=@UserId ");
                com.Parameters.AddWithValue("UserId", userid_);

                return ExecUserDTOCustomOne(com);
            }
        }
        [NotNull]
        public UserDTO Update([NotNull] UserDTOUpdater xu, bool needCustomSelect = true)
        {
            if (xu.UserId == default(int))
                throw new Exception("Update UserDTO: pk in UserDTOUpdater object is not initialized");

            UserDTO x = Update(xu, "dbo.[Users]", GetUserDTOTableIdValues( userid_:xu.UserId), ReadUserDTO);

            if (x == null)
                throw new Exception("Update UserDTO returns empty - no such PK"); // can not be in reality
            UserDTO rv = needCustomSelect ? FindUserDTOById(userid_:x.UserId) : x;
            UpdateAddon(xu);
            return rv;
        }
        partial void UpdateAddon([NotNull]UserDTOUpdater xu);
        [NotNull]
        public List<UserDTO> Update([NotNull] IEnumerable<UserDTOUpdater> xu, bool needCustomSelect = true)
        {
            List<UpdateBase> uArr = new List<UpdateBase>();
            List<IEnumerable<KeyValuePair<string, object>>> pkFiltersArr = new List<IEnumerable<KeyValuePair<string, object>>>();
            foreach (UserDTOUpdater u in xu)
            {
                if (u.UserId == default(int))
                    throw new Exception("Update UserDTO: userid_:u.UserId in UserDTOUpdater object is not initialized");
                uArr.Add(u);
                pkFiltersArr.Add(GetUserDTOTableIdValues(userid_:u.UserId));
            }
            List<UserDTO> x = Update(uArr, "dbo.[Users]", pkFiltersArr, ReadUserDTO);

            if (x == null)
                throw new Exception("Update UserDTO returns empty - no such PK"); // can not be in reality
            List<UserDTO> rv = needCustomSelect ? x.Select(t => FindUserDTOById(userid_:t.UserId)).ToList() : x;
            foreach (UserDTOUpdater xui in xu) UpdateAddon(xui);
            return rv;
        }
        [CanBeNull]
        public UserDTO FindUserDTO([NotNull] string login)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand com = PrepareCommand(con, ct:CommandType.Text))
            {
                com.CommandText = string.Format(FindUserDTOSelectTemplate, @"(u.[Login] is null and @Login is null or u.[Login] = @Login)");
                com.Parameters.AddWithValue("Login", GetNullableParamVal(login));

                return ExecUserDTOCustomOne(com);
            }
        }
        [CanBeNull]
        public UserDTO FindUserDTO([NotNull] string login, [NotNull] string passwordhash)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand com = PrepareCommand(con, ct:CommandType.Text))
            {
                com.CommandText = string.Format(FindUserDTOSelectTemplate, @"(u.[Login] is null and @Login is null or u.[Login] = @Login) and (u.[PasswordHash] is null and @PasswordHash is null or u.[PasswordHash] = @PasswordHash)");
                com.Parameters.AddWithValue("Login", GetNullableParamVal(login));
                com.Parameters.AddWithValue("PasswordHash", GetNullableParamVal(passwordhash));

                return ExecUserDTOCustomOne(com);
            }
        }
        [NotNull]
        public List<UserDTO> FindUserDTOByLoginOrEmail([NotNull] string login, [CanBeNull] string email)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand com = PrepareCommand(con, ct:CommandType.Text))
            {
                com.CommandText = @" select u.* from Users u where u.Login=@Login or @Email is not null and u.Email like '%'+@Email+'%' ";
                com.Parameters.AddWithValue("Login", GetNullableParamVal(login));
                com.Parameters.AddWithValue("Email", GetNullableParamVal(email));

                List<UserDTO> l = ExecUserDTOList(com);
                return l.Select(x => FindUserDTOById(userid_:x.UserId)).ToList();
            }
        }
        static string GetWhereClause([CanBeNull]UserDTOFinder f, [NotNull] SqlCommand com)
        {
            if (f == null || f.IsEmpty) return null;
            if (f.SearchString != null && (f.SearchString = f.SearchString.Trim()) == "") f.SearchString = null;

            List<string> wheres = new List<string>();
            if (!string.IsNullOrEmpty(f.SearchString))
            {
                List<string> ors = new List<string>();
                ors.Add(@" u.Login like @like or u.UserFIO like @like or u.Telephone like @like or u.Fax like @like or u.Email like @like or @intProposal is not null and u.UserId=@intProposal " );                 
                GetWhereClauseOrsAddon(f, ors, com);
                string orsS = ors == null ? null : string.Join(" or ", ors.Select(x=>"("+x+")"));
                if(!string.IsNullOrEmpty(orsS))
                {
                    wheres.Add(orsS);
                    com.Parameters.AddWithValue("like", f.SearchString == null ? (object) DBNull.Value : "%" + f.SearchString + "%");
                    int idProp;
                    com.Parameters.AddWithValue("intProposal", f.SearchString == null || !int.TryParse(f.SearchString, out idProp) ? DBNull.Value : (object) idProp);
                }
            }
            if (f.IsActive.HasValue) wheres.Add(" u.IsActive=@IsActive ");
            if (f.CreatedDateBegin.HasValue) wheres.Add(@" u.[CreatedDate]>= @CreatedDateBegin ");
            if (f.CreatedDateEnd.HasValue) wheres.Add(@" u.[CreatedDate]<= @CreatedDateEnd ");


            com.Parameters.AddWithValue("IsActive", GetNullableParamVal(f.IsActive));
            com.Parameters.AddWithValue("CreatedDateBegin", GetNullableParamVal(f.CreatedDateBegin));
            com.Parameters.AddWithValue("CreatedDateEnd", GetNullableParamVal(f.CreatedDateEnd));
            GetWhereClauseAddon(f, wheres, com);

            string whereClause = null;
            if (wheres.Count > 0)
                whereClause = " where " + string.Join(" and ", wheres.Select(w=> "("+w+")"));
            return whereClause;
        }
        static partial void GetWhereClauseOrsAddon([NotNull] UserDTOFinder f, [NotNull]List<string> ors, [NotNull]SqlCommand com);
        static partial void GetWhereClauseAddon([NotNull] UserDTOFinder f, [NotNull]List<string> wheres, [NotNull]SqlCommand com);
        /// <summary>
        ///     For gridviews with paging
        /// </summary>
        public int FindUserDTOCount([CanBeNull] UserDTOFinder f)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand com = PrepareCommand(con))
            {
                string whereClause = GetWhereClause(f, com);
                com.CommandText = @" select cast( count(*) as int) from dbo.[Users] u " + whereClause;

                int count = (int) com.ExecuteScalar();
                return count;
            }
        }
        [NotNull]
        public List<UserDTO> FindUserDTOs([CanBeNull] UserDTOFinder f, string orderBy = null, int startIndex = 0, int count = int.MaxValue, bool needCustomSelect = false)
        {
            if (string.IsNullOrEmpty(orderBy)) orderBy = "[UserId]";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand com = PrepareCommand(con))
            {
                string whereClause = GetWhereClause(f, com);
                if(f==null) f = new UserDTOFinder();
                com.CommandText = string.Format(startIndex == 0 && count == int.MaxValue
                                                        ? @" select u.* from dbo.[Users] u {1} order by {0} "
                                                        : @" select * from (
                                                                select u.*, row_number() over(order by {0}) r from dbo.[Users] u 
                                                                 {1}
                                                            ) t where r >= @startIndex and r < @startIndex + @count
                                                            order by {0}", orderBy, whereClause);

                com.Parameters.AddWithValue("startIndex", startIndex + 1);
                com.Parameters.AddWithValue("count", count);

                List<UserDTO> rv = ExecUserDTOList(com);
                if(needCustomSelect) rv = rv.Select(x => FindUserDTOById(userid_:x.UserId)).ToList();
                return rv;
            }
        }
        public int? FindUserDTOPageIndex([CanBeNull] UserDTOFinder f, int pageSize, int userid_, string orderBy = null)
        {
            if (string.IsNullOrEmpty(orderBy)) orderBy = "[UserId]";
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand com = PrepareCommand(con))
            {
                string whereClause = GetWhereClause(f, com);
                if (f == null) f = new UserDTOFinder();
                com.CommandText = string.Format(@"select r/@pageSize from (
                    select *, cast(row_number() over(order by {0}) as int) r
                    from dbo.[Users] u
                     {1}
                    )t where t.[UserId]=@UserId", orderBy, whereClause);

                com.Parameters.AddWithValue("UserId", userid_);
                
                com.Parameters.AddWithValue("pageSize", pageSize);
                object o = com.ExecuteScalar();
                return o==null? (int?)null: (int?)(int)o;
            }
        }
        [NotNull]
        public UserDTO Insert([NotNull] UserDTO x, bool needCustomSelect = false)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand com = PrepareCommand(con))
            {
                com.CommandText = @"declare @id as table ([UserId] int); insert into dbo.[Users]
                                ([Login], [PasswordHash], [UserFIO], [Telephone], [Fax], [Email], [IsActive], [CreatorUserId]) 
                               output inserted.[UserId] into @id values
                                (@Login, @PasswordHash, @UserFio, @Telephone, @Fax, @Email, @IsActive, @CreatorUserId);select t.* from dbo.[Users] t join @id i on t.[UserId]=i.[UserId]  ";
                  
                com.Parameters.AddWithValue("Login", x.Login);
                com.Parameters.AddWithValue("PasswordHash", x.PasswordHash);
                com.Parameters.AddWithValue("UserFio", GetNullableParamVal(x.UserFio));
                com.Parameters.AddWithValue("Telephone", GetNullableParamVal(x.Telephone));
                com.Parameters.AddWithValue("Fax", GetNullableParamVal(x.Fax));
                com.Parameters.AddWithValue("Email", GetNullableParamVal(x.Email));
                com.Parameters.AddWithValue("IsActive", x.IsActive);
                com.Parameters.AddWithValue("CreatorUserId", GetNullableParamVal(x.CreatorUserId));

                UserDTO ins = ExecUserDTOOne(com);
                UserDTO rv = needCustomSelect ? FindUserDTOById(userid_:ins.UserId) : ins;
                InsertAddon(rv);
                return rv;
            }
        }
        partial void InsertAddon([NotNull]UserDTO t);

        [NotNull]
        public List<UserDTO> Insert([NotNull] IEnumerable<UserDTO> x, bool needCustomSelect = false)
        {
            using(SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand com = PrepareCommand(con))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"insert into dbo.[Users]
                            ([Login], [PasswordHash], [UserFIO], [Telephone], [Fax], [Email], [IsActive], [CreatorUserId]) 
                            output inserted.*");
                int counter = 0;
                foreach (UserDTO xi in x)
                {
                    sb.Append(string.Format(@" select @Login{0}, @PasswordHash{0}, @UserFio{0}, @Telephone{0}, @Fax{0}, @Email{0}, @IsActive{0}, @CreatorUserId{0}
                              union all", counter));
                    com.Parameters.AddWithValue(string.Format("Login{0}", counter), xi.Login);
                    com.Parameters.AddWithValue(string.Format("PasswordHash{0}", counter), xi.PasswordHash);
                    com.Parameters.AddWithValue(string.Format("UserFio{0}", counter), GetNullableParamVal(xi.UserFio));
                    com.Parameters.AddWithValue(string.Format("Telephone{0}", counter), GetNullableParamVal(xi.Telephone));
                    com.Parameters.AddWithValue(string.Format("Fax{0}", counter), GetNullableParamVal(xi.Fax));
                    com.Parameters.AddWithValue(string.Format("Email{0}", counter), GetNullableParamVal(xi.Email));
                    com.Parameters.AddWithValue(string.Format("IsActive{0}", counter), xi.IsActive);
                    com.Parameters.AddWithValue(string.Format("CreatorUserId{0}", counter), GetNullableParamVal(xi.CreatorUserId));

                    counter++;
                }
                if(counter == 0)
                    return new List<UserDTO>();
                sb.Remove(sb.Length - 9, 9);
                com.CommandText = sb.ToString();
                  
                List<UserDTO> ins = ExecUserDTOList(com);
                List<UserDTO> rv =  needCustomSelect ? ins.Select(t =>FindUserDTOById(userid_:t.UserId)).ToList() : ins;
                rv.ForEach(xx=>InsertAddon(xx));
                return rv;
            }
        }
        public int DeleteUserDTODeep(int userid)
        {
            using(SqlConnection con = new SqlConnection(ConnectionString))
            using(SqlCommand com = PrepareCommand(con, ct:CommandType.Text))
            {
                com.CommandText = @"delete from UsersByRoles where UserId=@UserId
                delete from UserActivities where UserId=@UserId
                delete from Users where UserId=@UserId";
                com.Parameters.AddWithValue("UserId", GetNullableParamVal((object)userid));
                return com.ExecuteNonQuery();
            }
        }
        #endregion


        #region UserActivityDTO methods
        [NotNull]
        static UserActivityDTO ReadUserActivityDTO([NotNull] IDataRecord r)
        {
            UserActivityDTO x = new UserActivityDTO();
            x.UserActivityId = (int) r["UserActivityId"];
            x.UserId = (int) r["UserId"];
            x.IsChangePsw = (bool) r["IsChangePsw"];
            x.CreatedDate = (DateTime) r["CreatedDate"];
            x.RawUrl = GetNullableRef<string>(r["RawUrl"]);
            x.Browser = GetNullableRef<string>(r["Browser"]);
            x.UserHostAddress = GetNullableRef<string>(r["UserHostAddress"]);
            x.IsPostBack = (bool) r["IsPostBack"];
            x.ImpersonatedByUserId = GetNullableVal<int>(r["ImpersonatedByUserId"]);
            return x;
        }
        [NotNull]
        static List<UserActivityDTO> ReadUserActivityDTOList([NotNull] IDataReader r)
        {
            List<UserActivityDTO> l = new List<UserActivityDTO>();
            while (r.Read())
                l.Add(ReadUserActivityDTO(r));
            return l;
        }
        [CanBeNull]
        static UserActivityDTO ExecUserActivityDTOOne([NotNull] SqlCommand com)
        {
            UserActivityDTO rv = null;
            using (SqlDataReader r = com.ExecuteReader())
                if (r.Read())
                    rv = ReadUserActivityDTO(r);
            return rv;
        }
        [NotNull]
        static List<UserActivityDTO> ExecUserActivityDTOList([NotNull] SqlCommand com)
        {
            using (SqlDataReader r = com.ExecuteReader())
                return ReadUserActivityDTOList(r);
        }
        static KeyValuePair<string, object>[] GetUserActivityDTOTableIdValues(int useractivityid_)
        { return new[] { new KeyValuePair<string, object>("UserActivityId", useractivityid_) }; }
        static string GetWhereClause([CanBeNull]UserActivityDTOFinder f, [NotNull] SqlCommand com)
        {
            if (f == null || f.IsEmpty) return null;
            if (f.SearchString != null && (f.SearchString = f.SearchString.Trim()) == "") f.SearchString = null;

            List<string> wheres = new List<string>();
            if (!string.IsNullOrEmpty(f.SearchString))
            {
                List<string> ors = new List<string>();
                ors.Add(@" ua.RawUrl like @like or ua.Browser like @like or ua.UserHostAddress like @like " );                 
                GetWhereClauseOrsAddon(f, ors, com);
                string orsS = ors == null ? null : string.Join(" or ", ors.Select(x=>"("+x+")"));
                if(!string.IsNullOrEmpty(orsS))
                {
                    wheres.Add(orsS);
                    com.Parameters.AddWithValue("like", f.SearchString == null ? (object) DBNull.Value : "%" + f.SearchString + "%");
                    int idProp;
                    com.Parameters.AddWithValue("intProposal", f.SearchString == null || !int.TryParse(f.SearchString, out idProp) ? DBNull.Value : (object) idProp);
                }
            }
            if (f.IsChangePsw.HasValue) wheres.Add(" ua.IsChangePsw=@IsChangePsw ");
            if (f.IsPostBack.HasValue) wheres.Add(" ua.IsPostBack=@IsPostBack ");
            if (f.CreatedDateBegin.HasValue) wheres.Add(@" ua.[CreatedDate]>= @CreatedDateBegin ");
            if (f.CreatedDateEnd.HasValue) wheres.Add(@" ua.[CreatedDate]<= @CreatedDateEnd ");
            if (f.UserId != null && f.UserId.Count > 0)
                wheres.Add(string.Format(" ua.[UserId] in ({0}) ", string.Join(",", f.UserId) ));
            if (f.ImpersonatedByUserId != null && f.ImpersonatedByUserId.Count > 0)
                wheres.Add( (f.ImpersonatedByUserId.Any(x=>x!=null) ? string.Format(" ua.[ImpersonatedByUserId] in ({0}) ", string.Join(",", f.ImpersonatedByUserId.Where(x=>x!=null) )) :"" )+ (f.ImpersonatedByUserId.Any(x => x == null)? (f.ImpersonatedByUserId.Any(x=>x!=null)?" or ":"") + " ua.[ImpersonatedByUserId] is null ":""  ));

            com.Parameters.AddWithValue("IsChangePsw", GetNullableParamVal(f.IsChangePsw));
            com.Parameters.AddWithValue("IsPostBack", GetNullableParamVal(f.IsPostBack));
            com.Parameters.AddWithValue("CreatedDateBegin", GetNullableParamVal(f.CreatedDateBegin));
            com.Parameters.AddWithValue("CreatedDateEnd", GetNullableParamVal(f.CreatedDateEnd));
            GetWhereClauseAddon(f, wheres, com);

            string whereClause = null;
            if (wheres.Count > 0)
                whereClause = " where " + string.Join(" and ", wheres.Select(w=> "("+w+")"));
            return whereClause;
        }
        static partial void GetWhereClauseOrsAddon([NotNull] UserActivityDTOFinder f, [NotNull]List<string> ors, [NotNull]SqlCommand com);
        static partial void GetWhereClauseAddon([NotNull] UserActivityDTOFinder f, [NotNull]List<string> wheres, [NotNull]SqlCommand com);
        /// <summary>
        ///     For gridviews with paging
        /// </summary>
        public int FindUserActivityDTOCount([CanBeNull] UserActivityDTOFinder f)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand com = PrepareCommand(con))
            {
                string whereClause = GetWhereClause(f, com);
                com.CommandText = @" select cast( count(*) as int) from dbo.[UserActivities] ua " + whereClause;

                int count = (int) com.ExecuteScalar();
                return count;
            }
        }
        [NotNull]
        public List<UserActivityDTO> FindUserActivityDTOs([CanBeNull] UserActivityDTOFinder f, string orderBy = null, int startIndex = 0, int count = int.MaxValue)
        {
            if (string.IsNullOrEmpty(orderBy)) orderBy = "[UserActivityId]";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand com = PrepareCommand(con))
            {
                string whereClause = GetWhereClause(f, com);
                if(f==null) f = new UserActivityDTOFinder();
                com.CommandText = string.Format(startIndex == 0 && count == int.MaxValue
                                                        ? @" select ua.* from dbo.[UserActivities] ua {1} order by {0} "
                                                        : @" select * from (
                                                                select ua.*, row_number() over(order by {0}) r from dbo.[UserActivities] ua 
                                                                 {1}
                                                            ) t where r >= @startIndex and r < @startIndex + @count
                                                            order by {0}", orderBy, whereClause);

                com.Parameters.AddWithValue("startIndex", startIndex + 1);
                com.Parameters.AddWithValue("count", count);

                List<UserActivityDTO> rv = ExecUserActivityDTOList(com);

                return rv;
            }
        }
        public int? FindUserActivityDTOPageIndex([CanBeNull] UserActivityDTOFinder f, int pageSize, int useractivityid_, string orderBy = null)
        {
            if (string.IsNullOrEmpty(orderBy)) orderBy = "[UserActivityId]";
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand com = PrepareCommand(con))
            {
                string whereClause = GetWhereClause(f, com);
                if (f == null) f = new UserActivityDTOFinder();
                com.CommandText = string.Format(@"select r/@pageSize from (
                    select *, cast(row_number() over(order by {0}) as int) r
                    from dbo.[UserActivities] ua
                     {1}
                    )t where t.[UserActivityId]=@UserActivityId", orderBy, whereClause);

                com.Parameters.AddWithValue("UserActivityId", useractivityid_);
                
                com.Parameters.AddWithValue("pageSize", pageSize);
                object o = com.ExecuteScalar();
                return o==null? (int?)null: (int?)(int)o;
            }
        }
        [NotNull]
        public UserActivityDTO Insert([NotNull] UserActivityDTO x)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand com = PrepareCommand(con))
            {
                com.CommandText = @"declare @id as table ([UserActivityId] int); insert into dbo.[UserActivities]
                                ([UserId], [IsChangePsw], [RawUrl], [Browser], [UserHostAddress], [IsPostBack], [ImpersonatedByUserId]) 
                               output inserted.[UserActivityId] into @id values
                                (@UserId, @IsChangePsw, @RawUrl, @Browser, @UserHostAddress, @IsPostBack, @ImpersonatedByUserId);select t.* from dbo.[UserActivities] t join @id i on t.[UserActivityId]=i.[UserActivityId]  ";
                  
                com.Parameters.AddWithValue("UserId", x.UserId);
                com.Parameters.AddWithValue("IsChangePsw", x.IsChangePsw);
                com.Parameters.AddWithValue("RawUrl", GetNullableParamVal(x.RawUrl));
                com.Parameters.AddWithValue("Browser", GetNullableParamVal(x.Browser));
                com.Parameters.AddWithValue("UserHostAddress", GetNullableParamVal(x.UserHostAddress));
                com.Parameters.AddWithValue("IsPostBack", x.IsPostBack);
                com.Parameters.AddWithValue("ImpersonatedByUserId", GetNullableParamVal(x.ImpersonatedByUserId));

                UserActivityDTO ins = ExecUserActivityDTOOne(com);
                InsertAddon(ins);
                return ins;
            }
        }
        partial void InsertAddon([NotNull]UserActivityDTO t);

        [NotNull]
        public List<UserActivityDTO> Insert([NotNull] IEnumerable<UserActivityDTO> x)
        {
            using(SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand com = PrepareCommand(con))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"insert into dbo.[UserActivities]
                            ([UserId], [IsChangePsw], [RawUrl], [Browser], [UserHostAddress], [IsPostBack], [ImpersonatedByUserId]) 
                            output inserted.*");
                int counter = 0;
                foreach (UserActivityDTO xi in x)
                {
                    sb.Append(string.Format(@" select @UserId{0}, @IsChangePsw{0}, @RawUrl{0}, @Browser{0}, @UserHostAddress{0}, @IsPostBack{0}, @ImpersonatedByUserId{0}
                              union all", counter));
                    com.Parameters.AddWithValue(string.Format("UserId{0}", counter), xi.UserId);
                    com.Parameters.AddWithValue(string.Format("IsChangePsw{0}", counter), xi.IsChangePsw);
                    com.Parameters.AddWithValue(string.Format("RawUrl{0}", counter), GetNullableParamVal(xi.RawUrl));
                    com.Parameters.AddWithValue(string.Format("Browser{0}", counter), GetNullableParamVal(xi.Browser));
                    com.Parameters.AddWithValue(string.Format("UserHostAddress{0}", counter), GetNullableParamVal(xi.UserHostAddress));
                    com.Parameters.AddWithValue(string.Format("IsPostBack{0}", counter), xi.IsPostBack);
                    com.Parameters.AddWithValue(string.Format("ImpersonatedByUserId{0}", counter), GetNullableParamVal(xi.ImpersonatedByUserId));

                    counter++;
                }
                if(counter == 0)
                    return new List<UserActivityDTO>();
                sb.Remove(sb.Length - 9, 9);
                com.CommandText = sb.ToString();
                  
                List<UserActivityDTO> ins = ExecUserActivityDTOList(com);
                ins.ForEach(xx=>InsertAddon(xx));
                return ins;
            }
        }
        #endregion



// ReSharper restore InconsistentNaming
// ReSharper restore UnusedAutoPropertyAccessor.Global
// ReSharper restore MemberCanBePrivate.Global
    }
}
