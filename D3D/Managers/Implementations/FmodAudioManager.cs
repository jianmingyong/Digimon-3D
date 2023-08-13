using System;
using D3D.Content.Audio;
using D3D.Content.Utilities;
using FMOD;
using Microsoft.Xna.Framework;

namespace D3D.Managers.Implementations;

public sealed class FmodAudioManager : IFmodBackgroundMusicSystem, IFmodSoundEffectSystem, IDisposable
{
    /// <summary>
    ///     Background Music volume.
    /// </summary>
    /// <param name="value">
    ///     Volume level. 0 = silent, 1 = full. Negative level inverts the signal. Values larger than 1 amplify
    ///     the signal.
    /// </param>
    /// <returns>Volume level</returns>
    public float BackgroundMusicVolume
    {
        get
        {
            _backgroundMusicChannelGroup.getVolume(out var volume).ThrowOnError();
            return volume;
        }

        set => _backgroundMusicChannelGroup.setVolume(value).ThrowOnError();
    }

    /// <summary>
    ///     Background Music mute state.
    /// </summary>
    /// <param name="value">Mute state. True = silent. False = audible.</param>
    /// <returns>Mute state.</returns>
    public bool BackgroundMusicIsMute
    {
        get
        {
            _backgroundMusicChannelGroup.getMute(out var mute).ThrowOnError();
            return mute;
        }

        set => _backgroundMusicChannelGroup.setMute(value).ThrowOnError();
    }

    /// <summary>
    ///     Sound Effect volume.
    /// </summary>
    /// <param name="value">
    ///     Volume level. 0 = silent, 1 = full. Negative level inverts the signal. Values larger than 1 amplify
    ///     the signal.
    /// </param>
    /// <returns>Volume level</returns>
    public float SoundEffectVolume
    {
        get
        {
            _soundEffectChannelGroup.getVolume(out var volume).ThrowOnError();
            return volume;
        }

        set => _soundEffectChannelGroup.setVolume(value).ThrowOnError();
    }
    
    /// <summary>
    ///     Sound Effect mute state.
    /// </summary>
    /// <param name="value">Mute state. True = silent. False = audible.</param>
    /// <returns>Mute state.</returns>
    public bool SoundEffectIsMute
    {
        get
        {
            _soundEffectChannelGroup.getMute(out var mute).ThrowOnError();
            return mute;
        }

        set => _soundEffectChannelGroup.setMute(value).ThrowOnError();
    }

    FMOD.System IFmodSystem.System => _system;

    ChannelGroup IFmodBackgroundMusicSystem.ChannelGroup => _backgroundMusicChannelGroup;

    ChannelGroup IFmodSoundEffectSystem.ChannelGroup => _soundEffectChannelGroup;

    private FMOD.System _system;
    private ChannelGroup _backgroundMusicChannelGroup;
    private ChannelGroup _soundEffectChannelGroup;

    public FmodAudioManager(GameServiceContainer container)
    {
        Factory.System_Create(out _system).ThrowOnError();

        _system.init(1024, INITFLAGS.NORMAL, new IntPtr((int) OUTPUTTYPE.AUTODETECT)).ThrowOnError();

        container.AddService(typeof(IFmodBackgroundMusicSystem), this);
        container.AddService(typeof(IFmodSoundEffectSystem), this);

        _system.createChannelGroup("BGM", out _backgroundMusicChannelGroup).ThrowOnError();
        _system.createChannelGroup("SE", out _soundEffectChannelGroup).ThrowOnError();
    }

    public void Dispose()
    {
        if (_backgroundMusicChannelGroup.hasHandle())
        {
            _backgroundMusicChannelGroup.release().ThrowOnError();
        }

        if (_soundEffectChannelGroup.hasHandle())
        {
            _soundEffectChannelGroup.release().ThrowOnError();
        }

        if (_system.hasHandle())
        {
            _system.release().ThrowOnError();
        }
    }
}