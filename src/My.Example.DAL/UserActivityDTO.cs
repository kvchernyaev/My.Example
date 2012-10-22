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
    ///      tableName=UserActivities ua. 
    /// </summary>
    [DebuggerDisplay("UserId={UserId}, RawUrl={RawUrl}, Browser={Browser}")]
    [Serializable]
    public class UserActivityDTO : DtoDbBase<UserActivityDTO>
    {
        public UserActivityDTO(){}


        /// <summary>
        /// For insert
        /// </summary>
        public UserActivityDTO(int userid, bool ischangepsw, [CanBeNull] string rawurl, [CanBeNull] string browser, [CanBeNull] string userhostaddress, bool ispostback, int? impersonatedbyuserid)
        {
            UserId = userid;
            IsChangePsw = ischangepsw;
            RawUrl = rawurl;
            Browser = browser;
            UserHostAddress = userhostaddress;
            IsPostBack = ispostback;
            ImpersonatedByUserId = impersonatedbyuserid;

        }


        /// <summary>
        ///      PK. 
        /// </summary>
        public int UserActivityId { get; set; }

        public int UserId { get; set; }

        public bool IsChangePsw { get; set; }

        public DateTime CreatedDate { get; set; }

        [CanBeNull]
        public string RawUrl { get; set; }

        [CanBeNull]
        public string Browser { get; set; }

        [CanBeNull]
        public string UserHostAddress { get; set; }

        public bool IsPostBack { get; set; }

        public int? ImpersonatedByUserId { get; set; }

        protected override bool InteriorEquals(UserActivityDTO other)
        {
            return this.UserActivityId == other.UserActivityId;
        }
        public override int GetHashCode()
        {
            return this.UserActivityId.GetHashCode();
        }

// ReSharper restore InconsistentNaming
// ReSharper restore UnusedAutoPropertyAccessor.Global
// ReSharper restore MemberCanBePrivate.Global
    }
}
