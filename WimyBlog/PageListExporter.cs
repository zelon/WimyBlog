using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace WimyBlog
{
    class PageListExporter
    {
        private static int kCountPerPage = 10;
        private Config config_;
        private List<List<Post>> post_pages;

        public PageListExporter(List<Post> posts, Config config)
        {
            config_ = config;

            Collect(posts);
        }

        private void Collect(List<Post> posts)
        {
            post_pages = new List<List<Post>>();
            foreach (var post in posts)
            {
                CollectAsPages(post);
            }
        }

        private void CollectAsPages(Post post)
        {
            if (post_pages.Count == 0 || post_pages[post_pages.Count-1].Count == kCountPerPage)
            {
                post_pages.Add(new List<Post>());
            }
            var list = post_pages[post_pages.Count - 1];
            list.Add(post);
        }

        public void Export()
        {
            int total_page_count = post_pages.Count;
            int current_page_count = 0;
            foreach (List<Post> posts in post_pages)
            {
                ++current_page_count;
                string output_directory_name = Path.Combine(config_.PageDirectory, current_page_count.ToString());
                Directory.CreateDirectory(output_directory_name);

                string output_filename = Path.Combine(output_directory_name, "index.html");
                using (var stream = File.CreateText(output_filename))
                {
                    string output = CreateBody(posts, current_page_count, total_page_count);
                    stream.Write(output);
                }
            }
        }

        private string CreateBody(List<Post> posts, int current_page_count, int total_page_count)
        {
            Debug.Assert(current_page_count >= 1);

            string body = "";
            foreach (var post in posts)
            {
                body += string.Format("<p><a href='/{0}'>{1}&nbsp;&nbsp;&nbsp;{2}</a></p>",
                                      post.Id, post.CreatedTime.ToString("yyyy-MM-dd"), post.Title);
                body += System.Environment.NewLine;
            }

            body += "<p align='center'>";
            if (current_page_count > 1)
            {
                body += string.Format("<a href='../{0}'>&lt;&lt;</a>", current_page_count - 1);
            }
            else
            {
                body += string.Format("&lt;&lt;");
            }
            body += "&nbsp;&nbsp;&nbsp;&nbsp;";
            if (current_page_count < total_page_count)
            {
                body += string.Format("<a href='../{0}'>&gt;&gt;</a>", current_page_count + 1);
            }
            else
            {
                body += string.Format("&gt;&gt;");
            }
            body += "</p>";


            string layout_filename = Path.Combine(config_.RootDirectory, "layout_post.html");
            string layout;
            using (var stream = File.OpenText(layout_filename))
            {
                layout = stream.ReadToEnd();
            }
            string output = layout;

            output = output.Replace("<!--wimyblog:title-->",
                                    string.Format("<a href=\".\">{0}</a>", current_page_count));
            output = output.Replace("<!--wimyblog:content-->", body);

            return output;
        }
    }
}
