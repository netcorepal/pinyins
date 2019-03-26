using Microsoft.Extensions.Configuration;
using NetCorePal.Pinyins.Service;
using NetCorePal.Pinyins.Service.Repository;
using NetCorePal.Toolkit.Pinyins;
using SchoolPal.Toolkit.Caching;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace NetCorePal.Pinyins.Service
{
    /// <summary>
    /// 拼音服务
    /// </summary>
    public class PinYinService : IPinYinService
    {
        private readonly IDuoYinZiDictionaryRepository _duoYinZiDictionaryRepository;
        private readonly ICache _cache;
        private readonly IConfiguration _config;
        private readonly IBaiJiaXingRepository _baiJiaXingRepository;

        public PinYinService(IDuoYinZiDictionaryRepository duoYinZiDictionaryRepository, ICache cache, IConfiguration config, IBaiJiaXingRepository baiJiaXingRepository)
        {
            _duoYinZiDictionaryRepository = duoYinZiDictionaryRepository;
            _cache = cache;
            _config = config;
            _baiJiaXingRepository = baiJiaXingRepository;
        }

        /// <summary>
        /// 查询姓名的拼音
        /// </summary>
        /// <returns></returns>
        public string GetChineseNamePinYin(string name)
        {
            //百家姓字典
            Dictionary<string, string> dictionary = GetBaiJiaXing();
            //校验姓名是否为空，如果为空，直接抛错
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            //只有一个字符
            if (name.Length == 1)
            {
                //命中百家姓单姓，直接返回单姓
                if (dictionary.ContainsKey(name))
                {
                    return dictionary[name];
                }
                return PinyinConvert.ToPinyins(name, true)[0];
            }
            var namePinYin = new StringBuilder();
            var num = 0;
            //判断key是否存在
            if (dictionary.ContainsKey(name.Substring(0, 2)))
            {
                namePinYin.Append(dictionary[name.Substring(0, 2)]);
                num = 2;
            }
            else
            {
                //判断key是否存在
                if (dictionary.ContainsKey(name.Substring(0, 1)))
                {
                    namePinYin.Append(dictionary[name.Substring(0, 1)]);
                    num = 1;
                }
            }
            //名命中
            var ming = name.Remove(0, num);
            if (ming.Length > 0)
            {
                var redisPinYin = GetDuoYinZi();
                //尝试命中多音字
                foreach (var item in ming)
                {
                    var zi = item.ToString();
                    if (redisPinYin.ContainsKey(zi))
                    {
                        namePinYin.Append(redisPinYin[zi]);
                    }
                    else
                    {
                        //常规拼音转换
                        namePinYin.Append(PinyinConvert.ToPinyins(zi, true)[0]);
                    }
                }
            }
            return namePinYin.ToString();
        }

        /// <summary>
        /// 创建拼音
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool CreatePinYin(CreatePinYinDto dto)
        {
            var result = _duoYinZiDictionaryRepository.Create(new DuoYinZiDictionaryRepo()
            {
                WordKey = dto.WordKey,
                WordValue = dto.WorkValue
            });
            _cache.Remove(_config.GetSection("DuoYinZiKey").Value);
            return true;
        }
        /// <summary>
        /// 删除拼音
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool DeletePinYin(DeletePinYinDto dto)
        {
            _duoYinZiDictionaryRepository.Delete(dto.Word);
            _cache.Remove(_config.GetSection("DuoYinZiKey").Value);
            return true;
        }

        /// <summary>
        /// 读取百家姓
        /// </summary>
        /// <returns>字典集</returns>
        private Dictionary<string, string> GetBaiJiaXing()
        {
            var key = _config.GetSection("BaiJiaXingKey").Value;
            //定义字典集
            var dictionary = new Dictionary<string, string>();
            var dic = _cache.Get<Dictionary<string, string>>(key);
            if (dic == null || dic.Count <= 0)
            {
                var result = _baiJiaXingRepository.GetList();
                foreach (var item in result)
                {
                    dictionary.Add(item.WordKey, item.WordValue);
                }
                _cache.Set<Dictionary<string, string>>(key, dictionary, DateTime.Now.AddDays(1));
                return dictionary;
            }
            return dic;
        }

        /// <summary>
        /// 读取多音字
        /// </summary>
        /// <returns>字典集</returns>
        private Dictionary<string, string> GetDuoYinZi()
        {
            var key = _config.GetSection("DuoYinZiKey").Value;
            //定义字典集
            var dictionary = new Dictionary<string, string>();
            var dic = _cache.Get<Dictionary<string, string>>(key);
            if (dic == null || dic.Count <= 0)
            {
                var result = _duoYinZiDictionaryRepository.GetList();
                foreach (var item in result)
                {
                    dictionary.Add(item.WordKey, item.WordValue);
                }
                _cache.Set<Dictionary<string, string>>(key, dictionary, DateTime.Now.AddDays(1));
                return dictionary;
            }
            return dic;
        }
    }
}
