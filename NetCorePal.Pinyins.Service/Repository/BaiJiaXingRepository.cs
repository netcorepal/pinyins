using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCorePal.Pinyins.Service.Repository
{
    public class BaiJiaXingRepository : IBaiJiaXingRepository
    {
        private MyContext _context;

        public BaiJiaXingRepository(MyContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 查询所有百家姓
        /// </summary>
        /// <returns></returns>
        public List<BaiJiaXingRepo> GetList()
        {
            return _context.BaiJiaXingRepos.ToList();
        }
    }
}
