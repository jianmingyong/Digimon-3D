using System.Text;
using D3D.Content.Utilities;
using FMOD;

namespace D3D.Content.Audio;

public sealed class FmodBackgroundMusic : FmodSound
{
    protected override ChannelGroup ChannelGroup { get; }

    public FmodBackgroundMusic(IFmodBackgroundMusicSystem system, string filePath) : this(system, filePath, 0, 0)
    {
    }
    
    internal unsafe FmodBackgroundMusic(IFmodBackgroundMusicSystem system, string filePath, uint loopStart, uint loopEnd) : base(system, filePath, MODE._2D | MODE.LOOP_NORMAL | MODE.CREATESTREAM)
    {
        ChannelGroup = system.ChannelGroup;

        if (loopStart != 0 || loopEnd != 0)
        {
            SetLoopPoints(loopStart, loopEnd);
        }
        else if (sound.getTag("LOOPSTART", 0, out var loopStartTag) == RESULT.OK &&
                 sound.getTag("LOOPLENGTH", 0, out var loopLengthTag) == RESULT.OK)
        {
            var loopStartString = Encoding.UTF8.GetString((byte*) loopStartTag.data.ToPointer(), (int) loopStartTag.datalen);
            var loopLengthString = Encoding.UTF8.GetString((byte*) loopLengthTag.data.ToPointer(), (int) loopLengthTag.datalen);

            if (uint.TryParse(loopStartString, out loopStart) && uint.TryParse(loopLengthString, out var loopLength))
            {
                SetLoopPoints(loopStart, loopStart + loopLength);
            }
        }
    }

    public override void Play()
    {
        if (channel.hasHandle() && channel.getPaused(out var isPaused) == RESULT.OK && isPaused)
        {
            channel.setPaused(false).ThrowOnError();
            return;
        }
        
        ChannelGroup.stop().ThrowOnError();
        System.playSound(sound, ChannelGroup, false, out channel).ThrowOnError();
    }
}