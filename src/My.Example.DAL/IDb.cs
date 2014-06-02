#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global


namespace My.Example.DAL
{
    public interface IDb : IDbUserAddon
    {
        string ConnectionString { get; }



        #region UserRoleDTO methods
        [NotNull]
        List<UserRoleDTO> GetAllUserRoleDTO();
        int? FindUserRoleDTOPageIndex(int pageSize, int userroleid_, string orderBy = null);
        #endregion


        #region UserDTO methods
        [CanBeNull] UserDTO FindUserDTOById(int userid_);
        [NotNull]
        UserDTO Update([NotNull] UserDTOUpdater xu, bool needCustomSelect = true);
        [NotNull]
        List<UserDTO> Update([NotNull] IEnumerable<UserDTOUpdater> xu, bool needCustomSelect = true);
        [CanBeNull]
        UserDTO FindUserDTO([NotNull] string login);
        [CanBeNull]
        UserDTO FindUserDTO([NotNull] string login, [NotNull] string passwordhash);
        [NotNull]
        List<UserDTO> FindUserDTOByLoginOrEmail([NotNull] string login, [CanBeNull] string email);
        /// <summary>
        ///     For gridviews with paging
        /// </summary>
        int FindUserDTOCount([CanBeNull] UserDTOFinder f);
        [NotNull]
        List<UserDTO> FindUserDTOs([CanBeNull] UserDTOFinder f, string orderBy = null, int startIndex = 0, int count = int.MaxValue, bool needCustomSelect = false);
        int? FindUserDTOPageIndex([CanBeNull] UserDTOFinder f, int pageSize, int userid_, string orderBy = null);
        [NotNull]
        UserDTO Insert([NotNull] UserDTO x, bool needCustomSelect = false);
        [NotNull]
        List<UserDTO> Insert([NotNull] IEnumerable<UserDTO> x, bool needCustomSelect = false);
        int DeleteUserDTODeep(int userid);
        #endregion


        #region UserActivityDTO methods
        /// <summary>
        ///     For gridviews with paging
        /// </summary>
        int FindUserActivityDTOCount([CanBeNull] UserActivityDTOFinder f);
        [NotNull]
        List<UserActivityDTO> FindUserActivityDTOs([CanBeNull] UserActivityDTOFinder f, string orderBy = null, int startIndex = 0, int count = int.MaxValue);
        int? FindUserActivityDTOPageIndex([CanBeNull] UserActivityDTOFinder f, int pageSize, int useractivityid_, string orderBy = null);
        [NotNull]
        UserActivityDTO Insert([NotNull] UserActivityDTO x);
        [NotNull]
        List<UserActivityDTO> Insert([NotNull] IEnumerable<UserActivityDTO> x);
        #endregion



    }

}

// ReSharper restore InconsistentNaming
// ReSharper restore UnusedAutoPropertyAccessor.Global
// ReSharper restore MemberCanBePrivate.Global
// ReSharper restore UnusedMemberInSuper.Global
// ReSharper restore UnusedMember.Global
// ReSharper restore UnusedMethodReturnValue.Global

