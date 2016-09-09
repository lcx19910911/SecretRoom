using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository;
using System.Linq;

namespace Web.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (DbRepository entities = new DbRepository())
            {
                
            }
        }
    }



}
