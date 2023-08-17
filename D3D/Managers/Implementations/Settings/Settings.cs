using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace D3D.Managers.Implementations.Settings;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(Settings), GenerationMode = JsonSourceGenerationMode.Default)]
public sealed partial class SettingsJsonSerializerContext : JsonSerializerContext
{
}

public sealed class Settings : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public float BackgroundMusicVolume
    {
        get => _backgroundMusicVolume;
        set
        {
            if (value.Equals(_backgroundMusicVolume)) return;
            _backgroundMusicVolume = value;
            OnPropertyChanged();
            OnPropertyChanged();
        }
    }

    public bool BackgroundMusicIsMuted
    {
        get => _backgroundMusicIsMuted;
        set
        {
            if (value == _backgroundMusicIsMuted) return;
            _backgroundMusicIsMuted = value;
            OnPropertyChanged();
        }
    }

    public float SoundEffectMusicVolume
    {
        get => _soundEffectMusicVolume;
        set
        {
            if (value.Equals(_soundEffectMusicVolume)) return;
            _soundEffectMusicVolume = value;
            OnPropertyChanged();
        }
    }

    public bool SoundEffectMusicIsMuted
    {
        get => _soundEffectMusicIsMuted;
        set
        {
            if (value == _soundEffectMusicIsMuted) return;
            _soundEffectMusicIsMuted = value;
            OnPropertyChanged();
        }
    }

    public string[] ContentPacks
    {
        get => _contentPacks;
        set
        {
            if (value.AsSpan().SequenceEqual(_contentPacks)) return;
            _contentPacks = value;
            OnPropertyChanged();
        }
    }

    private float _backgroundMusicVolume = 1f;
    private bool _backgroundMusicIsMuted;
    private float _soundEffectMusicVolume = 1f;
    private bool _soundEffectMusicIsMuted;
    private string[] _contentPacks = Array.Empty<string>();

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}