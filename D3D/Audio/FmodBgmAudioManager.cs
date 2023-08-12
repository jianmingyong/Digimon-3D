using System;
using D3D.Content.Audio;
using D3D.Content.Utilities;
using FMOD;
using Microsoft.Xna.Framework;

namespace D3D.Audio;

public sealed class FmodBgmAudioManager : IFmodBgmSystem, IFmodSeSystem, IDisposable
{
    FMOD.System IFmodBgmSystem.System => _system;
    FMOD.System IFmodSeSystem.System => _system;

    ChannelGroup IFmodBgmSystem.BgmChannelGroup => _bgmChannelGroup;
    ChannelGroup IFmodSeSystem.SeChannelGroup => _seChannelGroup;

    private FMOD.System _system;
    private ChannelGroup _bgmChannelGroup;
    private ChannelGroup _seChannelGroup;

    public FmodBgmAudioManager(Game game)
    {
        Factory.System_Create(out _system).ThrowOnError();

        _system.init(32, INITFLAGS.NORMAL, new IntPtr((int) OUTPUTTYPE.AUTODETECT)).ThrowOnError();

        game.Services.AddService(typeof(IFmodBgmSystem), this);
        game.Services.AddService(typeof(IFmodSeSystem), this);

        _system.createChannelGroup("BGM", out _bgmChannelGroup).ThrowOnError();
        _system.createChannelGroup("SE", out _seChannelGroup).ThrowOnError();
    }

    public void Dispose()
    {
        if (_bgmChannelGroup.hasHandle())
        {
            _bgmChannelGroup.release();
        }

        if (_seChannelGroup.hasHandle())
        {
            _seChannelGroup.release();
        }

        if (_system.hasHandle())
        {
            _system.release();
        }
    }
}