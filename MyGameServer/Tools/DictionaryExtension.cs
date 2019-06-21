using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGameServer.Tools
{
    public static class DictionaryExtension
    {
        /// <summary>
        /// 尝试根据key得到value，得到了的话直接返回value，没有得到直接返回null
        /// this Dictionary<Tkey,Tvalue> dict 这个字典表示我们要获取值的字典
        /// </summary>
        public static Tvalue TryGet<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dict, Tkey key)
        {
            Tvalue value;
            dict.TryGetValue(key, out value);
            return value;
        }
    }
}
