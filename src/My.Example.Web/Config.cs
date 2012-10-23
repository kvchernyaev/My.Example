#region usings
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;

using JetBrains.Annotations;

using NLog;


#endregion



namespace My.Example.Web
{
    public static class Config
    {
        public static bool LogAjaxErrorsFromLocalhost { get { return GetBoolAppSetting("LogAjaxErrorsFromLocalhost", false); } }


        [UsedImplicitly]
        public static string DecimalValidatorRegex
        {
            get
            {
                return ConfigurationManager.AppSettings["DecimalValidatorRegex"]
                       ?? @"[\d\s\u00a0]+([\.,][\d\s\u00a0]*)?";
            }
        }

        /// <summary>
        ///     Секунды необязательны
        /// </summary>
        [UsedImplicitly]
        public static string DateTimeValidatorRegex
        {
            get
            {
                return ConfigurationManager.AppSettings["DateTimeValidatorRegex"]
                       ?? @"^((((31[\/\.](0?[13578]|1[02]))|((29|30)[\/\.](0?[1,3-9]|1[0-2])))[\/\.](1[6-9]|[2-9]\d)?\d{2})|(29[\/\.]0?2[\/\.](((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))|(0?[1-9]|1\d|2[0-8])[\/\.]((0?[1-9])|(1[0-2]))[\/\.]((1[6-9]|[2-9]\d)?\d{2}))\s+(20|21|22|23|[0-1]?\d):[0-5]?\d(:[0-5]?\d)?$";
            }
        }

        /// <summary>
        ///     Время необязательно
        /// </summary>
        [UsedImplicitly]
        public static string DateValidatorRegex
        {
            get
            {
                return ConfigurationManager.AppSettings["DateValidatorRegex"]
                       ?? @"^((((31[\/\.](0?[13578]|1[02]))|((29|30)[\/\.](0?[1,3-9]|1[0-2])))[\/\.](1[6-9]|[2-9]\d)?\d{2})|(29[\/\.]0?2[\/\.](((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))|(0?[1-9]|1\d|2[0-8])[\/\.]((0?[1-9])|(1[0-2]))[\/\.]((1[6-9]|[2-9]\d)?\d{2}))(\s+(20|21|22|23|[0-1]?\d):[0-5]?\d(:[0-5]?\d)?)?$";
            }
        }


        static string PageNamePrefix { get { return "DTO Example"; } }

        static readonly Logger Logger = LogManager.GetLogger("Config");


        public static string GetPageName(string caption, string userName)
        {
            string p = PageNamePrefix;
            return string.Format("{0}{1}{2}", string.IsNullOrEmpty(p) ? "" : p + " - ", caption, string.IsNullOrEmpty(userName) ? "" : " - " + userName);
        }


        public static Color GetRoleColor(int userRoleId)
        {
            if (userRoleId == 1)
                return Color.OrangeRed;
            if (userRoleId == 3)
                return Color.Blue;
            if (userRoleId == 2)
                return Color.Green;
            if (userRoleId == 4)
                return Color.DarkGreen;
            return Color.Black;
        }


        static bool GetBoolAppSetting(string name, bool def)
        {
            bool rv;
            return bool.TryParse(ConfigurationManager.AppSettings[name], out rv) ? rv : def;
        }


        static int GetIntAppSetting(string name, int def)
        {
            int rv;
            if (int.TryParse(ConfigurationManager.AppSettings[name], out rv))
                return rv;
            return def;
        }
    }
}