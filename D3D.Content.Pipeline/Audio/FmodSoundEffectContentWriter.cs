using D3D.Content.Readers;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace D3D.Content.Pipeline.Audio;

[ContentTypeWriter]
public class FmodSoundEffectContentWriter : ContentTypeWriter<FmodSoundEffectContent>
{
    public override string GetRuntimeReader(TargetPlatform targetPlatform)
    {
        return $"{typeof(FmodSoundEffectContentReader).FullName}, {typeof(FmodSoundEffectContentReader).Assembly.FullName}";
    }

    protected override void Write(ContentWriter output, FmodSoundEffectContent value)
    {
        output.Write(value.Data.Length);
        output.Write(value.Data);
    }
}