using D3D.Content.Audio;
using D3D.Content.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Content;

namespace D3D.Content.Readers;

public class FmodSoundEffectContentReader : ContentTypeReader<FmodSoundEffect>
{
    protected override FmodSoundEffect Read(ContentReader input, FmodSoundEffect existingInstance)
    {
        var fmodSystem = input.GetServiceProvider().GetRequiredService<IFmodSoundEffectSystem>();
        var dataLength = input.ReadInt32();

        return new FmodSoundEffect(fmodSystem, input.ReadBytes(dataLength));
    }
}