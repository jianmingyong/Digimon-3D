using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace D3D.Content.Pipeline.Shaders;

[ContentProcessor(DisplayName = "BasicEffectWithAlphaTest - D3D")]
internal class BasicEffectWithAlphaTestProcessor : ContentProcessor<EffectContent, BasicEffectWithAlphaTestContent>
{
    public EffectProcessorDebugMode DebugMode { get; set; }

    public string Defines { get; set; }

    public override BasicEffectWithAlphaTestContent Process(EffectContent input, ContentProcessorContext context)
    {
        var effectProcessor = new EffectProcessor { DebugMode = DebugMode, Defines = Defines };
        var result = effectProcessor.Process(input, context);

        return new BasicEffectWithAlphaTestContent(result.GetEffectCode());
    }
}