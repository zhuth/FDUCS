using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Linq;

namespace phpClassCreator
{
    class Program
    {
        static XDocument xdSqlKeywords = XDocument.Load("sql.xml");

        static void Main(string[] args)
        {
            Regex regDeSpace = new Regex(@"([\r\n]|\s+)", RegexOptions.Compiled);
            Regex regDeComment = new Regex(@"(\/\*.*\*\/|--.*[\r\n])", RegexOptions.Compiled);

            if (args.Length == 0) return;
            string filename = args[0],
                outphp = args[0].Substring(0, args[0].LastIndexOf('.')) + ".schema.php";

            bool g = args.Contains("g"), s = args.Contains("s"); 

            string pContent = File.ReadAllText(filename);
            pContent = regDeComment.Replace(pContent, " ");
            pContent = regDeSpace.Replace(pContent, " ");
            pContent = regDeSpace.Replace(pContent, " ");

            StreamWriter swPhp = new StreamWriter(outphp, false);
            generateCode(swPhp, pContent, g, s);
            swPhp.Close();

        }

        private static void generateCode(StreamWriter swPhp, string sourceSQL, bool g, bool s)
        {
            string[] lines = sourceSQL.Replace("\r\n", "").Split(';');

            swPhp.WriteLine("<?php\r\n");

            bool firstTable = true;
            string tname = "", tpkey = "id"; int ix = -1;
            List<string> colname = new List<string>(), coltype = new List<string>();
            bool[] ai = new bool[1024];

            foreach (string tmp in lines)
            {
                string line = tmp.Trim();
                if (string.IsNullOrEmpty(line)) continue;
                if (!line.ToUpper().StartsWith("CREATE TABLE")) continue;

                if (!firstTable)
                {
                    ix = colname.IndexOf(tpkey);
                    if (ix < 0) ix = ai.Length - 1; 
                    generateCode(swPhp, tname, tpkey, ai[ix], g, s, ref colname, ref coltype);
                }
                else
                    firstTable = false;

                colname.Clear(); coltype.Clear(); ai = new bool[1024];

                line = line.Substring("CREATE TABLE ".Length);
                if (line.IndexOf('(') >= 0) tname = line.Substring(0, line.IndexOf('(')); else continue;
                if (tname.IndexOf('.') >= 0) tname = tname.Substring(tname.IndexOf('.') + 1);
                tname = demodify(tname.Trim());
                tpkey = "id";

                line = line.Substring(line.IndexOf('(') + 1);
                line = line.Substring(0, line.LastIndexOf(')'));
                string[] tokens = line.Split(',');
                for (int i = 0; i < tokens.Length; ++i)
                {
                    string token = tokens[i].Trim();
                    if (token.ToUpper().StartsWith("PRIMARY KEY"))
                    {
                        tpkey = demodify(demodify(token.Substring(token.LastIndexOf(' '))));
                    }
                    else
                    {
                        string[] cols = token.Split(" ".ToCharArray(), 3);
                        if (cols.Length < 2) continue;
                        if (validColName(cols[0]))
                        {
                            if (token.ToUpper().Contains("AUTO_INCREMENT")) ai[colname.Count] = true;
                            colname.Add(demodify(cols[0])); coltype.Add(demodify(cols[1]));
                        }
                    }
                }

            }

            ix = colname.IndexOf(tpkey); if (ix < 0) ix = ai.Length - 1;
            generateCode(swPhp, tname, tpkey, ai[ix], g,s, ref colname, ref coltype);
            swPhp.Write("?>");
        }

        private static bool validColName(string p)
        {
            if (string.IsNullOrEmpty(p)) return false;
            p = p.ToUpper().Trim();
            string dp = demodify(p, '`');
            if (dp[0] < 'A' || dp[0] > 'Z') return false;
            if (defaultValue(p) != "") return false; // 是个已知的类型名称
            var res = from keyword in xdSqlKeywords.Descendants("KeyWord")
                      where keyword.Attribute("name").Value == p
                      select keyword.Name;
            return res.Count<XName>() == 0;
        }

        private static string demodify(string text, char ch)
        {
            text = text.Trim();
            if (text.Length < 2) return text;
            if (text[0] == text[text.Length - 1] && (text[0] == ch))
                if (text.Length == 2) return ""; else return text.Substring(1, text.Length - 2);
            return text;
        }

