using System.ComponentModel;
using System.Text.Json;
using D3D.Utilities;
using Microsoft.Xna.Framework;

namespace D3D.Managers.Implementations.Settings;

public sealed class GameSettingManager : IDisposable
{
    public event PropertyChangedEventHandler? PropertyChanged
    {
        add => _settings.PropertyChanged += value;
        remove => _settings.PropertyChanged -= value;
    }

    public float BackgroundMusicVolume
    {
        get => _settings.BackgroundMusicVolume;
        set => _settings.BackgroundMusicVolume = value;
    }

    public bool BackgroundMusicIsMuted
    {
        get => _settings.BackgroundMusicIsMuted;
        set => _settings.BackgroundMusicIsMuted = value;
    }

    public float SoundEffectMusicVolume
    {
        get => _settings.SoundEffectMusicVolume;
        set => _settings.SoundEffectMusicVolume = value;
    }

    public bool SoundEffectMusicIsMuted
    {
        get => _settings.SoundEffectMusicIsMuted;
        set => _settings.SoundEffectMusicIsMuted = value;
    }

    public string[] ContentPacks
    {
        get => _settings.ContentPacks;
        set => _settings.ContentPacks = value;
    }

    private const string SETTING_FILE_NAME = "appsettings.json";

    private readonly Settings _settings = new();
    private readonly string _settingFilePath;

    public GameSettingManager(Game game)
    {
        var applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create);
        var settingDirPath = Path.Combine(applicationDataPath, ApplicationUtility.Name);

        if (!Directory.Exists(settingDirPath))
        {
            Directory.CreateDirectory(settingDirPath);
        }

        _settingFilePath = Path.Combine(settingDirPath, SETTING_FILE_NAME);

        if (File.Exists(_settingFilePath))
        {
            using var file = File.OpenRead(_settingFilePath);
            _settings = JsonSerializer.Deserialize(file, SettingsJsonSerializerContext.Default.Settings) ?? new Settings();
        }
        else
        {
            SaveSettings();
        }

        game.Services.AddService(typeof(GameSettingManager), this);
    }

    public void SaveSettings()
    {
        File.WriteAllText(_settingFilePath, JsonSerializer.Serialize(_settings, SettingsJsonSerializerContext.Default.Settings));
    }

    public void Dispose()
    {
        SaveSettings();
    }
}