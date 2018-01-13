using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iExpr.Parser
{
    /// <summary>
    /// 字典树
    /// </summary>
    public class Trie : Dictionary<char, Trie>
    {
        public bool Flag { get; set; } = false;

        /// <summary>
        /// 插入一个字符串到指定字典树中
        /// </summary>
        /// <param name="s"></param>
        /// <param name="root"></param>
        public static void Insert(string s, Trie root)
        {
            foreach (char c in s)
            {
                if (!root.ContainsKey(c)) root.Add(c, new Trie());
                root = root[c];
            }
            root.Flag = true;
        }
    }
}
