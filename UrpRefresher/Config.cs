using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.Configuration;
using System.Runtime.Serialization.Formatters.Binary;

namespace Fudantong.Infrastructure
{

    /// <summary>
    /// 将一个 XML 文档用来存储配置信息。
    /// </summary>
    public class Config
    {
        XmlDocument doc;
        string fileName;
        string rootName;

        /// <summary>
        /// 从文件载入 XML 文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="rootName">XML 的根节点</param>
        public Config(string fileName, string rootName)
        {
            this.fileName = fileName;
            this.rootName = rootName;

            doc = new XmlDocument();

            try
            {
                doc.Load(fileName);
            }
            catch (System.IO.FileNotFoundException)
            {
                CreateSettingsDocument();
            }
        }

        /// <summary>
        /// 从文件载入，使用默认根节点 Settings
        /// </summary>
        /// <param name="fileName"></param>
        public Config(string fileName)
            : this(fileName, "Settings")
        {
        }

        public IEnumerable<string> Keys
        {
            get
            {
                XmlNode root = doc.DocumentElement;
                XmlNode s = root.SelectSingleNode('/' + rootName + '/' + "fudantong");

                if (s != null)
                {
                    foreach (XmlNode n in s.ChildNodes)
                    {
                        yield return n.Name;
                    }
                }
            
            }
        }

        /// <summary>
        /// 初始化设置文档并建立根节点。
        /// </summary>
        protected void CreateSettingsDocument()
        {
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", null, null));
            doc.AppendChild(doc.CreateElement(rootName));
        }

        /// <summary>
        /// 将 XML 文档写入文件。
        /// </summary>
        public void Flush()
        {
            try
            {
                doc.Save(fileName);
            }
            catch (Exception)
            {
                throw new Exception("bad format.");
            }
        }

        /// <summary>
        /// 同上。
        /// </summary>
        public void Save()
        {
            Flush();
        }

        /// <summary>
        /// 读取或设置设置项的内容
        /// </summary>
        /// <param name="key">键值名</param>
        /// <returns>键值</returns>
        public string this[string key]
        {
            get
            {
                return ReadString("fudantong", key.ToLower(), "");
            }
            set
            {
                WriteString("fudantong", key.ToLower(), value);
            }
        }

        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <param name="section">节段</param>
        /// <param name="name">名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public string ReadString(string section, string name, string defaultValue)
        {
            XmlNode root = doc.DocumentElement;
            XmlNode s = root.SelectSingleNode('/' + rootName + '/' + section);

            if (s == null)
                return defaultValue;  //not found

            XmlNode n = s.SelectSingleNode(name);

            if (n == null)
                return defaultValue;  //not found

            XmlAttributeCollection attrs = n.Attributes;

            foreach (XmlAttribute attr in attrs)
            {
                if (attr.Name == "value")
                    return attr.Value;
            }

            return defaultValue;
        }

        /// <summary>
        /// 写入字符串
        /// </summary>
        /// <param name="section">节点</param>
        /// <param name="name">键值名</param>
        /// <param name="value">键值</param>
        public void WriteString(string section, string name, string value)
        {
            XmlNode root = doc.DocumentElement;
            XmlNode s = root.SelectSingleNode('/' + rootName + '/' + section);

            if (s == null)
                s = root.AppendChild(doc.CreateElement(section));

            XmlNode n = s.SelectSingleNode(name);

            if (n == null)
                n = s.AppendChild(doc.CreateElement(name));

            XmlAttribute attr = ((XmlElement)n).SetAttributeNode("value", "");
            attr.Value = value;
        }

    }
}
