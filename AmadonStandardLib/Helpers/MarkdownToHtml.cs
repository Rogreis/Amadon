using Markdig;

namespace AmadonStandardLib.Helpers
{
    public class MarkdownToHtml
    {
        public static string Convert(string markdown)
        {
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            return Markdown.ToHtml(markdown, pipeline);
        }

    }
}
