using Microsoft.Extensions.DependencyInjection;
using NetCorePal.Pinyins.Client;
using System;
namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //使用ServiceCollaction对象的扩展方法进行注册服务
            ServiceCollection service = new ServiceCollection();
            service.AddSingleton(typeof(ChineseNamePinyinConvert), new ChineseNamePinyinConvert("http://192.168.20.202:9000"));

            var provider = service.BuildServiceProvider();
            var convert = provider.GetService<ChineseNamePinyinConvert>();

            var result = convert.GetChineseNamePinYin("言福朱石");
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
