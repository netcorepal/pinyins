using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetCorePal.Pinyins.Client;

namespace test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var result = ChineseNamePinyinConvert.GetChineseNamePinYin("���ۼ�");
        }
    }
}
