using FMOD;

namespace D3D.Content.Audio;

public interface IFmodSoundEffectSystem : IFmodSystem
{
    ChannelGroup ChannelGroup { get; }
}