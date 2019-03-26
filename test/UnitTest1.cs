using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetCorePal.Toolkit.Pinyins.ChineseName;

namespace test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //var result = ChineseNamePinyinConvert.GetChineseNamePinYin("µ¥ÐÛ¼Ò");
            var result = ChineseNamePinyinConvert.GetChineseNamePinYin("ass");
        }
    }
}
