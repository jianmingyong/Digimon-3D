using D3D.Content.Audio;
using Microsoft.Xna.Framework.Content;

namespace D3D.Content.Readers;

public class FmodSoundEffectContentReader : ContentTypeReader<FmodSoundEffect>
{
    protected override FmodSoundEffect Read(ContentReader input, FmodSoundEffect existingInstance)
    {
        var fmodSystem = (IFmodSoundEffectSystem) input.ContentManager.ServiceProvider.GetService(typeof(IFmodSoundEffectSystem))!;
        if (fmodSystem is null) throw new ContentLoadException("FMOD system is not registered as a service.");

        var dataLength = input.ReadInt32();

        return new FmodSoundEffect(fmodSystem, input.ReadBytes(dataLength));
    }
}