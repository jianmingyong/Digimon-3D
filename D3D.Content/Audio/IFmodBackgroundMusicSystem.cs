using FMOD;

namespace D3D.Content.Audio;

public interface IFmodBackgroundMusicSystem : IFmodSystem
{
    ChannelGroup ChannelGroup { get; }
}