using System;
using Microsoft.International.Converters.PinYinConverter;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections.Concurrent;
namespace NetCorePal.Toolkit.Pinyins
{
    /// <summary>
    /// 拼音转换器
    /// </summary>
    public class PinyinConvert
    {
        /// <summary>
        /// 将字符串转为拼音，多音字会转成多个组合
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="lower">输出小写</param>
        /// <param name="maxLength">输出数组最大长度，默认10000</param>
        /// <returns></returns>
        public static string[] ToPinyins(string str, bool lower = false, int maxLength = 10000)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException(nameof(str));
            }
            if (maxLength <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxLength));
            }
            var results = ToPinyinsImp(str, int.MaxValue, maxLength, 0, lower);

            return results;
        }
        /// <summary>
        /// 转为拼音首字母，存在多音字则返回各种组合，最大输出不会超过5000000个字符，避免内存溢出
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="lower">是否转为全小写</param>
        /// <param name="maxLength">输出数组最大长度，默认10000</param>
        /// <returns></returns>
        public static string[] ToPinyinInitials(string str, bool lower = false, int maxLength = 10000)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException(nameof(str));
            }
            if (maxLength <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxLength));
            }
            var results = ToPinyinInitialsImp(str, int.MaxValue, maxLength, 0, lower);
            return results;
        }


        /// <summary>
        /// 获取格式化过的拼音字符串，由 首字母拼音和全拼拼音组成，由<paramref name="separator"/>隔开,最大支持输出5000000长度的字符
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="separator"></param>
        /// <param name="lower">是否转为全小写</param>
        /// <param name="maxLength">输出结果最大长度，默认maxLength小于或等于0时，输出最大长度500000，否则将根据maxLength截取数据结果</param>
        /// <returns></returns>
        public static string ToPinyinSearchFomat(string str, string separator = ";", bool lower = false, int maxLength = 500000)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (maxLength <= 0) { maxLength = OutPutMaxLength; }

            StringBuilder r = new StringBuilder(Math.Min(maxLength, OutPutMaxLength));
            var arr = ToPinyinInitialsImp(str, maxLength, int.MaxValue, separator.Length, lower);
            foreach (var item in arr)
            {
                r.Append(item);
                r.Append(separator);
                if (maxLength > 0 && r.Length > maxLength)
                {
                    return r.ToString(0, maxLength);
                }
            }
            arr = ToPinyinsImp(str, maxLength - r.Length, int.MaxValue, separator.Length, lower);
            foreach (var item in arr)
            {
                r.Append(item);
                r.Append(separator);
                if (maxLength > 0 && r.Length > maxLength)
                {
                    return r.ToString(0, maxLength);
                }
            }
            r.Remove(r.Length - 1, 1);
            return lower ? r.ToString().ToLower() : r.ToString();
        }


        #region 私有方法
        /// <summary>
        /// 输出的最大长度
        /// </summary>
        const int OutPutMaxLength = 5000000;
        static ConcurrentDictionary<char, CachedChar> cache = new ConcurrentDictionary<char, CachedChar>();
        static CachedChar GetChart(char chr)
        {
            if (cache.TryGetValue(chr, out var value))
            {
                return value;
            }
            var v = new CachedChar(chr);
            cache.TryAdd(chr, v);
            return v;
        }

        static string[] ToPinyinsImp(string str, int maxLength, int maxArrayLength, int separatorLength, bool lower = false)
        {
            var ar = ToPinyinsArray(str, lower);

            return ToPinyinsWorlds(ar, maxLength, maxArrayLength, separatorLength, lower);
        }


        static string[] ToPinyinInitialsImp(string str, int maxLength, int maxArrayLength, int separatorLength, bool lower = false)
        {
            var ar = ToInitialArray(str, lower);

            return ToPinyinsWorlds(ar, maxLength, maxArrayLength, separatorLength, lower);
        }


        static bool CanSkip(char chr)
        {
            return false;
        }


        static string[][] ToInitialArray(string str, bool lower)
        {
            var ar = new string[str.Length][];
            for (int i = 0; i < str.Length; i++)
            {
                var chr = str[i];
                if (ChineseChar.IsValidChar(chr))
                {
                    var cc = GetChart(chr);
                    ar[i] = lower ? cc.Initials.Select(p => char.ToLower(p).ToString()).ToArray() : cc.Initials.Select(p => p.ToString()).ToArray();
                }
                else
                {
                    ar[i] = new string[] { lower ? char.ToLower(chr).ToString() : char.ToUpper(chr).ToString() };
                }
            }

            return ar;
        }

        static string[][] ToPinyinsArray(string str, bool lower)
        {
            var ar = new string[str.Length][];
            for (int i = 0; i < str.Length; i++)
            {
                var chr = str[i];
                if (ChineseChar.IsValidChar(chr))
                {
                    var cc = GetChart(chr);
                    ar[i] = lower ? cc.Pinyins.Select(p => p.ToLower()).ToArray() : cc.Pinyins;
                }
                else
                {
                    ar[i] = new string[] { lower ? char.ToLower(chr).ToString() : char.ToUpper(chr).ToString() };
                }
            }

            return ar;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        /// <param name="maxLength"></param>
        /// <param name="maxArrayLength"></param>
        /// <param name="separatorLength"></param>
        /// <param name="lower"></param>
        /// <returns></returns>
        static string[] ToPinyinsWorlds(string[][] ar, int maxLength, int maxArrayLength, int separatorLength, bool lower)
        {

            var indexAr = new int[ar.Length];
            indexAr[0] = -1;


            var r = new List<string>();

            StringBuilder sb = new StringBuilder();
            while (maxArrayLength > r.Count && Next(ar, indexAr))
            {
                for (int i = 0; i < ar.Length; i++)
                {
                    sb.Append(ar[i][indexAr[i]]);
                }
                var len = maxLength - r.Sum(p => p.Length) - (separatorLength * r.Count);

                if (len > sb.Length)
                {

                    r.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    if (len > 0)
                    {
                        r.Add(sb.ToString(0, len));
                    }
                    sb.Clear();
                    break;
                }
            }

            return r.ToArray();
        }

        static bool Next(string[][] ar, int[] indexAr)
        {
            for (int i = 0; i < indexAr.Length; i++)
            {
                var innerAr = ar[i];

                if (innerAr.Length > indexAr[i] + 1)
                {
                    indexAr[i] += 1;
                    return true;
                }
                else
                {
                    indexAr[i] = 0;
                }
            }
            return false;
        }
        #endregion
    }
}
