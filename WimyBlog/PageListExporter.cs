using System.IO;
using System.Collections.Generic;

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
                    string output = CreatePageBody(posts, current_page_count, total_page_count);
                    stream.Write(output);
                }
            }
        }

        private static string CreatePageBody(List<Post> posts,
                                             int current_page_count,
                                             int total_page_count)
        {
            string output = string.Format("page {0}:", current_page_count);
            foreach (var post in posts)
            {
                output += " " + post.Id;
            }
            return output;
        }
    }
}
