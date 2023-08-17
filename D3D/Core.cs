using D3D.Managers.Implementations;
using D3D.Managers.Implementations.Settings;
using D3D.Screens.Gui;
using D3D.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace D3D;

public sealed class Core : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private readonly GameSettingManager _gameSettingManager;
    private readonly AssetManager _assetManager;
    private readonly FmodAudioManager _fmodAudioManager;
    private readonly InputManager _inputManager;
    private readonly DebugInformationDisplay _debugInformationDisplay;

    public Core()
    {
        _graphics = new GraphicsDeviceManager(this);
        _gameSettingManager = new GameSettingManager(this);
        _assetManager = new AssetManager(this);
        _fmodAudioManager = new FmodAudioManager(this, _gameSettingManager, _assetManager);
        _inputManager = new InputManager(this);
        _debugInformationDisplay = new DebugInformationDisplay(_graphics, _assetManager, _inputManager);
    }

    protected override void Initialize()
    {
        Window.Title = $"{ApplicationUtility.Name} {ApplicationUtility.Version} ({ApplicationUtility.FrameworkVersion})";

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        if (_graphics.GraphicsDevice.Adapter.IsProfileSupported(GraphicsProfile.HiDef))
        {
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();

        base.Initialize();
        
        _debugInformationDisplay.Initialize();
    }

    protected override void LoadContent()
    {
        _debugInformationDisplay.LoadContent();
        _fmodAudioManager.LoadBackgroundMusic("Audio/BGM/Title").Play();
    }

    protected override void Update(GameTime gameTime)
    {
        _inputManager.Update();
        _debugInformationDisplay.Update();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _debugInformationDisplay.Draw();
    }

    protected override void Dispose(bool disposing)
    {
        _debugInformationDisplay.Dispose();
        _fmodAudioManager.Dispose();
        _assetManager.Dispose();
        _gameSettingManager.Dispose();
        base.Dispose(disposing);
    }
}