using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Fudantong.Modules
{
    delegate void LoginCallback(bool result);

    interface ILogin
    {
        CookieContainer Cookies { get; set; }
        WebProxy Proxy { get; set; }
        bool Login(string Username, string Password);
        void StartLogin(string Username, string Password, LoginCallback Callback);
    }

}
