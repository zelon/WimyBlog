using System;
using System.IO;
using System.Collections.Generic;

namespace WimyBlog
{
    class Program
    {
        static void Main(string[] args)
        {
            string target_directory_name = ".";
            if (args.Length > 0)
            {
                target_directory_name = args[0];
            }
            Console.WriteLine("TargetDirectory: {0}", target_directory_name);

            Config config = Config.Parse(Path.Combine(target_directory_name, "config.xml"));

            PostCollector post_collector = new PostCollector(config);
            Console.WriteLine("Collecting posts...");
            List<Post> posts = post_collector.Collect();
            ValidatePosts(posts);

            Console.WriteLine("Exporting index.html files...");
            new HtmlExporter(posts, config).Export();

            Console.WriteLine("Exporting page lists...");
            new PageListExporter(posts, config).Export();

            Console.WriteLine("Exporting front page...");
            new FrontPageExporter(posts, config).Export();

            Console.WriteLine("Exporting rss...");
            new RssExporter(posts, config).Export();

            Console.WriteLine("Completed");
        }

        private static void ValidatePosts(List<Post> posts)
        {
            if (posts.Count == 0)
            {
                Console.WriteLine("Cannot find any post");
                Environment.Exit(1);
            }
            foreach (var post in posts)
            {
                if (string.IsNullOrEmpty(post.Title))
                {
                    Console.WriteLine("[ERROR] There is no title. Id: {0}", post.Id);
                    System.Diagnostics.Debug.WriteLine("[ERROR] There is no title. Id: {0}", post.Id);
                    Environment.Exit(1);
                }
            }
        }
    }
}
