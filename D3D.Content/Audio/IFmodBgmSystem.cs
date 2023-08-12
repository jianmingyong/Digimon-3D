namespace D3D.Content.Audio;

public interface IFmodBgmSystem
{
    FMOD.System System { get; }
    
    FMOD.ChannelGroup BgmChannelGroup { get; }
}