using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace WimyBlog
{
    class HtmlExporter
    {
        private Config config_;
        private List<Post> posts_;
        public HtmlExporter(List<Post> posts, Config config)
        {
            posts_ = posts;
            config_ = config;
        }

        public void Export()
        {
            foreach (var post in posts_)
            {
                string filename = Path.Combine(config_.PostDirectory, post.Id.ToString(), "index.html");
                using (var stream = File.CreateText(filename))
                {
                    stream.Write(GetHtmlWithLayout(post));
                }
            }
        }

        private string GetHtmlWithLayout(Post post)
        {
            string layout_filename = Path.Combine(config_.RootDirectory, "layout_post.html");
            string layout;
            using (var stream = File.OpenText(layout_filename))
            {
                layout = stream.ReadToEnd();
            }
            string output = layout;

            output = output.Replace("<!--wimyblog:title-->",
                                    string.Format("<a href=\"/{0}\">{1}</a>", post.Id, post.Title));
            output = output.Replace("<!--wimyblog:content-->", post.HtmlContent);

            return output;
        }
    }
}
