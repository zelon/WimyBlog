using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            var directory_enumerator = Directory.EnumerateDirectories(config_.PostDirectory);
            int error_count = 0;
            Parallel.ForEach(directory_enumerator, (string directory) =>
            {
                try
                {
                    var full_path = Path.GetFullPath(directory);
                    var name = Path.GetFileName(directory);
                    if (IsNumeric(name))
                    {
                        posts.Add(Post.Convert(full_path, config_));
                    }
                } catch (MetadataReader.MetadataReaderException exception)
                {
                    Console.WriteLine("Invalid metadata file. filename:{0}", exception.Message);
                    ++error_count;
                }
            });

            if (error_count > 0)
            {
                Environment.Exit(1);
            }

            posts.Sort((Post left, Post right) => right.Id.CompareTo(left.Id));

            return posts;
        }

        private static bool IsNumeric(string value)
        {
            return Int32.TryParse(value, out int _);
        }

    }
}
