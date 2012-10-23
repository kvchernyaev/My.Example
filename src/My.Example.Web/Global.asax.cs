#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using My.Common;

using NLog;


#endregion



namespace My.Example.Web
{
    public class Global : System.Web.HttpApplication
    {
        static readonly Logger Logger = LogManager.GetLogger("Global.asax");


        void Application_Start(object sender, EventArgs e)
        {
            ConfigExpressionBuilder.ConfigType = typeof (Config);
        }


        void Application_End(object sender, EventArgs e) {}


        void Application_Error(object sender, EventArgs e)
        {
            try
            {
                HttpApplication a = (HttpApplication) sender;
                if (a.Context == null)
                {
                    Exception ex = (Exception) typeof (HttpApplication).InvokeMember("_lastError", BindingFlags.DeclaredOnly | BindingFlags.NonPublic
                                                                                                   | BindingFlags.Instance | BindingFlags.GetField,
                                                                                     null, a, null);

                    Logger.FatalException("Application_Error with null context", ex);
                }
                else
                    foreach (Exception ex in a.Context.AllErrors)
                        Fatal(ex, a.Context, a.User.Identity);
            }
            catch (NullReferenceException ex)
            {
                Logger.ErrorException("Application_Error", ex);
            }
        }


        public static void Fatal(Exception ex, HttpContext c, IIdentity user)
        {
            Logger.FatalException("Application_Error:" + Environment.NewLine
                                  + "url " + c.Request.RawUrl + "," + Environment.NewLine
                                  + "request from " + c.Request.UserHostName + "," + Environment.NewLine
                                  + "IIS login " + c.Request.LogonUserIdentity.Name + "," + Environment.NewLine
                                  + "user login " + user.Name + Environment.NewLine,
                                  ex);
        }


        void Session_Start(object sender, EventArgs e) {}


        void Session_End()
        {
            Logger.Debug("Session_End");
            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session.SessionID != null)
            {
                Logger.Debug("Session_End, SessionID=" + HttpContext.Current.Session.SessionID);
                HttpContext.Current.Cache.Remove(HttpContext.Current.Session.SessionID + "LoginedUser");
            }

            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.
        }
    }
}