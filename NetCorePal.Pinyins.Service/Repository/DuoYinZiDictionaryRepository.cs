using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCorePal.Pinyins.Service
{
    public class DuoYinZiDictionaryRepository : IDuoYinZiDictionaryRepository
    {
        private MyContext _context;

        public DuoYinZiDictionaryRepository(MyContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 创建多音字
        /// </summary>
        /// <returns></returns>
        public bool Create(DuoYinZiDictionaryRepo entity)
        {
            _context.DuoYinZiDictionaryRepos.Add(entity);
            return _context.SaveChanges() > 0;
        }
        /// <summary>
        /// 创建多音字
        /// </summary>
        /// <returns></returns>
        public bool Delete(string wordKey)
        {
            _context.Database.ExecuteSqlCommand("delete from duoyinzidictionary where wordkey=@p0", wordKey);
            return true;
        }
        /// <summary>
        /// 查询所有多音字
        /// </summary>
        /// <returns></returns>
        public List<DuoYinZiDictionaryRepo> GetList()
        {
            return _context.DuoYinZiDictionaryRepos.ToList();
        }
    }
}
