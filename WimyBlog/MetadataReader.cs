using System;
using System.IO;
using System.Xml;

namespace WimyBlog
{
    class MetadataReader
    {
        public string Title { get; private set; }
        public DateTime CreatedTime { get; private set; }
        private Config config_;

        public MetadataReader(string filename, Config config)
        {
            config_ = config;

            if (File.Exists(filename) == false)
            {
                CreateDefaultMetadataFile(filename);
            }
            System.Diagnostics.Debug.Assert(File.Exists(filename));
            var file_stream = File.OpenRead(filename);
            var xml_document = new XmlDocument();
            try
            {
                xml_document.Load(file_stream);
            } catch (XmlException exception)
            {
                var new_exception = new MetadataReaderException(exception.Message + " filename: " + filename);
                throw new_exception;
            }
            Parse(xml_document);
        }

        public class MetadataReaderException : XmlException {
            public MetadataReaderException(string msg) : base(msg)
            {
            }
        }

        private void Parse(XmlDocument xml_document)
        {
            Title = xml_document.GetElementsByTagName("title")[0].InnerText;
            CreatedTime = DateTime.Parse(xml_document.GetElementsByTagName("created_time")[0].InnerText);
        }

        private void CreateDefaultMetadataFile(string filename)
        {
            string data = string.Format(
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine +
                "<metadata>" + Environment.NewLine +
                "  <title></title>" + Environment.NewLine +
                "  <created_time>{0}</created_time>" + Environment.NewLine +
                "</metadata>", DateTime.Now.ToString(config_.DateTimeFormat));

            using (var file_stream = File.CreateText(filename))
            {
                file_stream.Write(data);
            }
        }
    }
}
