using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetCorePal.Pinyins.Service;

namespace NetCorePal.Pinyins.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PinYinController : ControllerBase
    {
        private readonly IPinYinService _pinyinService;

        /// <summary>
        /// 拼音
        /// </summary>
        /// <param name="pinyinService"></param>
        public PinYinController(IPinYinService pinyinService)
        {
            _pinyinService = pinyinService;

        }

        /// <summary>
        /// 获取姓名拼音
        /// </summary>
        /// <param name="name">姓名</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> GetChineseNamePinYin(string name)
        {
            var result = _pinyinService.GetChineseNamePinYin(name);
            return result;
        }

        /// <summary>
        /// 新增多音字
        /// </summary>
        /// <param name="dto">新增多音字</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<bool> CreateWord(CreatePinYinDto dto)
        {
            var result = _pinyinService.CreatePinYin(dto);
            return result;
        }
        /// <summary>
        /// 删除多音字
        /// </summary>
        /// <param name="model">多音字</param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult<bool> DeleteWord(DeletePinYinDto model)
        {
            var result = _pinyinService.DeletePinYin(model);
            return result;
        }
    }
}
