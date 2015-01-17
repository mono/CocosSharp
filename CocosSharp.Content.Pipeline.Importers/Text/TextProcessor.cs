using Microsoft.Xna.Framework.Content.Pipeline;

using TInput = System.String;
using TOutput = CocosSharp.CCContent;

namespace CocosSharp.Content.Pipeline.Importers
{
    [ContentProcessor(DisplayName = "Text - CocosSharp")]
    internal sealed class TextProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            var result = new TOutput();
            result.Content = input;

            return result;
        }
    }
}