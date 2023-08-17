using System.ComponentModel;
using D3D.Content.Audio;
using D3D.Content.Utilities;
using D3D.Managers.Implementations.Settings;
using FMOD;
using Microsoft.Xna.Framework;

namespace D3D.Managers.Implementations;

public sealed class FmodAudioManager : IFmodBackgroundMusicSystem, IFmodSoundEffectSystem, IDisposable
{
    FMOD.System IFmodSystem.System => _system;

    ChannelGroup IFmodBackgroundMusicSystem.ChannelGroup => _backgroundMusicChannelGroup;

    ChannelGroup IFmodSoundEffectSystem.ChannelGroup => _soundEffectChannelGroup;

    private readonly GameSettingManager _gameSettingManager;
    private readonly IAssetManager _assetManager;

    private FMOD.System _system;
    private ChannelGroup _backgroundMusicChannelGroup;
    private ChannelGroup _soundEffectChannelGroup;

    public FmodAudioManager(Game game, GameSettingManager gameSettingManager, IAssetManager assetManager)
    {
        _gameSettingManager = gameSettingManager;
        _assetManager = assetManager;

        Factory.System_Create(out _system).ThrowOnError();

        _system.init(1024, INITFLAGS.NORMAL, new IntPtr((int) OUTPUTTYPE.AUTODETECT)).ThrowOnError();
        _system.createChannelGroup("BGM", out _backgroundMusicChannelGroup).ThrowOnError();
        _system.createChannelGroup("SE", out _soundEffectChannelGroup).ThrowOnError();

        _backgroundMusicChannelGroup.setVolume(gameSettingManager.BackgroundMusicVolume).ThrowOnError();
        _backgroundMusicChannelGroup.setMute(gameSettingManager.BackgroundMusicIsMuted).ThrowOnError();
        
        _soundEffectChannelGroup.setVolume(gameSettingManager.SoundEffectMusicVolume).ThrowOnError();
        _soundEffectChannelGroup.setMute(gameSettingManager.SoundEffectMusicIsMuted).ThrowOnError();

        _gameSettingManager.PropertyChanged += GameSettingManagerOnPropertyChanged;

        game.Services.AddService(typeof(FmodAudioManager), this);
        game.Services.AddService(typeof(IFmodBackgroundMusicSystem), this);
        game.Services.AddService(typeof(IFmodSoundEffectSystem), this);
    }

    ~FmodAudioManager()
    {
        ReleaseUnmanagedResources();
    }

    public FmodBackgroundMusic LoadBackgroundMusic(string filePath)
    {
        return _assetManager.TryLoadAsset<FmodBackgroundMusic>(filePath, out var result) ? result : new FmodBackgroundMusic(this, filePath);
    }

    public FmodSoundEffect LoadSoundEffect(string filePath)
    {
        return _assetManager.TryLoadAsset<FmodSoundEffect>(filePath, out var result) ? result : new FmodSoundEffect(this, filePath);
    }

    private void GameSettingManagerOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(GameSettingManager.BackgroundMusicVolume):
                _backgroundMusicChannelGroup.setVolume(_gameSettingManager.BackgroundMusicVolume).ThrowOnError();
                break;

            case nameof(GameSettingManager.BackgroundMusicIsMuted):
                _backgroundMusicChannelGroup.setMute(_gameSettingManager.BackgroundMusicIsMuted).ThrowOnError();
                break;
            
            case nameof(GameSettingManager.SoundEffectMusicVolume):
                _soundEffectChannelGroup.setVolume(_gameSettingManager.SoundEffectMusicVolume).ThrowOnError();
                break;
            
            case nameof(GameSettingManager.SoundEffectMusicIsMuted):
                _soundEffectChannelGroup.setMute(_gameSettingManager.SoundEffectMusicIsMuted).ThrowOnError();
                break;
        }
    }

    private void ReleaseUnmanagedResources()
    {
        if (_backgroundMusicChannelGroup.hasHandle() && _backgroundMusicChannelGroup.release() == RESULT.OK)
        {
            _backgroundMusicChannelGroup.clearHandle();
        }

        if (_soundEffectChannelGroup.hasHandle() && _soundEffectChannelGroup.release() == RESULT.OK)
        {
            _soundEffectChannelGroup.clearHandle();
        }

        if (_system.hasHandle() && _system.release() == RESULT.OK)
        {
            _system.clearHandle();
        }
    }

    public void Dispose()
    {
        _gameSettingManager.PropertyChanged -= GameSettingManagerOnPropertyChanged;
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }
}