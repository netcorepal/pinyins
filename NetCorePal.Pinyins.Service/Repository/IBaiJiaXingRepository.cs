using System;
using System.Collections.Generic;
using System.Text;

namespace NetCorePal.Pinyins.Service.Repository
{
    /// <summary>
    /// 百家姓仓储服务
    /// </summary>
    public interface IBaiJiaXingRepository
    {
        /// <summary>
        /// 查询所有百家姓
        /// </summary>
        /// <returns></returns>
        List<BaiJiaXingRepo> GetList();
    }
}
