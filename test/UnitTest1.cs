using System;
using System.Threading.Tasks;
using HLib.Functional.Currying;
using HLib.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HLib.Test
{
    [TestClass]
    public class UnitTest1
    {
        private int invokedCount = 0;
        private int result = 0;

        public float Test1(int x, int y, float z)
        {
            return (x + y) * z;
        }
        public void Test2(int x, int y, float z)
        {
            result = (int)((x + y) * z);
        }
        [TestMethod]
        public void CurryingTest()
        {
            var func = new Func<int, int, float, float>(Test1).Currying();
            Assert.AreEqual(33, func(4)(6)(3.3f));

            var action = new Action<int, int, float>(Test2).Currying();
            action(4)(6)(3.3f);
            Assert.AreEqual(33, result);
        }

        async Task<int> Test1Async(int x)
        {
            Assert.AreEqual(1, x);
            await Task.Delay(x);
            invokedCount++;
            return 2;
        }

        async Task<int> Test2Async(int x)
        {
            Assert.AreEqual(2, x);
            await Task.Delay(x);
            invokedCount++;
            return 3;
        }

        async Task Test3Async(int x)
        {
            Assert.AreEqual(3, x);
            await Task.Delay(x);
            invokedCount++;
        }
        [TestMethod]
        public async Task ThenTest()
        {
            var p = Test1Async(1).Then(x => Test2Async(x));
            Assert.AreEqual(3, await p);

            var k = Test1Async(1).Then(x => Test2Async(x)).Then(async x => await Test3Async(x));
            await k;

            await Test3Async(3).Then(async () => await Test2Async(2));

            await Test3Async(3).Then(async () => await Test3Async(3));

            Assert.AreEqual(9, invokedCount);
        }
    }
}
