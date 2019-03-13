using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCorePal.Toolkit.Pinyins.ChineseName
{
    /// <summary>
    /// 创建多音字
    /// </summary>
    public class CreateWordModel
    {
        /// <summary>
        /// 多音字
        /// </summary>
        public string Word
        {
            get; set;
        }
        /// <summary>
        /// 拼音
        /// </summary>
        public string PinYin
        {
            get; set;
        }
    }
}
