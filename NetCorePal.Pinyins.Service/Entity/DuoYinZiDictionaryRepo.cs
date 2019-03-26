using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetCorePal.Pinyins.Service
{
    /// <summary>
    /// 多音字字典
    /// </summary>
    [Table("duoyinzidictionary")]
    public class DuoYinZiDictionaryRepo
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id
        {
            get; set;
        }
        /// <summary>
        /// 字
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
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateAt
        {
            get; set;
        } = DateTime.Now;
    }
}
