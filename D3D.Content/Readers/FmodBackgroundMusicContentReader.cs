using D3D.Content.Audio;
using D3D.Content.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Content;

namespace D3D.Content.Readers;

public class FmodBackgroundMusicContentReader : ContentTypeReader<FmodBackgroundMusic>
{
    protected override FmodBackgroundMusic Read(ContentReader input, FmodBackgroundMusic existingInstance)
    {
        var fmodSystem = input.GetServiceProvider().GetRequiredService<IFmodBackgroundMusicSystem>();
        var fileName = Path.Combine(input.ContentManager.RootDirectory, Path.GetDirectoryName(input.AssetName)!, input.ReadString());

        return new FmodBackgroundMusic(fmodSystem, fileName.Replace("\\", "/"), input.ReadUInt32(), input.ReadUInt32());
    }
}