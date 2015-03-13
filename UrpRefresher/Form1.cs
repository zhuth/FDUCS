using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using Fudantong.Infrastructure;
using Fudantong.Modules;

namespace UrpRefresher
{
    public partial class Form1 : Form
    {
        Config cfg = new Config("config.xml");
            
        public Form1()
        {
            InitializeComponent();

            if (cfg["username"] == "" || cfg["username"].StartsWith("["))
            {
                MessageBox.Show("请修改 config.xml 文件，给定你的学号和 URP 密码。");
                cfg["username"] = cfg["password"] = "[请在此输入]";
                cfg.Save();
                Environment.Exit(0);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            doUpdate();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            cfg.Save();
            Environment.Exit(0);
        }

        private void Form1_MinimumSizeChanged(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            doUpdate();
        }

        private void doUpdate()
        {
            CookieContainer cc = new CookieContainer();
            UISLogin uis = new UISLogin();
            uis.Cookies = cc;
            if (!uis.Login(cfg["username"], cfg["password"]))
            {
                MessageBox.Show("登录到 URP 失败。");
            }
            Regex reg = new Regex(@"<img [^>]*>", RegexOptions.Multiline | RegexOptions.Compiled);
            webBrowser1.Navigate("about:blank");
            webBrowser1.Document.OpenNew(true);
            webBrowser1.Document.Write(reg.Replace(uis.Response, ""));
        }
    }
}
