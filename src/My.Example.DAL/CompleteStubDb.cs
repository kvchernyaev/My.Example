#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

using My.Common.DAL;

#endregion

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global


namespace My.Example.DAL
{
    public partial class CompleteStubDb : DbBase, IDb
    {

        #region UserRoleDTO methods
        [NotNull]
        public virtual List<UserRoleDTO> GetAllUserRoleDTO()
        { throw new NotImplementedException(); }
        public virtual int? FindUserRoleDTOPageIndex(int pageSize, int userroleid_, string orderBy = null)
        { throw new NotImplementedException(); }
        #endregion


        #region UserDTO methods
        [CanBeNull] public virtual UserDTO FindUserDTOById(int userid_)
        { throw new NotImplementedException(); }
        [NotNull]
        public virtual UserDTO Update([NotNull] UserDTOUpdater xu, bool needCustomSelect = true)
        { throw new NotImplementedException(); }
        [NotNull]
        public virtual List<UserDTO> Update([NotNull] IEnumerable<UserDTOUpdater> xu, bool needCustomSelect = true)
        { throw new NotImplementedException(); }
        [CanBeNull]
        public virtual UserDTO FindUserDTO([NotNull] string login)
        { throw new NotImplementedException(); }
        [CanBeNull]
        public virtual UserDTO FindUserDTO([NotNull] string login, [NotNull] string passwordhash)
        { throw new NotImplementedException(); }
        [NotNull]
        public virtual List<UserDTO> FindUserDTOByLoginOrEmail([NotNull] string login, [CanBeNull] string email)
        { throw new NotImplementedException(); }
        /// <summary>
        ///     For gridviews with paging
        /// </summary>
        public virtual int FindUserDTOCount([CanBeNull] UserDTOFinder f)
        { throw new NotImplementedException(); }
        [NotNull]
        public virtual List<UserDTO> FindUserDTOs([CanBeNull] UserDTOFinder f, string orderBy = null, int startIndex = 0, int count = int.MaxValue, bool needCustomSelect = false)
        { throw new NotImplementedException(); }
        public virtual int? FindUserDTOPageIndex([CanBeNull] UserDTOFinder f, int pageSize, int userid_, string orderBy = null)
        { throw new NotImplementedException(); }
        [NotNull]
        public virtual UserDTO Insert([NotNull] UserDTO x, bool needCustomSelect = false)
        { throw new NotImplementedException(); }
        [NotNull]
        public virtual List<UserDTO> Insert([NotNull] IEnumerable<UserDTO> x, bool needCustomSelect = false)
        { throw new NotImplementedException(); }
        public virtual int DeleteUserDTODeep(int userid)
        { throw new NotImplementedException(); }
        #endregion


        #region UserActivityDTO methods
        /// <summary>
        ///     For gridviews with paging
        /// </summary>
        public virtual int FindUserActivityDTOCount([CanBeNull] UserActivityDTOFinder f)
        { throw new NotImplementedException(); }
        [NotNull]
        public virtual List<UserActivityDTO> FindUserActivityDTOs([CanBeNull] UserActivityDTOFinder f, string orderBy = null, int startIndex = 0, int count = int.MaxValue)
        { throw new NotImplementedException(); }
        public virtual int? FindUserActivityDTOPageIndex([CanBeNull] UserActivityDTOFinder f, int pageSize, int useractivityid_, string orderBy = null)
        { throw new NotImplementedException(); }
        [NotNull]
        public virtual UserActivityDTO Insert([NotNull] UserActivityDTO x)
        { throw new NotImplementedException(); }
        [NotNull]
        public virtual List<UserActivityDTO> Insert([NotNull] IEnumerable<UserActivityDTO> x)
        { throw new NotImplementedException(); }
        #endregion



    }

}

// ReSharper restore InconsistentNaming
// ReSharper restore UnusedAutoPropertyAccessor.Global
// ReSharper restore MemberCanBePrivate.Global
// ReSharper restore UnusedMemberInSuper.Global
// ReSharper restore UnusedMember.Global
// ReSharper restore UnusedMethodReturnValue.Global

