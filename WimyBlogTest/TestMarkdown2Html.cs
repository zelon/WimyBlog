﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimyBlog;

namespace WimyBlogTest
{
    [TestClass]
    public class TestMarkdown2Html
    {
        [TestMethod]
        public void TestBold()
        {
            Assert.AreEqual("<p><strong>test bold</strong></p>\n", Markdown2Html.Convert("**test bold**"));
        }

        [TestMethod]
        public void TestEndLine()
        {
            Assert.AreEqual("<p>a<br />\nb</p>\n", Markdown2Html.Convert("a\nb"));
        }
    }
}
