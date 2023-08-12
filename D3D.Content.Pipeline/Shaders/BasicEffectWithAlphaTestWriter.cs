using D3D.Content.Readers;
using D3D.Content.Shaders;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace D3D.Content.Pipeline.Shaders;

[ContentTypeWriter]
internal class BasicEffectWithAlphaTestWriter : ContentTypeWriter<BasicEffectWithAlphaTestContent>
{
    public override string GetRuntimeReader(TargetPlatform targetPlatform)
    {
        return $"{typeof(BasicEffectWithAlphaTestReader).FullName}, {typeof(BasicEffectWithAlphaTest).Assembly}";
    }

    protected override void Write(ContentWriter output, BasicEffectWithAlphaTestContent value)
    {
        output.Write(value.GetEffectCode().Length);
        output.Write(value.GetEffectCode());
    }
}