using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

using Fudantong.Infrastructure;

namespace Fudantong.Modules
{

    class UISLogin : ILogin
    {
        private HttpWebRequest hwr;
        private CookieContainer cc = new CookieContainer();

        private Thread thrLogin;
        private bool loginResult = false;
        private LoginCallback callback;
        private WebProxy pxy;

        private string username, password;

        private string response;

        public UISLogin()
        {
           
        }

        public string Response
        {
            get { return response; }
            set { response = value; }
        }

        public WebProxy Proxy
        {
            get { return pxy; }
            set { pxy = value; }
        }

        public CookieContainer Cookies
        {
            get { return cc; }
            set { cc = value; }
        }

        public bool Login(string Username, string Password)
        {
            username = Username; password = Password;
            login();
            return loginResult;
        }
        
        public void StartLogin(string Username, string Password, LoginCallback Callback)
        {
            username = Username; password = Password;
            callback = Callback;
            thrLogin = new Thread(new ThreadStart(login));
            thrLogin.Start();
        }

        private void login()
        {
            hwr = Common.Create("http://uis2.fudan.edu.cn/amserver/UI/Login/", "post",cc,pxy);
            using (StreamWriter sw = new StreamWriter(hwr.GetRequestStream(), Encoding.UTF8))
            {
                string strSubmit = "IDButton=Submit&IDToken0=&IDToken1={0}&IDToken2={1}&encoded=false&goto={2}&gx_charset=UTF-8&inputCode=";
                sw.Write(strSubmit, System.Web.HttpUtility.UrlEncode(username), System.Web.HttpUtility.UrlEncode(password), "http://www.urp.fudan.edu.cn:84/epstar/app/fudan/ScoreManger/ScoreViewer/Student/Course.jsp");
                sw.Flush();
            }

            try
            {
                using (HttpWebResponse wr = (HttpWebResponse)hwr.GetResponse())
                {
                    HttpStatusCode code = wr.StatusCode;
                    string header = wr.ResponseUri.ToString();
                    response = "";
                    using (StreamReader sr = new StreamReader(wr.GetResponseStream(), Encoding.UTF8))
                    {
                        response = sr.ReadToEnd();
                    }
                    if (header.ToLower().StartsWith("http://www.urp.fudan.edu.cn"))
                    {
                        loginResult = true;
                        if (callback != null) callback.Invoke(true);
                        return;
                    }
                }

            }
            catch (Exception)
            {
                throw new LoginConnectionException();
            }

            loginResult = false;
            if (callback != null) callback.Invoke(false);
        }
    }

    class LoginConnectionException : Exception { }
}
