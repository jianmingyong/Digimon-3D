using System;
using System.IO;
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
            if (!Channel.hasHandle()) return false;
            Channel.getPaused(out var paused).ThrowOnError();
            return !paused;
        }
    }

    protected FMOD.System System => _fmodSystem.System;
    protected abstract ChannelGroup ChannelGroup { get; }
    
    protected Sound Sound;
    protected Channel Channel;

    private readonly IFmodSystem _fmodSystem;
    
    private readonly string _name;
    private readonly SOUND_TYPE _type;
    private readonly SOUND_FORMAT _format;
    private readonly int _channels;
    private readonly int _bits;
    private uint _loopStart;
    private uint _loopEnd;

    protected FmodSound(IFmodSystem fmodSystem, string fileName, MODE mode)
    {
        _fmodSystem = fmodSystem;
        
        string path;
        
        using var stream = (FileStream) TitleContainer.OpenStream(fileName);
        {
            path = stream.Name;
        }

        System.createSound(path, mode, out Sound).ThrowOnError();
        
        Sound.getName(out _name, 256).ThrowOnError();
        Sound.getFormat(out _type, out _format, out _channels, out _bits).ThrowOnError();
        Sound.getLength(out var length, TIMEUNIT.MS).ThrowOnError();
        Sound.getLoopPoints(out _loopStart, TIMEUNIT.PCM, out _loopEnd, TIMEUNIT.PCM).ThrowOnError();
        
        Length = TimeSpan.FromMilliseconds(length);
    }
    
    internal FmodSound(IFmodSystem fmodSystem, byte[] data, MODE mode)
    {
        _fmodSystem = fmodSystem;
        
        var exInfo = new CREATESOUNDEXINFO
        {
            cbsize = MarshalHelper.SizeOf(typeof(CREATESOUNDEXINFO))
        };

        System.createSound(data, mode, ref exInfo, out Sound).ThrowOnError();
        
        Sound.getName(out _name, 256).ThrowOnError();
        Sound.getFormat(out _type, out _format, out _channels, out _bits).ThrowOnError();
        Sound.getLength(out var length, TIMEUNIT.MS).ThrowOnError();
        Sound.getLoopPoints(out _loopStart, TIMEUNIT.PCM, out _loopEnd, TIMEUNIT.PCM).ThrowOnError();
        
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
        if (Channel.hasHandle())
        {
            Resume();
            return;
        }

        System.playSound(Sound, ChannelGroup, false, out Channel).ThrowOnError();
    }

    /// <summary>
    /// Pause the Channel from playing.
    /// </summary>
    public virtual void Pause()
    {
        if (!Channel.hasHandle()) return;
        Channel.setPaused(true).ThrowOnError();
    }

    /// <summary>
    /// Resume the Channel from playing.
    /// </summary>
    public virtual void Resume()
    {
        if (!Channel.hasHandle()) return;
        Channel.setPaused(false).ThrowOnError();
    }

    /// <summary>
    /// Stops the Channel from playing.
    /// </summary>
    public virtual void Stop()
    {
        if (!Channel.hasHandle()) return;
        Channel.stop().ThrowOnError();
    }
    
    /// <summary>
    /// Sets the loop points within a sound.
    /// </summary>
    /// <param name="loopStart">Loop start point in PCM format.</param>
    /// <param name="loopEnd">Loop end point in PCM format.</param>
    public virtual void SetLoopPoints(uint loopStart, uint loopEnd)
    {
        Sound.setLoopPoints(loopStart, TIMEUNIT.PCM, loopEnd, TIMEUNIT.PCM).ThrowOnError();
        
        _loopStart = loopStart;
        _loopEnd = loopEnd;
    }

    private void ReleaseUnmanagedResources()
    {
        if (Sound.hasHandle())
        {
            Sound.release().ThrowOnError();
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