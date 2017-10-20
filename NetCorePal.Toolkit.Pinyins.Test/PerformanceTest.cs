using Microsoft.International.Converters.PinYinConverter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;
namespace NetCorePal.Toolkit.Pinyins.Test
{
    [TestClass]
    public class PerformanceTest
    {
        [TestMethod]
        public void Performance1000()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Parallel.For(0, 10000, i =>
            {
                //ChineseChar.IsValidChar('行');
                var str = PinyinConvert.ToPinyinSearchFomat("沙发沙发沙发行杀放上士大夫士大夫好士大夫似的空撒旦解放立刻");
            });

            watch.Stop();
        }
    }
}
