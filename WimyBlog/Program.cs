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
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

            PostCollector post_collector = new PostCollector(config);
            Console.Write("Collecting posts...");
            stopwatch.Start();
            List<Post> posts = post_collector.Collect();
            Console.WriteLine($" DONE -> collected count: {posts.Count},elapsed milliseconds: {stopwatch.ElapsedMilliseconds}");

            ValidatePosts(posts);

            Console.Write("Exporting index.html files...");
            stopwatch.Restart();
            new HtmlExporter(posts, config).Export();
            Console.WriteLine($" DONE -> elapsed milliseconds: {stopwatch.ElapsedMilliseconds}");

            Console.Write("Exporting page lists...");
            stopwatch.Restart();
            new PageListExporter(posts, config).Export();
            Console.WriteLine($" DONE -> elapsed milliseconds: {stopwatch.ElapsedMilliseconds}");

            Console.Write("Exporting front page...");
            stopwatch.Restart();
            new FrontPageExporter(posts, config).Export();
            Console.WriteLine($" DONE -> elapsed milliseconds: {stopwatch.ElapsedMilliseconds}");

            Console.Write("Exporting rss...");
            stopwatch.Restart();
            new RssExporter(posts, config).Export();
            Console.WriteLine($" DONE -> elapsed milliseconds: {stopwatch.ElapsedMilliseconds}");

            Console.WriteLine("Completed");
        }

        private static void ValidatePosts(List<Post> posts)
        {
            if (posts.Count == 0)
            {
                Console.WriteLine("Cannot find any post");
                Environment.Exit(1);
            }
            foreach (Post post in posts)
            {
                if (string.IsNullOrEmpty(post.Title))
                {
                    string errorMessage = string.Format("[ERROR] There is no title. Id:{0},Directory:{1}",
                        post.Id, post.ContentDirectoryName);
                    Console.WriteLine(errorMessage);
                    System.Diagnostics.Debug.WriteLine(errorMessage);
                    Environment.Exit(1);
                }
            }
        }
    }
}
