using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
namespace NetCorePal.Toolkit.Pinyins.Test
{
    [TestClass]
    public class PinyinConvtorTest
    {
        [TestMethod]
        public void ToPinyinsTest()
        {
            var v = PinyinConvert.ToPinyins("行行好吧");

            Assert.AreEqual(9, v.Length);

            Assert.IsTrue(v.Contains("XINGXINGHAOBA"));
            Assert.IsTrue(v.Contains("XINGHENGHAOBA"));
            Assert.IsTrue(v.Contains("XINGHANGHAOBA"));
            Assert.IsTrue(v.Contains("HENGXINGHAOBA"));
            Assert.IsTrue(v.Contains("HENGHENGHAOBA"));
            Assert.IsTrue(v.Contains("HENGHANGHAOBA"));
            Assert.IsTrue(v.Contains("HANGXINGHAOBA"));
            Assert.IsTrue(v.Contains("HANGHENGHAOBA"));
            Assert.IsTrue(v.Contains("HANGHANGHAOBA"));


            v = PinyinConvert.ToPinyins("行行好吧", true);

            Assert.AreEqual(9, v.Length);

            Assert.IsTrue(v.Contains("XINGXINGHAOBA".ToLower()));
            Assert.IsTrue(v.Contains("XINGHENGHAOBA".ToLower()));
            Assert.IsTrue(v.Contains("XINGHANGHAOBA".ToLower()));
            Assert.IsTrue(v.Contains("HENGXINGHAOBA".ToLower()));
            Assert.IsTrue(v.Contains("HENGHENGHAOBA".ToLower()));
            Assert.IsTrue(v.Contains("HENGHANGHAOBA".ToLower()));
            Assert.IsTrue(v.Contains("HANGXINGHAOBA".ToLower()));
            Assert.IsTrue(v.Contains("HANGHENGHAOBA".ToLower()));
            Assert.IsTrue(v.Contains("HANGHANGHAOBA".ToLower()));
        }


        [TestMethod]
        public void ToPinyinInitialsTest()
        {
            var v = PinyinConvert.ToPinyinInitials("行行好吧");

            Assert.AreEqual(4, v.Length);
            Assert.IsTrue(v.Contains("XXHB"));
            Assert.IsTrue(v.Contains("XHHB"));
            Assert.IsTrue(v.Contains("HXHB"));
            Assert.IsTrue(v.Contains("HHHB"));

            v = PinyinConvert.ToPinyinInitials("行行好吧", true);
            Assert.AreEqual(4, v.Length);
            Assert.IsTrue(v.Contains("XXHB".ToLower()));
            Assert.IsTrue(v.Contains("XHHB".ToLower()));
            Assert.IsTrue(v.Contains("HXHB".ToLower()));
            Assert.IsTrue(v.Contains("HHHB".ToLower()));
        }

        [TestMethod]
        public void ToPinyinSearchFomatTest()
        {
            var v = PinyinConvert.ToPinyinSearchFomat("行行好吧");

            Assert.IsTrue(v.Contains(";"));
            Assert.IsTrue(v.Contains("XINGXINGHAOBA"));
            Assert.IsTrue(v.Contains("XINGHENGHAOBA"));
            Assert.IsTrue(v.Contains("XINGHANGHAOBA"));
            Assert.IsTrue(v.Contains("HENGXINGHAOBA"));
            Assert.IsTrue(v.Contains("HENGHENGHAOBA"));
            Assert.IsTrue(v.Contains("HENGHANGHAOBA"));
            Assert.IsTrue(v.Contains("HANGXINGHAOBA"));
            Assert.IsTrue(v.Contains("HANGHENGHAOBA"));
            Assert.IsTrue(v.Contains("HANGHANGHAOBA"));
            Assert.IsTrue(v.Contains("XXHB"));
            Assert.IsTrue(v.Contains("XHHB"));
            Assert.IsTrue(v.Contains("HXHB"));
            Assert.IsTrue(v.Contains("HHHB"));

            v = PinyinConvert.ToPinyinSearchFomat("行行好吧", separator: ",", lower: true);
            Assert.IsTrue(v.Contains(","));
            Assert.IsTrue(v.Contains("XINGXINGHAOBA".ToLower()));
            Assert.IsTrue(v.Contains("XINGHENGHAOBA".ToLower()));
            Assert.IsTrue(v.Contains("XINGHANGHAOBA".ToLower()));
            Assert.IsTrue(v.Contains("HENGXINGHAOBA".ToLower()));
            Assert.IsTrue(v.Contains("HENGHENGHAOBA".ToLower()));
            Assert.IsTrue(v.Contains("HENGHANGHAOBA".ToLower()));
            Assert.IsTrue(v.Contains("HANGXINGHAOBA".ToLower()));
            Assert.IsTrue(v.Contains("HANGHENGHAOBA".ToLower()));
            Assert.IsTrue(v.Contains("HANGHANGHAOBA".ToLower()));
            Assert.IsTrue(v.Contains("XXHB".ToLower()));
            Assert.IsTrue(v.Contains("XHHB".ToLower()));
            Assert.IsTrue(v.Contains("HXHB".ToLower()));
            Assert.IsTrue(v.Contains("HHHB".ToLower()));



            Assert.AreEqual(145, v.Length);

            v = PinyinConvert.ToPinyinSearchFomat("行行好吧", separator: ",", lower: true, maxLength: 100);
            Assert.AreEqual(100, v.Length);

            v = PinyinConvert.ToPinyinSearchFomat("行行好吧", separator: ",", lower: true, maxLength: 0);
            Assert.AreEqual(145, v.Length);

            v = PinyinConvert.ToPinyinSearchFomat("行行好吧", separator: ",", lower: true, maxLength: -1);
            Assert.AreEqual(145, v.Length);

            v = PinyinConvert.ToPinyinSearchFomat("行行好吧", separator: ",", lower: true, maxLength: 146);
            Assert.AreEqual(145, v.Length);
        }
    }
}
