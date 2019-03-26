using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetCorePal.Pinyins.Service
{
    [Table("baijiaxing")]
    public class BaiJiaXingRepo
    {
        public long Id
        {
            get; set;
        }
        /// <summary>
        /// 姓
        /// </summary>
        public string WordKey
        {
            get; set;
        }
        /// <summary>
        /// 拼音
        /// </summary>
        public string WordValue
        {
            get; set;
        }
    }
}
