using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchoolPal.Toolkit.Caching;
using SchoolPal.Toolkit.Caching.Redis;
using System;

namespace test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //ʹ��ServiceCollaction�������չ��������ע�����

            ICache c = new RedisCache("192.168.50.210:6379,defaultDatabase=1,abortConnect=false,ssl=false");
            //192.168.50.210:6379,defaultDatabase = 1,abortConnect = false,ssl = false
            c.Set<string>("zhao huimi ma", "he llo", DateTime.Now.AddHours(1));

            var AA = c.Get("zhao huimi ma");


        }
    }
}
