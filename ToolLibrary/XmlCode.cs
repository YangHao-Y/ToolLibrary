using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ToolLibrary
{
    public class XmlCode
    {

        /// <summary>
        /// 取出XML中指定节点的值
        /// </summary>
        /// <param name="src"></param>
        /// <param name="field"></param>
        /// <param name="defaultNull"></param>
        /// <returns></returns>
        private static string MatchProterty(string src, string field, bool defaultNull)
        {
            Match match = Regex.Match(src, "<" + field + @">(?<Value>[\s\S]*?)</" + field + ">");
            if (match.Success)
            {
                return match.Groups["Value"].Value;
            }
            return defaultNull ? null : "";
        }
    }
}
