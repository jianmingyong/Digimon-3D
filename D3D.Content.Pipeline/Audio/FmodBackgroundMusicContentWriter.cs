using D3D.Content.Audio;
using D3D.Content.Readers;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace D3D.Content.Pipeline.Audio;

[ContentTypeWriter]
public class FmodBackgroundMusicContentWriter : ContentTypeWriter<FmodBackgroundMusicContent>
{
    public override string GetRuntimeReader(TargetPlatform targetPlatform)
    {
        return $"{typeof(FmodBackgroundMusicContentReader).FullName}, {typeof(FmodBackgroundMusic).Assembly.FullName}";
    }

    protected override void Write(ContentWriter output, FmodBackgroundMusicContent value)
    {
        output.Write(value.FileName);
        output.Write(value.LoopStart);
        output.Write(value.LoopEnd);
    }
}