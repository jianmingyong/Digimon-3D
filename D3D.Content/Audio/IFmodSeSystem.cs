namespace D3D.Content.Audio;

public interface IFmodSeSystem
{
    FMOD.System System { get; }
    
    FMOD.ChannelGroup SeChannelGroup { get; }
}