using System.IO;
using D3D.Content.Audio;
using Microsoft.Xna.Framework.Content;

namespace D3D.Content.Readers;

public class FmodBackgroundMusicContentReader : ContentTypeReader<FmodBackgroundMusic>
{
    protected override FmodBackgroundMusic Read(ContentReader input, FmodBackgroundMusic existingInstance)
    {
        var fmodSystem = (IFmodBackgroundMusicSystem) input.ContentManager.ServiceProvider.GetService(typeof(IFmodBackgroundMusicSystem))!;
        if (fmodSystem is null) throw new ContentLoadException("FMOD system is not registered as a service.");
        
        var fileName = Path.Combine(input.ContentManager.RootDirectory, input.AssetName + Path.GetExtension(input.ReadString()));

        return new FmodBackgroundMusic(fmodSystem, fileName.Replace("\\", "/"), input.ReadUInt32(), input.ReadUInt32());
    }
}