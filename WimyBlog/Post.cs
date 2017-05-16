using System;
using System.IO;

namespace WimyBlog
{
    public class Post
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string HtmlContent { get; private set; }
        public DateTime CreatedTime { get; private set; }
        public string ContentDirectoryName { get; private set; }

        public static Post Convert(string directory_name, Config config)
        {
            string metadata_filename = Path.Combine(directory_name, "metadata.xml");
            var metadata_reader = new MetadataReader(metadata_filename, config);
            string markdown_content = GetMarkdownContent(directory_name);

            Post post = new Post();
            post.Id = int.Parse(Path.GetFileName(directory_name));
            post.Title = metadata_reader.Title;
            post.HtmlContent = Markdown2Html.Convert(markdown_content);
            post.CreatedTime = metadata_reader.CreatedTime;
            post.ContentDirectoryName = directory_name;
            return post;
        }

        private static string GetMarkdownContent(string directory_name)
        {
            string filename = Path.Combine(directory_name, "index.md");
            using (var stream = File.OpenText(filename))
            {
                return stream.ReadToEnd();
            }
        }
    }
}
