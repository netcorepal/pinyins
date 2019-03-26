using System;

namespace NetCorePal.Pinyins.Service
{
    /// <summary>
    /// 拼音服务
    /// </summary>
    public interface IPinYinService
    {
        /// <summary>
        /// 查询姓名的拼音
        /// </summary>
        /// <returns></returns>
        string GetChineseNamePinYin(string name);


        /// <summary>
        /// 创建拼音
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        bool CreatePinYin(CreatePinYinDto dto);

        /// <summary>
        /// 删除拼音
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        bool DeletePinYin(DeletePinYinDto dto);


    }
}
