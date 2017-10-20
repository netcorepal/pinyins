using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.International.Converters.PinYinConverter;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("NetCorePal.Toolkit.Pinyins.Test")]
namespace NetCorePal.Toolkit.Pinyins
{
    internal static class ChineseCharExtensions
    {
        public static string[] GetPinyinsWithOutTone(this ChineseChar chineseChar)
        {
            return chineseChar.Pinyins.Take(chineseChar.PinyinCount).Select(p => p.Substring(0, p.Length - 1)).Distinct().ToArray();
        }

        public static char[] GetPinyinInitial(this ChineseChar chineseChar)
        {
            return chineseChar.Pinyins.Take(chineseChar.PinyinCount).Select(p => p[0]).Distinct().ToArray();
        }
    }
}
