using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathsHelper;
//using MathsCommonHelper;
//using MathsCommonHelper;

namespace NUnit
{
    [TestFixture]
    public class TestClass_MathsLib
    {
        [TestCase]
        [Category("Normal Maths")]
        public void AddTest()
        {
            Maths helper = new Maths();
            int result = helper.Add(20, 10, true);
            Assert.AreEqual(30, result);

        }

        [TestCase]
        [Category("Normal Maths")]
        public void AddTestFalse()
        {
            Maths helper = new Maths();
            int result = helper.Add(20, 10, false);
            Assert.AreEqual(35, result);

        }

        [TestCase]
        [Category("Normal Maths")]
        //[Ignore("No Subract Test")]
        public void SubtractTest()
        {
            Maths helper = new Maths();
            int result = helper.Subtract(20, 10);
            Assert.AreEqual(10, result);
        }

        [TestCase]
        [Category("Advance Maths")]
        //[Ignore("No Subract Test")]
        public void MultTest()
        {
            Maths helper = new Maths();
            int result = helper.Multiply(20, 10);
            Assert.AreEqual(200, result);
        }

        [TestCase]
        [Category("Advance Maths")]
        //[Ignore("No Subract Test")]
        public void DivTest()
        {
            Maths helper = new Maths();
            int result = helper.Divide(20, 10);
            Assert.AreEqual(2, result);
        }

        //[TestCase]
        //[Category("Commmon Maths")]
        //[Ignore("No Subract Test")]
        //public void DivTestCommon()
        //{
        //    MathsCommon helper = new MathsCommon();
        //    int result = helper.DivideCommon(20, 10);
        //    Assert.AreEqual(2, result);
        //}
    }
}
