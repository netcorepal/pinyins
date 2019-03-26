using System;
using System.Collections.Generic;
using System.Text;

namespace NetCorePal.Pinyins.Service
{
    /// <summary>
    /// 多音字仓储方法
    /// </summary>
    public interface IDuoYinZiDictionaryRepository
    {
        /// <summary>
        /// 创建多音字
        /// </summary>
        /// <returns></returns>
        bool Create(DuoYinZiDictionaryRepo entity);
        /// <summary>
        /// 删除多音字
        /// </summary>
        /// <param name="wordKey"></param>
        /// <returns></returns>
        bool Delete(string wordKey);
        /// <summary>
        /// 查询所有多音字
        /// </summary>
        /// <returns></returns>
        List<DuoYinZiDictionaryRepo> GetList();
    }
}
