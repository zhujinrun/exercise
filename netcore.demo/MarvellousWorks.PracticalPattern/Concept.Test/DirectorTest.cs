using ConceptAttribute.Attributing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Concept.Test
{
    [TestClass]
    public class DirectorTest
    {
        [TestMethod]
        public void Test()
        {
            IAttributeBuilder builder = new AttributeBuilder();
            Director director = new Director();

            director.BuildUp(builder);
            Assert.AreEqual<string>("a", builder.Log[0]);
            Assert.AreEqual<string>("b", builder.Log[1]);
            Assert.AreEqual<string>("c", builder.Log[2]);
        }
    }
}
