using System.IO;
using D3D.Content.Audio;
using Microsoft.Xna.Framework.Content;

namespace D3D.Content.Readers;

public sealed class FmodBgmContentReader : ContentTypeReader<FmodBgmSound>
{
    protected override FmodBgmSound Read(ContentReader input, FmodBgmSound existingInstance)
    {
        var fileName = Path.Combine(input.ContentManager.RootDirectory, input.AssetName + Path.GetExtension(input.ReadString()));
        var fmodSystem = (IFmodBgmSystem) input.ContentManager.ServiceProvider.GetService(typeof(IFmodBgmSystem))!;
        if (fmodSystem is null) throw new ContentLoadException("FMOD system is not registered as a service.");
        
        return new FmodBgmSound(fmodSystem, fileName.Replace("\\", "/"), input.ReadUInt32(), input.ReadUInt32());
    }
}