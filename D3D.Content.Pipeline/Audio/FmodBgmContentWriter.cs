using D3D.Content.Audio;
using D3D.Content.Readers;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace D3D.Content.Pipeline.Audio;

[ContentTypeWriter]
internal class FmodBgmContentWriter : ContentTypeWriter<FmodBgmContent>
{
    public override string GetRuntimeReader(TargetPlatform targetPlatform)
    {
        return $"{typeof(FmodBgmContentReader).FullName}, {typeof(FmodBgmSound).Assembly.FullName}";
    }

    protected override void Write(ContentWriter output, FmodBgmContent value)
    {
        output.Write(value.FileName);
        output.Write(value.LoopStart);
        output.Write(value.LoopEnd);
    }
}