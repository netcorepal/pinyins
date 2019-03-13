using Microsoft.International.Converters.PinYinConverter;
using MySql.Data.MySqlClient;
using NetCorePal.Toolkit.Pinyins;
using NetCorePal.Toolkit.Pinyins.ChineseName.Properties;
using SchoolPal.Toolkit.Caching;
using SchoolPal.Toolkit.Caching.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetCorePal.Toolkit.Pinyins.ChineseName
{
    /// <summary>
    /// 中文名称拼音转换
    /// </summary>
    public class ChineseNamePinyinConvert
    {
        /// <summary>
        /// 静态资源：存放百家姓
        /// </summary>
        static Dictionary<string, string> dictionary;

        /// <summary>
        /// 静态资源：存放redis实例
        /// </summary>
        static RedisCache redis { get; set; }

        /// <summary>
        /// 静态构造函数
        /// </summary
        static ChineseNamePinyinConvert()
        {
            dictionary = ReadPinYinConfiguration();
        }

        /// <summary>
        /// 获取姓名拼音
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="dbConnect">数据库连接字符串</param>
        /// <param name="redisConnect">redis连接字符串</param>
        /// <param name="cachePrefix">缓存</param>
        /// <param name="expiry">缓存过期时间</param>
        /// <returns>转换后的拼音</returns>
        public static string GetChineseNamePinYin(string name, string dbConnect = null, string redisConnect = null, string cachePrefix = null, DateTime? expiry = null)
        {
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
                //判断dbHost和redisHost是否配置
                if (!string.IsNullOrWhiteSpace(dbConnect) || !string.IsNullOrWhiteSpace(redisConnect))
                {
                    //判断多音字优先字典是否存在
                    var redisPinYin = GetPinyinByRedis(redisConnect, cachePrefix);
                    if (redisPinYin == null || redisPinYin.Count() <= 0)//不存在
                    {
                        //从db获取多音字优先字典
                        redisPinYin = GetPinYinDictionary(dbConnect);
                        //存入redis
                        SetPinYinToRedis(redisPinYin, redisConnect, cachePrefix, expiry);
                    }
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
                else
                {
                    //常规拼音转换
                    namePinYin.Append(PinyinConvert.ToPinyins(ming, true)[0]);
                }
            }
            return namePinYin.ToString();
        }

        /// <summary>
        /// 读取拼音配置项
        /// </summary>
        /// <returns>字典集</returns>
        private static Dictionary<string, string> ReadPinYinConfiguration()
        {
            //定义字典集
            var dictionary = new Dictionary<string, string>();
            var resource = Resources.ChineseNames;
            if (string.IsNullOrWhiteSpace(resource))
            {
                throw new ArgumentNullException("resource", "百家姓资源不存在");
            }
            var strArray = resource.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (var item in strArray)
            {
                string item_new = ReplaceString(item);
                string[] nameStrArray = item_new.Split(new string[] { "\t" }, StringSplitOptions.None);
                dictionary.Add(nameStrArray[0], nameStrArray[1]);
            }
            return dictionary;
        }

        /// <summary>
        /// 替换声调和空格
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>处理后的字符串</returns>
        private static string ReplaceString(string str)
        {
            //a
            str = str.Replace("ā", "a");
            str = str.Replace("á", "a");
            str = str.Replace("ǎ", "a");
            str = str.Replace("à", "a");
            //e
            str = str.Replace("ē", "e");
            str = str.Replace("é", "e");
            str = str.Replace("ě", "e");
            str = str.Replace("è", "e");
            //i
            str = str.Replace("ī", "i");
            str = str.Replace("í", "i");
            str = str.Replace("ǐ", "i");
            str = str.Replace("ì", "i");
            //o
            str = str.Replace("ō", "o");
            str = str.Replace("ó", "o");
            str = str.Replace("ǒ", "o");
            str = str.Replace("ò", "o");
            //u
            str = str.Replace("ū", "u");
            str = str.Replace("ú", "u");
            str = str.Replace("ǔ", "u");
            str = str.Replace("ù", "u");
            //v
            str = str.Replace("ǖ", "v");
            str = str.Replace("ǘ", "v");
            str = str.Replace("ǚ", "v");
            str = str.Replace("ǜ", "v");

            //去除空格
            str = str.Replace(" ", string.Empty);
            return str;
        }

        /// <summary>
        /// 获取多音字优先命中字典
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns>多音字优先命中字典</returns>
        public static Dictionary<string, string> GetPinYinDictionary(string connectionString)
        {
            var conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    var result = new Dictionary<string, string>();
                    command.CommandText = "SELECT WordKey,WordValue FROM duoyinzidictionary;";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var key = reader["WordKey"].ToString();
                            var value = reader["WordValue"].ToString();
                            result.Add(key, value);
                        }
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 设置拼音字典到redis
        /// </summary>
        /// <param name="dictionary">多音字字典集</param>
        /// <param name="redisConnect">redis连接字符串</param>
        /// <param name="cachePrefix">缓存</param>
        /// <param name="expiry">缓存过期时间</param>
        public static void SetPinYinToRedis(Dictionary<string, string> dictionary, string redisConnect, string cachePrefix, DateTime? expiry)
        {
            try
            {
                //构造一个redis实例
                if (redis == null)
                {
                    redis = new RedisCache(redisConnect, cachePrefix);
                }
                //存入多音字字典，在不传入过期时间的情况下，默认为1小时
                if (expiry == null)
                {
                    expiry = DateTime.Now.AddHours(1);
                }
                redis.Set("pinyinset", dictionary, Convert.ToDateTime(expiry));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 清空拼音Redis
        /// </summary>
        /// <param name="redisConnect"></param>
        /// <param name="cachePrefix"></param>
        public static void ClearPinYinRedis(string redisConnect, string cachePrefix)
        {
            try
            {
                //构造一个redis实例
                if (redis == null)
                {
                    redis = new RedisCache(redisConnect, cachePrefix);
                }
                redis.Remove("pinyinset");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 从redis获取拼音字典
        /// </summary>
        /// <param name="redisConnect">redis连接字符串</param>
        /// <param name="cachePrefix">缓存</param>
        /// <returns>多音字字典集</returns>
        public static Dictionary<string, string> GetPinyinByRedis(string redisConnect, string cachePrefix)
        {
            try
            {
                //构造一个redis实例
                if (redis == null)
                {
                    redis = new RedisCache(redisConnect, cachePrefix);
                }
                //获取多音字字典
                var pinyinset = redis.Get<Dictionary<string, string>>("pinyinset");
                return pinyinset;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 创建多音字
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool CreateWordPinYin(CreateWordModel model, string connectionString, string redisConnect = null, string cachePrefix = null)
        {
            var conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.Parameters.Add(new MySqlParameter() { ParameterName = "@WordKey", Value = model.Word });
                    command.Parameters.Add(new MySqlParameter() { ParameterName = "@WordValue", Value = model.PinYin });
                    command.CommandText = "insert into duoyinzidictionary(WordKey,WordValue) values(@WordKey,@WordValue)";
                    int num = command.ExecuteNonQuery();
                    if (num > 0)
                    {
                        ClearPinYinRedis(redisConnect, cachePrefix);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 删除多音字
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool DeleteWordPinYin(string word, string connectionString, string redisConnect = null, string cachePrefix = null)
        {
            var conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.Parameters.Add(new MySqlParameter() { ParameterName = "@WordKey", Value = word });
                    command.CommandText = "delete from duoyinzidictionary where WordKey=@WordKey";
                    int num = command.ExecuteNonQuery();
                    if (num > 0)
                    {
                        ClearPinYinRedis(redisConnect, cachePrefix);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
    }

}
