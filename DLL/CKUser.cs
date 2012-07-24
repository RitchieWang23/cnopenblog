using System;
using System.Collections.Generic;
using System.Text;
using System.Web.SessionState;

namespace DLL
{
    public class CKUser
    {
        /// <summary>
        /// ��ȡ��ǰ�û���
        /// </summary>
        public static string Username
        {
            get { return Tools.DecryptDES(Cookie.GetCookie("username")).Replace("'", ""); }
        }
        /// <summary>
        /// ��ȡ��ǰ�û�����
        /// </summary>
        public static string Fullname
        {
            get { return Cookie.GetCookie("fullname"); }
        }
        /// <summary>
        /// ��ȡ��ǰ�û�email
        /// </summary>
        public static string Email
        {
            get { return Cookie.GetCookie("email"); }
        }
        /// <summary>
        /// �ж��û��Ƿ��¼
        /// </summary>
        public static bool IsLogin
        {
            get { return Username != ""; }
        }

        /// <summary>
        /// д���û���Ϣ��cookie
        /// </summary>
        /// <param name="username">�û���</param>
        /// <param name="fullname">����</param>
        /// <param name="email">email</param>
        /// <param name="remember">�Ƿ��ס</param>
        public static void Login(string username, string fullname, string email, bool remember)
        {
            username = Tools.EncryptDES(username);
            if (remember)
            {
                DateTime expires = DateTime.Now.AddYears(1);
                Cookie.SetCookie("username", username, expires);
                Cookie.SetCookie("fullname", fullname, expires);
                Cookie.SetCookie("email", email, expires);
            }
            else
            {
                Cookie.SetCookie("username", username);
                Cookie.SetCookie("fullname", fullname);
                Cookie.SetCookie("email", email);
            }
        }

        public static void Logout()
        {
            Cookie.RemoveCookie("username");
            Cookie.RemoveCookie("fullname");
            Cookie.RemoveCookie("email");
        }
    }
}
