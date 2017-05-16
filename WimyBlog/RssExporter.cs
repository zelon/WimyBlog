using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;

// online validator: http://www.feedvalidator.org

// reference links:
//  http://www.intertwingly.net/wiki/pie/Rss20AndAtom10Compared
//  http://naearu.tistory.com/2982748
//  https://ko.wikipedia.org/wiki/RSS
//  https://www.w3schools.com/xml/xml_rss.asp

namespace WimyBlog
{
    class RssExporter
    {
        private Config config_;
        private XmlDocument document_;
        private const int kMaxItemCount = 50;

        public RssExporter(List<Post> posts, Config config)
        {
            config_ = config;
            document_ = new XmlDocument();

            XmlNode doc_node = document_.CreateXmlDeclaration("1.0", "UTF-8", null);
            document_.AppendChild(doc_node);

            XmlNode rss = CreateRssElement();
            document_.AppendChild(rss);

            XmlNode channel = document_.CreateElement("channel");
            rss.AppendChild(channel);

            channel.AppendChild(CreateElement("title", config.SiteBaseUrl));
            channel.AppendChild(CreateElement("link", config.SiteBaseUrl));
            channel.AppendChild(CreateElement("description", HtmlEncode(config.BlogDescription)));
            channel.AppendChild(CreateElement("generator", "wimyblog"));

            int added_item_count = 0;
            foreach (Post post in posts)
            {
                var item_node = CreateItemNodeFromPost(post);
                channel.AppendChild(item_node);

                ++added_item_count;
                if (added_item_count == kMaxItemCount)
                {
                    break;
                }
            }
        }

        public void Export()
        {
            using (var stream = File.CreateText(Path.Combine(config_.RootDirectory, "rss.xml")))
            {
                document_.Save(stream);
            }
        }

        private XmlNode CreateRssElement()
        {
            XmlNode rss_node = document_.CreateElement("rss");
            var version_attribute = document_.CreateAttribute("version");
            version_attribute.InnerText = "2.0";
            rss_node.Attributes.Append(version_attribute);
            return rss_node;
        }

        private XmlNode CreateItemNodeFromPost(Post post)
        {
            XmlNode item_node = document_.CreateElement("item");

            item_node.AppendChild(CreateElement("title", HtmlEncode(post.Title)));
            item_node.AppendChild(CreateElement("link", config_.SiteBaseUrl + post.Id));
            item_node.AppendChild(CreateElement("description", HtmlEncode(post.HtmlContent)));
            item_node.AppendChild(CreateElement("guid", config_.SiteBaseUrl + post.Id));
            item_node.AppendChild(CreateElement("pubDate", post.CreatedTime.ToString(config_.DateTimeFormat)));

            return item_node;
        }

        private XmlNode CreateElement(string tag_name, string inner_text)
        {
            XmlNode node = document_.CreateElement(tag_name);
            node.InnerText = inner_text;
            return node;
        }

        private string HtmlEncode(string value)
        {
            return System.Net.WebUtility.HtmlEncode(value);
        }
    }
}
