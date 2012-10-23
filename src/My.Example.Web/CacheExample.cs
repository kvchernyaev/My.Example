#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

using My.Common;
using My.Example.DAL;


#endregion



namespace My.Example.Web
{
    public class CacheExample : CacheBase
    {
        protected override string DatabaseEntryName { get { return "a"; } }
        protected override string ConnectionStringName { get { return "My.Example_sa"; } }
        readonly Db _db;


        public CacheExample(Db db)
        {
            _db = db;
            Init();
        }


        protected override sealed void Init()
        {
            _data.Add(typeof (UserDTO), new Tuple<Func<int, object>, string[]>(_db.FindUserDTOById,
                                                                               new[]
                                                                               {
                                                                                       "Users",
                                                                                       "UsersByRoles",
                                                                               }));
        }


        public UserDTO FindUserCached(int uid)
        {
            return FindCached<UserDTO>(uid);
        }


        public AggregateCacheDependency GetUserAggregateCacheDependency()
        {
            return GetAggregateCacheDependency<UserDTO>();
        }
    }
}