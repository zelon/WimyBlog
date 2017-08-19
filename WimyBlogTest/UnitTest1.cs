using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using WimyBlog;

namespace WimyBlogTest
{
    [TestClass]
    public class UnitTest1
    {
        private string test_source_directory_name_;
        private Config config_;

        public UnitTest1()
        {
            string current_dir = System.IO.Directory.GetCurrentDirectory();
            test_source_directory_name_ = Path.Combine(current_dir, "..", "..", "..", "test_source");

            config_ = new Config();
        }

        [TestMethod]
        public void TestExistTestSource()
        {
            string post_directory_name = Path.Combine(test_source_directory_name_, "post", "1");
            string metadata_filename = Path.Combine(post_directory_name, "metadata.xml");
            string markdown_filename = Path.Combine(post_directory_name, "index.md");

            Assert.IsTrue(File.Exists(metadata_filename));
            Assert.IsTrue(File.Exists(markdown_filename));
        }

        [TestMethod]
        public void TestPostRead()
        {
            string test_post_directory_name = Path.Combine(test_source_directory_name_, "post", "1");
            Post post = Post.Convert(test_post_directory_name, config_);

            Assert.AreEqual(1, post.Id);
            Assert.AreEqual("Test 타이틀", post.Title);
            Assert.AreEqual("<p>Test 테스트</p>", post.HtmlContent);
            Assert.AreEqual(DateTime.Parse("2017-06-07 23:25:05").ToBinary(), post.CreatedLocalTime.ToBinary());
        }
    }
}
