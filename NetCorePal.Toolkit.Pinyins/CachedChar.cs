using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.Toolkit.Pinyins
{
    class CachedChar
    {
        public CachedChar(char chr)
        {
            this.Char = chr;
            var cc = new ChineseChar(chr);
            this.Initials = cc.GetPinyinInitial();
            this.Pinyins = cc.GetPinyinsWithOutTone();
        }
        public Char Char { get; private set; }

        public string[] Pinyins { get; private set; }

        public char[] Initials { get; private set; }
    }
}
