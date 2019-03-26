using System;
using System.Collections.Generic;
using System.Text;

namespace NetCorePal.Pinyins.Service
{
    /// <summary>
    /// 创建拼音的模型
    /// </summary>
    public class CreatePinYinDto
    {
        /// <summary>
        /// 汉字
        /// </summary>
        public string WordKey
        {
            get; set;
        }
        /// <summary>
        /// 拼音
        /// </summary>
        public string WorkValue
        {
            get; set;
        }
    }
}
