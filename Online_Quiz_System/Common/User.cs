using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Online_Quiz_System.Models;
namespace Online_Quiz_System.Common
{
    public class User
    {
        public bool ISLOGIN { get; set; } = false;
        public int ID { get; set; }
        public int PERMISSION { get; set; }
        public string USERNAME { get; set; }
        public string EMAIL { get; set; }
        public string AVATAR { get; set; }
        public string NAME { get; set; }
        public int TESTCODE { get; set; } = 0;
        public string TIME { get; set; }

        public User()
        {
            try { 
            //    ISLOGIN = (bool)HttpContext.Current.Session[UserSession.ISLOGIN];
            //    ID = (int)HttpContext.Current.Session[UserSession.ID];
            //    PERMISSION = (int)HttpContext.Current.Session[UserSession.PERMISSION];
            //    USERNAME = (string)HttpContext.Current.Session[UserSession.USERNAME];
            //    EMAIL = (string)HttpContext.Current.Session[UserSession.EMAIL];
            //    AVATAR = (string)HttpContext.Current.Session[UserSession.AVATAR];
            //    NAME = (string)HttpContext.Current.Session[UserSession.NAME];
            //    TESTCODE = (int)HttpContext.Current.Session[UserSession.TESTCODE];
            //    TIME = (string)HttpContext.Current.Session[UserSession.TIME];

                 var session = HttpContext.Current.Session;
                if (session != null)
                {
                    ISLOGIN = session[UserSession.ISLOGIN] != null && (bool)session[UserSession.ISLOGIN];
                    ID = session[UserSession.ID] != null ? (int)session[UserSession.ID] : 0;
                    PERMISSION = session[UserSession.PERMISSION] != null ? (int)session[UserSession.PERMISSION] : 0;
                    USERNAME = session[UserSession.USERNAME] as string;
                    EMAIL = session[UserSession.EMAIL] as string;
                    AVATAR = session[UserSession.AVATAR] as string;
                    NAME = session[UserSession.NAME] as string;
                    TESTCODE = session[UserSession.TESTCODE] != null ? (int)session[UserSession.TESTCODE] : 0;
                    TIME = session[UserSession.TIME] as string;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public bool IsLogin()
        {
            try
            {
                if (ISLOGIN)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void Reset()
        {
            HttpContext.Current.Session.Clear();
        }
        public bool IsAdmin()
        {
            try
            {
                if (ISLOGIN && PERMISSION == 1)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool IsTeacher()
        {
            try
            {
                if (ISLOGIN && PERMISSION == 2)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool IsStudent()
        {
            try
            {
                if (ISLOGIN && PERMISSION == 3)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool IsTesting()
        {
            try
            {
                if (ISLOGIN && PERMISSION == 3 && TESTCODE > 0)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}