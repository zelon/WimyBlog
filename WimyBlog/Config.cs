using System;
using System.IO;
using System.Xml;

namespace WimyBlog
{
    public class Config
    {
        public string Author { get; private set; }
        public string SiteBaseUrl { get; private set; }
        public string BlogDescription { get; private set; }
        public string DateTimeFormat { get; private set; }
        public string RootDirectory { get; private set; }
        public string PostDirectory { get; private set; }
        public string PageDirectory { get; private set; }
        public string TimezoneString { get; private set; }

        public static Config Parse(string filename)
        {
            if (File.Exists(filename) == false)
            {
                Console.WriteLine("Cannot find config.xml");
                Environment.Exit(1);
            }

            XmlDocument xml_document = new XmlDocument();
            using (var file_stream = File.OpenText(filename))
            {
                xml_document.Load(file_stream);
            }
            return CreateConfig(xml_document["wimyblog"], Path.GetDirectoryName(filename));
        }

        private static Config CreateConfig(XmlNode node, string base_directory_name)
        {
            Config config = new Config();

            config.Author = node["author"].InnerText;
            config.SiteBaseUrl = node["site_base_url"].InnerText;
            config.BlogDescription = node["blog_description"].InnerText;
            config.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            config.RootDirectory = base_directory_name;
            config.PostDirectory = Path.Combine(base_directory_name, node["post_directory"].InnerText);
            config.PageDirectory = Path.Combine(base_directory_name, node["page_directory"].InnerText);

            if (config.SiteBaseUrl.EndsWith("/") == false)
            {
                config.SiteBaseUrl += "/";
            }

            return config;
        }
    }
}
