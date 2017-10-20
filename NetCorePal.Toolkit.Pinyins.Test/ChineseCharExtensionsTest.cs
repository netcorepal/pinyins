using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.International.Converters.PinYinConverter;
namespace NetCorePal.Toolkit.Pinyins.Test
{
    [TestClass]
    public class ChineseCharExtensionsTest
    {
        [TestMethod]
        public void GetPinyinInitialTest()
        {
            var initials = new ChineseChar('行').GetPinyinInitial();
            Assert.AreEqual(2, initials.Length);
            Assert.AreEqual('H', initials[0]);
            Assert.AreEqual('X', initials[1]);


            initials = new ChineseChar('啊').GetPinyinInitial();
            Assert.AreEqual(1, initials.Length);
            Assert.AreEqual('A', initials[0]);

            initials = new ChineseChar('好').GetPinyinInitial();
            Assert.AreEqual(1, initials.Length);
            Assert.AreEqual('H', initials[0]);
        }

        [TestMethod]
        public void GetPinyinsWithOutToneTest()
        {
            var pinyins = new ChineseChar('行').GetPinyinsWithOutTone();
            Assert.AreEqual(3, pinyins.Length);
            Assert.AreEqual("HANG", pinyins[0]);
            Assert.AreEqual("HENG", pinyins[1]);
            Assert.AreEqual("XING", pinyins[2]);

            pinyins = new ChineseChar('啊').GetPinyinsWithOutTone();
            Assert.AreEqual(1, pinyins.Length);
            Assert.AreEqual("A", pinyins[0]);

            pinyins = new ChineseChar('好').GetPinyinsWithOutTone();
            Assert.AreEqual(1, pinyins.Length);
            Assert.AreEqual("HAO", pinyins[0]);


        }
    }
}
