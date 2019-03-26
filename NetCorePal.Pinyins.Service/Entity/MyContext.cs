using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCorePal.Pinyins.Service
{
    /// <summary>
    /// 构造的 DbContext
    /// </summary>
    public class MyContext : DbContext
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        { }

        /// <summary>
        /// 多音字字典
        /// </summary>
        public DbSet<DuoYinZiDictionaryRepo> DuoYinZiDictionaryRepos { get; set; }

        /// <summary>
        /// 百家姓
        /// </summary>
        public DbSet<BaiJiaXingRepo> BaiJiaXingRepos { get; set; }
    }
}