        private static void generateCode(StreamWriter swPhp, string tname, string primary_key, bool pkai, bool getter, bool setter, ref List<string> colname, ref List<string> coltype)
        {
            swPhp.WriteLine("class {0} {{\r\n", capitalize(tname));

            if (colname.Count == 0) return;
            swPhp.Write("\tvar ${0}{1}", colname[0], defaultValue(coltype[0], primary_key == colname[0]));
            for (int i = 1; i < colname.Count; ++i)
            {
                string cname = colname[i], ctype = coltype[i];
                swPhp.Write(", ${0}{1}", cname, defaultValue(ctype, primary_key == colname[i]));
            }
            swPhp.WriteLine(";");

            if (getter || setter)
            {
                for (int i = 0; i < colname.Count; ++i)
                {
                    string cname = colname[i], ctype = coltype[i];
                    swPhp.WriteLine();
                    if (getter) swPhp.WriteLine("\tpublic function get{0}(){{ return $this->{1}; }}", capitalize(cname), cname);
                    if (setter) swPhp.WriteLine("\tpublic function set{0}($value){{ return $this->{1} = $value; }}", capitalize(cname), cname);
                }
            }
            
            /*
            swPhp.WriteLine();
            swPhp.Write("\tpublic function __construct($" + colname[0]);
            string constructor = "";
            for (int i = 1; i < colname.Count; ++i)
            {
                swPhp.Write(", $" + colname[i]);
                constructor += string.Format("\t\t$this->{0} = ${0}; \r\n", colname[i]);
            }
            swPhp.WriteLine(") {");
            swPhp.WriteLine(constructor);
            swPhp.WriteLine("}");
            */

            swPhp.WriteLine();
            swPhp.WriteLine("\tpublic function fromRow(Array $row) {");
            for (int i = 0; i < colname.Count; ++i)
            {
                string cname = colname[i], ctype = coltype[i];
                swPhp.WriteLine("\t\t$this->{0} = !isset($row['{0}']) ? $this->{0} : $row['{0}'];", cname);
            }
            swPhp.WriteLine("\t}");

            swPhp.WriteLine();
            swPhp.WriteLine("\tpublic function delete(Database $db) {");
            swPhp.WriteLine("\t\t$db->query(\"DELETE FROM `{0}` WHERE `{1}`=$this->{1}\");", tname, primary_key);
            swPhp.WriteLine("\t}");
            
            swPhp.WriteLine();
            swPhp.WriteLine("\tpublic function update(Database $db) {");
            string sqlInsert= "",
                values = "",
                sqlUpdate = string.Format("UPDATE `" + tname + "` SET {0} = {1}", modify(colname[0], "`"), modify("$this->" + colname[0], coltype[0]));

            for (int i = 0; i < colname.Count; ++i)
            {
                if (colname[i] != primary_key || !pkai)
                {
                    string cname = colname[i], ctype = coltype[i];
                    swPhp.WriteLine("\t\t$t_{0} = mysql_escape_string(\"$this->{0}\");", cname);
                    sqlInsert += ", " + modify(cname, "`");
                    values += ", " + modify("$t_" + cname, ctype);
                    sqlUpdate += string.Format(", {0} = {1}", modify(cname, "`"), modify("$t_" + cname, ctype));
                }
            }
            sqlInsert = "INSERT INTO `" + tname + "` (" + sqlInsert.Substring(2) + ") VALUES (" + values.Substring(2) + ")";
            sqlUpdate += " WHERE " + modify(primary_key, "`") + " = $this->" + primary_key;
            swPhp.WriteLine("\t\t$SQLINSERT = " + modify(sqlInsert, "\"") + ";");
            swPhp.WriteLine("\t\t$SQLUPDATE = " + modify(sqlUpdate, "\"") + ";");
            swPhp.WriteLine("\t\tif ($this->{0} <= 0) $db->query($SQLINSERT); else $db->query($SQLUPDATE);", primary_key);
            swPhp.WriteLine("\t}");

            swPhp.WriteLine();
            swPhp.WriteLine("\tpublic static function fetchRecords($db, $condition = \"\", $recPerPage = 10, $page = 0) {\r\n\t\t$arr = Array();");
            swPhp.WriteLine("\t\t$result = $db->fetch('{0}', $condition, $recPerPage, $page * $recPerPage);", tname);
            swPhp.WriteLine("\t\twhile ($row = $db->fetchRow($result)) {");
            swPhp.WriteLine("\t\t\t$tmp = new {0}();", tname);
            swPhp.WriteLine("\t\t\t$tmp->fromRow($row);");
            swPhp.WriteLine("\t\t\t$arr[] = $tmp;", primary_key);
            swPhp.WriteLine("\t\t}\r\n\t\treturn $arr;\r\n\t}");
            
            swPhp.WriteLine("\r\n}\r\n");

        }

        private static string defaultValue(string sqltype, bool primaryKey)
        {
            string dv = defaultValue(sqltype);
            if (primaryKey && dv == " = 0") return " = -1";
            return dv;
        }

        private static string modify(string text, string parameter)
        {
            if (parameter.Length == 1) return parameter + text + parameter;
            if (defaultValue(parameter) == " = \"\"") parameter = "'"; else parameter = "";
            return parameter + text + parameter;
        }

        private static string demodify(string text)
        {
            text = text.Trim();
            if (text.Length < 2) return text;
            if ((text[0] == text[text.Length - 1] && (text[0] == '`' || text[0] == '\''|| text[0] == '\"')) || (text[0] =='(' && text[text.Length-1] == ')'))
                if (text.Length == 2) return ""; else return text.Substring(1, text.Length - 2);
            return text;
        }

        private static string capitalize(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return text.Substring(0, 1).ToUpper() + text.Substring(1);
        }

        private static string defaultValue(string sqltype)
        {
            if (sqltype.IndexOf('(') >= 0) sqltype = sqltype.Substring(0, sqltype.IndexOf('('));
            switch (sqltype.ToLower())
            {
                case "int":
                case "decimal":
                case "real":
                case "float":
                case "bigint":
                case "tinyint":
                    return " = 0";
                case "varchar":
                case "string":
                case "datetime":
                case "text":
                case "tinytext":
                case "longtext":
                    return " = \"\"";
                default:
                    return "";
            }
        }
    }
}
