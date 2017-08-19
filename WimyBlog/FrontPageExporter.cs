using System.Collections.Generic;
using System.IO;

namespace WimyBlog
{
    class FrontPageExporter
    {
        private static int kPostCount = 10;
        private Config config_;
        private List<Post> posts_;

        public FrontPageExporter(List<Post> posts, Config config)
        {
            config_ = config;
            Collect(posts);
        }

        private void Collect(List<Post> posts)
        {
            posts_ = new List<Post>();
            foreach (var post in posts)
            {
                posts_.Add(post);
                if (posts_.Count == kPostCount)
                {
                    break;
                }
            }
        }

        public void Export()
        {
            string output_filename = Path.Combine(config_.RootDirectory, "index.html");
            using (var stream = File.CreateText(output_filename))
            {
                string output = CreateBody();
                stream.Write(output);
            }
        }

        private string CreateBody()
        {
            string body = "";
            foreach (var post in posts_)
            {
                body += string.Format("<p><a href='/{0}'>{1}&nbsp;&nbsp;&nbsp;{2}</a></p>",
                                      post.Id, post.CreatedTime.ToString("yyyy-MM-dd"), post.Title);
                body += System.Environment.NewLine;
            }

            string output = config_.Layout;
            output = output.Replace("<!--wimyblog:content-->", body);
            return output;
        }
    }
}
