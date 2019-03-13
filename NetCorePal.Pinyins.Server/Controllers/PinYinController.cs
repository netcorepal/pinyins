using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetCorePal.Toolkit.Pinyins.ChineseName;

namespace NetCorePal.Pinyins.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PinYinController : ControllerBase
    {
        //DbContext
        private readonly IConfiguration _config;
        string connection;
        string redisConnection;
        string redisPrefix;

        public PinYinController(IConfiguration _config)
        {
            connection = _config.GetConnectionString("DefaultConnection");
            redisConnection = _config.GetConnectionString("RedisConnectionStrings");
            redisPrefix = _config["RedisPrefix"];
        }

        /// <summary>
        /// 获取姓名拼音
        /// </summary>
        /// <param name="name">姓名</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> GetChineseNamePinYin(string name)
        {
            var result = ChineseNamePinyinConvert.GetChineseNamePinYin(name, connection, redisConnection, redisPrefix, null);
            return result;
        }

        /// <summary>
        /// 新增多音字
        /// </summary>
        /// <param name="name">新增多音字</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<bool> CreateWord(CreateWordModel name)
        {
            var result = ChineseNamePinyinConvert.CreateWordPinYin(name, connection, redisConnection, redisPrefix);
            return result;
        }
        /// <summary>
        /// 删除多音字
        /// </summary>
        /// <param name="model">多音字</param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult<bool> DeleteWord(DeleteWordModel model)
        {
            var result = ChineseNamePinyinConvert.DeleteWordPinYin(model.Word, connection, redisConnection, redisPrefix);
            return result;
        }
    }
}
