using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.Pinyins.Service
{
    /// <summary>
    /// 删除多音字
    /// </summary>
    public class DeletePinYinDto
    {
        /// <summary>
        /// 多音字
        /// </summary>
        public string Word
        {
            get; set;
        }
    }
}
