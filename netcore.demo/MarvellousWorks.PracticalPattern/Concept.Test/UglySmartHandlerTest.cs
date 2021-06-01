using ConceptAttribute.Delegating;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concept.Test
{
 
    [TestClass()]
    public class UglySmartHandlerTest
    {
        [TestMethod()]
        public void Test()
        {
            UglySmartDelegateInvoker target = new UglySmartDelegateInvoker();
            Assert.AreEqual<int>(1+2 + 3, (int)target.Invoke(1, 2, 3));
        }
    }
}
