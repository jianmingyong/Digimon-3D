using FMOD;

namespace D3D.Content.Audio;

public sealed class FmodSoundEffect : FmodSound
{
    protected override ChannelGroup ChannelGroup { get; }
    
    public FmodSoundEffect(IFmodSoundEffectSystem system, string filePath) : base(system, filePath, MODE._2D | MODE.LOOP_OFF)
    {
        ChannelGroup = system.ChannelGroup;
    }
    
    internal FmodSoundEffect(IFmodSoundEffectSystem system, byte[] data) : base(system, data, MODE._2D | MODE.LOOP_OFF)
    {
        ChannelGroup = system.ChannelGroup;
    }

    public override void SetLoopPoints(uint loopStart, uint loopEnd)
    {
        throw new NotSupportedException("Setting loop points on sound effect is not supported.");
    }
}