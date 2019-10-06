
namespace WimyBlog
{
    public class Markdown2Html
    {
        public static string Convert(string markdownContent)
        {
            MarkdownSharp.Markdown markdown = new MarkdownSharp.Markdown
            {
                AutoNewLines = true,
                AutoHyperlink = true
            };
            return markdown.Transform(markdownContent);
        }
    }
}
