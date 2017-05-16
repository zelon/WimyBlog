using System;
using System.IO;
using System.Collections.Generic;

namespace WimyBlog
{
    class PostCollector
    {
        private Config config_;

        public PostCollector(Config config)
        {
            config_ = config;
        }

        public List<Post> Collect()
        {
            List<Post> posts = new List<Post>();
            foreach (var directory in Directory.EnumerateDirectories(config_.PostDirectory))
            {
                var full_path = Path.GetFullPath(directory);
                var name = Path.GetFileName(directory);
                if (IsNumeric(name))
                {
                    posts.Add(Post.Convert(full_path, config_));
                }
            }
            return posts;
        }

        private static bool IsNumeric(string value)
        {
            return Int32.TryParse(value, out int _);
        }

    }
}
