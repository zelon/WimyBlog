using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimyBlog;

namespace WimyBlogTest
{
    [TestClass]
    public class TestMarkdown2Html
    {
        [TestMethod]
        public void TestBold()
        {
            Assert.AreEqual("<p><strong>test bold</strong></p>", Markdown2Html.Convert("**test bold**"));
        }
    }
}
