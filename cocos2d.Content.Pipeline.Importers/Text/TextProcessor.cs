using Microsoft.Xna.Framework.Content.Pipeline;

using TInput = System.String;
using TOutput = cocos2d.Framework.CCContent;

namespace cocos2d.Content.Pipeline.Importers
{
    [ContentProcessor(DisplayName = "TextProcessor")]
    public class TextProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            var result = new TOutput();
            result.Content = input;

            return result;
        }
    }
}