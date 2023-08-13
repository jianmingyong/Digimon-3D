using D3D.Content.Utilities;
using FMOD;

namespace D3D.Content.Audio;

public sealed class FmodBackgroundMusic : FmodSound
{
    protected override ChannelGroup ChannelGroup { get; }

    internal FmodBackgroundMusic(IFmodBackgroundMusicSystem system, string fileName, uint loopStart, uint loopEnd) : base(system, fileName, MODE._2D | MODE.LOOP_NORMAL | MODE.CREATESTREAM)
    {
        ChannelGroup = system.ChannelGroup;
        SetLoopPoints(loopStart, loopEnd);
    }

    public override void Play()
    {
        if (Channel.hasHandle())
        {
            Resume();
            return;
        }
        
        ChannelGroup.stop().ThrowOnError();
        System.playSound(Sound, ChannelGroup, false, out Channel).ThrowOnError();
    }
}