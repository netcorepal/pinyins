using NetCorePal.Toolkit.Pinyins.ChineseName.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCorePal.Toolkit.Pinyins.ChineseName
{

    /// <summary>
    /// 中文名称拼音转换
    /// </summary>
    public class ChineseNamePinyinConvert
    {
        /// <summary>
        /// 静态资源：存放百家姓
        /// </summary>
        static Dictionary<string, string> dictionary;

        /// <summary>
        /// 静态构造函数
        /// </summary
        static ChineseNamePinyinConvert()
        {
            dictionary = ReadPinYinConfiguration();
        }

        /// <summary>
        /// 获取姓名拼音
        /// </summary>
        /// <param name="name">姓名</param>
        /// <returns>转换后的拼音</returns>
        public static string GetChineseNamePinYin(string name)
        {
            //校验姓名是否为空，如果为空，直接抛错
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            //只有一个字符
            if (name.Length == 1)
            {
                //命中百家姓单姓，直接返回单姓
                if (dictionary.ContainsKey(name))
                {
                    return dictionary[name];
                }
                return PinyinConvert.ToPinyins(name, true)[0];
            }
            var namePinYin = new StringBuilder();
            var num = 0;
            //判断key是否存在
            if (dictionary.ContainsKey(name.Substring(0, 2)))
            {
                namePinYin.Append(dictionary[name.Substring(0, 2)]);
                num = 2;
            }
            else
            {
                //判断key是否存在
                if (dictionary.ContainsKey(name.Substring(0, 1)))
                {
                    namePinYin.Append(dictionary[name.Substring(0, 1)]);
                    num = 1;
                }
            }
            //名命中
            var ming = name.Remove(0, num);
            if (ming.Length > 0)
            {
                //常规拼音转换
                namePinYin.Append(PinyinConvert.ToPinyins(ming, true)[0]);
            }
            return namePinYin.ToString();
        }

        /// <summary>
        /// 读取拼音配置项
        /// </summary>
        /// <returns>字典集</returns>
        private static Dictionary<string, string> ReadPinYinConfiguration()
        {
            //定义字典集
            var dictionary = new Dictionary<string, string>();
            var resource = Resources.ChineseNames;
            if (string.IsNullOrWhiteSpace(resource))
            {
                throw new ArgumentNullException("resource", "百家姓资源不存在");
            }
            var strArray = resource.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (var item in strArray)
            {
                string item_new = ReplaceString(item);
                string[] nameStrArray = item_new.Split(new string[] { "\t" }, StringSplitOptions.None);
                dictionary.Add(nameStrArray[0], nameStrArray[1]);
            }
            return dictionary;
        }

        /// <summary>
        /// 替换声调和空格
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>处理后的字符串</returns>
        private static string ReplaceString(string str)
        {
            //a
            str = str.Replace("ā", "a");
            str = str.Replace("á", "a");
            str = str.Replace("ǎ", "a");
            str = str.Replace("à", "a");
            //e
            str = str.Replace("ē", "e");
            str = str.Replace("é", "e");
            str = str.Replace("ě", "e");
            str = str.Replace("è", "e");
            //i
            str = str.Replace("ī", "i");
            str = str.Replace("í", "i");
            str = str.Replace("ǐ", "i");
            str = str.Replace("ì", "i");
            //o
            str = str.Replace("ō", "o");
            str = str.Replace("ó", "o");
            str = str.Replace("ǒ", "o");
            str = str.Replace("ò", "o");
            //u
            str = str.Replace("ū", "u");
            str = str.Replace("ú", "u");
            str = str.Replace("ǔ", "u");
            str = str.Replace("ù", "u");
            //v
            str = str.Replace("ǖ", "v");
            str = str.Replace("ǘ", "v");
            str = str.Replace("ǚ", "v");
            str = str.Replace("ǜ", "v");

            //去除空格
            str = str.Replace(" ", string.Empty);
            return str;
        }
    }
}
