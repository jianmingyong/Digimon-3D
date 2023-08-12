using System;
using System.IO;
using D3D.Content.Utilities;
using FMOD;
using Microsoft.Xna.Framework;

namespace D3D.Content.Audio;

public sealed class FmodBgmSound : IDisposable
{
    public string FileName { get; }

    public string Name => _name;

    public SOUND_TYPE Type => _type;

    public SOUND_FORMAT Format => _format;

    public int Channels => _channels;

    public int Bits => _bits;
    
    public TimeSpan Length { get; private set; }

    public bool IsPlaying
    {
        get
        {
            if (!_channel.hasHandle()) return false;
            _channel.getPaused(out var paused).ThrowOnError();
            return !paused;
        }
    }
    
    private readonly IFmodBgmSystem _fmodBgmSystem;
    private Sound _sound;
    private Channel _channel;

    private readonly string _name;
    private readonly SOUND_TYPE _type;
    private readonly SOUND_FORMAT _format;
    private readonly int _channels;
    private readonly int _bits;

    internal FmodBgmSound(IFmodBgmSystem fmodBgmSystem, string fileName, uint loopStart, uint loopEnd)
    {
        _fmodBgmSystem = fmodBgmSystem;
        FileName = fileName;

        string path;
        
        using var stream = (FileStream) TitleContainer.OpenStream(FileName);
        {
            path = stream.Name;
        }

        fmodBgmSystem.System.createStream(path, MODE._2D | MODE.LOOP_NORMAL, out _sound).ThrowOnError();

        if (loopStart != 0)
        {
            _sound.setLoopPoints(loopStart, TIMEUNIT.PCM, loopEnd, TIMEUNIT.PCM).ThrowOnError();
        }

        _sound.getName(out _name, 256).ThrowOnError();
        _sound.getFormat(out _type, out _format, out _channels, out _bits).ThrowOnError();
        _sound.getLength(out var length, TIMEUNIT.MS).ThrowOnError();
        
        Length = TimeSpan.FromMilliseconds(length);
    }

    public void Play()
    {
        _fmodBgmSystem.BgmChannelGroup.stop().ThrowOnError();
        _fmodBgmSystem.System.playSound(_sound, _fmodBgmSystem.BgmChannelGroup, false, out _channel).ThrowOnError();
    }

    public void Pause()
    {
        if (!_channel.hasHandle()) return;
        _channel.setPaused(true).ThrowOnError();
    }

    public void Resume()
    {
        if (!_channel.hasHandle()) return;
        _channel.setPaused(false).ThrowOnError();
    }

    public void Stop()
    {
        if (!_channel.hasHandle()) return;
        _channel.stop().ThrowOnError();
    }

    public void Dispose()
    {
        _sound.release();
    }
}