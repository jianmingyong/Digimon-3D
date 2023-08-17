using D3D.Content.Utilities;
using FMOD;
using Microsoft.Xna.Framework;

namespace D3D.Content.Audio;

public abstract class FmodSound : IDisposable
{
    /// <summary>
    /// Name of the sound.
    /// </summary>
    public string Name => _name;

    /// <summary>
    /// Type of sound.
    /// </summary>
    public SOUND_TYPE Type => _type;

    /// <summary>
    /// Format of the sound.
    /// </summary>
    public SOUND_FORMAT Format => _format;

    /// <summary>
    /// Number of channels.
    /// </summary>
    public int Channels => _channels;

    /// <summary>
    /// Number of bits per sample, corresponding to <see cref="Format"/>.
    /// </summary>
    public int Bits => _bits;
    
    /// <summary>
    /// Sound length.
    /// </summary>
    public TimeSpan Length { get; private set; }

    public uint LoopStart => _loopStart;

    public uint LoopEnd => _loopEnd;
    
    public bool IsPlaying
    {
        get
        {
            if (!channel.hasHandle()) return false;
            if (channel.getPaused(out var isPaused) == RESULT.OK) return !isPaused;
            return false;
        }
    }

    protected FMOD.System System => _fmodSystem.System;
    protected abstract ChannelGroup ChannelGroup { get; }
    
    protected Sound sound;
    protected Channel channel;

    private readonly IFmodSystem _fmodSystem;
    
    private readonly string _name;
    private readonly SOUND_TYPE _type;
    private readonly SOUND_FORMAT _format;
    private readonly int _channels;
    private readonly int _bits;
    private uint _loopStart;
    private uint _loopEnd;

    protected FmodSound(IFmodSystem fmodSystem, string filePath, MODE mode)
    {
        _fmodSystem = fmodSystem;
        
        string path;
        
        using var stream = (FileStream) TitleContainer.OpenStream(filePath);
        {
            path = stream.Name;
        }

        System.createSound(path, mode, out sound).ThrowOnError();
        
        sound.getName(out _name, 256).ThrowOnError();
        sound.getFormat(out _type, out _format, out _channels, out _bits).ThrowOnError();
        sound.getLength(out var length, TIMEUNIT.MS).ThrowOnError();
        sound.getLoopPoints(out _loopStart, TIMEUNIT.PCM, out _loopEnd, TIMEUNIT.PCM).ThrowOnError();
        
        Length = TimeSpan.FromMilliseconds(length);
    }
    
    internal FmodSound(IFmodSystem fmodSystem, byte[] data, MODE mode)
    {
        _fmodSystem = fmodSystem;
        
        var exInfo = new CREATESOUNDEXINFO
        {
            cbsize = MarshalHelper.SizeOf(typeof(CREATESOUNDEXINFO))
        };

        System.createSound(data, mode, ref exInfo, out sound).ThrowOnError();
        
        sound.getName(out _name, 256).ThrowOnError();
        sound.getFormat(out _type, out _format, out _channels, out _bits).ThrowOnError();
        sound.getLength(out var length, TIMEUNIT.MS).ThrowOnError();
        sound.getLoopPoints(out _loopStart, TIMEUNIT.PCM, out _loopEnd, TIMEUNIT.PCM).ThrowOnError();
        
        Length = TimeSpan.FromMilliseconds(length);
    }

    ~FmodSound()
    {
        Dispose(false);
    }

    /// <summary>
    /// Plays a Sound on a Channel.
    /// </summary>
    public virtual void Play()
    {
        if (channel.hasHandle() && channel.getPaused(out var isPaused) == RESULT.OK && isPaused)
        {
            channel.setPaused(false).ThrowOnError();
            return;
        }

        System.playSound(sound, ChannelGroup, false, out channel).ThrowOnError();
    }

    /// <summary>
    /// Pause the Channel from playing.
    /// </summary>
    public virtual void Pause()
    {
        if (channel.hasHandle() && channel.getPaused(out var isPaused) == RESULT.OK && !isPaused)
        {
            channel.setPaused(true).ThrowOnError();
        }
    }

    /// <summary>
    /// Resume the Channel from playing.
    /// </summary>
    public virtual void Resume()
    {
        if (channel.hasHandle() && channel.getPaused(out var isPaused) == RESULT.OK && isPaused)
        {
            channel.setPaused(false).ThrowOnError();
        }
    }

    /// <summary>
    /// Stops the Channel from playing.
    /// </summary>
    public virtual void Stop()
    {
        if (channel.stop() == RESULT.OK)
        {
            channel.clearHandle();
        }
    }
    
    /// <summary>
    /// Sets the loop points within a sound.
    /// </summary>
    /// <param name="loopStart">Loop start point in PCM format.</param>
    /// <param name="loopEnd">Loop end point in PCM format.</param>
    public virtual void SetLoopPoints(uint loopStart, uint loopEnd)
    {
        sound.setLoopPoints(loopStart, TIMEUNIT.PCM, loopEnd, TIMEUNIT.PCM).ThrowOnError();
        
        _loopStart = loopStart;
        _loopEnd = loopEnd;
    }

    private void ReleaseUnmanagedResources()
    {
        if (System.hasHandle() && sound.hasHandle() && sound.release() == RESULT.OK)
        {
            sound.clearHandle();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();
    }
}