﻿using System.IO;
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
                                      post.Id, post.CreatedLocalTime.ToString("yyyy-MM-dd"), post.Title);
                body += System.Environment.NewLine;
            }

            body += GetPageLinkHtml(current_page_count, total_page_count);

            string output = config_.Layout;

            output = output.Replace("<!--wimyblog:post_title-->", $"page {current_page_count}");
            output = output.Replace("<!--wimyblog:content-->", body);

            return output;
        }

        public static string GetPageLinkHtml(int currentPageCount, int totalPageCount)
        {
            string body = "";
            body += "<p align='center'>";
            if (currentPageCount > 1)
            {
                body += string.Format("<a href='/page/{0}'>&lt;&lt;</a>", currentPageCount - 1);
            }
            else
            {
                body += string.Format("&lt;&lt;");
            }
            body += "&nbsp;&nbsp;&nbsp;&nbsp;";
            if (currentPageCount < totalPageCount)
            {
                body += string.Format("<a href='/page/{0}'>&gt;&gt;</a>", currentPageCount + 1);
            }
            else
            {
                body += string.Format("&gt;&gt;");
            }
            body += "</p>";

            return body;
        }
    }
}
