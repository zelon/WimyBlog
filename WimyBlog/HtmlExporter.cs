using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            Parallel.ForEach(posts_, (Post post) =>
            {
                string filename = Path.Combine(config_.PostDirectory, post.Id.ToString(), "index.html");
                using var stream = File.CreateText(filename);
                stream.Write(GetHtmlWithLayout(post));
             });
        }

        private string GetHtmlWithLayout(Post post)
        {
            string output = config_.Layout;

			output = output.Replace("<!--wimyblog:blog_title-->",
									string.Format("{0}", config_.Title));
			output = output.Replace("<!--wimyblog:title-->",
                                    string.Format("{0}", post.Title));
			output = output.Replace("<!--wimyblog:post_title_with_link-->",
									string.Format("<a href=\"/{0}\">{1}</a>", post.Id, post.Title));
			output = output.Replace("<!--wimyblog:post_title-->",
									string.Format("{0}", post.Title));
			output = output.Replace("<!--wimyblog:post_datetime-->",
                                    string.Format("{0}", post.CreatedLocalTime.ToString(config_.DateTimeFormat)));
            output = output.Replace("<!--wimyblog:content-->", post.HtmlContent);
            output = output.Replace("<!--wimyblog:comment-->", GetCommentWithLayout(post));

            return output;
        }

        private string GetCommentWithLayout(Post post)
        {
            string layout = config_.LayoutComment;

            layout = layout.Replace("<!--wimyblog:comment_url-->", config_.GetPostLink(post.Id));
            layout = layout.Replace("<!--wimyblog:comment_identifier-->", post.Id.ToString());

            return layout;
        }
    }
}
