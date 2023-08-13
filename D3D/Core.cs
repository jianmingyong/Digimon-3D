using D3D.Content.Audio;
using D3D.Managers.Implementations;
using D3D.Screens.Gui;
using D3D.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace D3D;

public sealed class Core : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private readonly FmodAudioManager _fmodAudioManager;
    private readonly AssetManager _assetManager;
    private readonly DebugInformationDisplay _debugInformationDisplay;

    public Core()
    {
        _graphics = new GraphicsDeviceManager(this);
        _fmodAudioManager = new FmodAudioManager(Services);
        _assetManager = new AssetManager(Content);
        _debugInformationDisplay = new DebugInformationDisplay(_graphics, _assetManager);
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
        var bgm = _assetManager.LoadAsset<FmodBackgroundMusic>("Audio/BGM/TestTitle");
        bgm.Play();
    }

    protected override void Update(GameTime gameTime)
    {
        _debugInformationDisplay.Update();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        _debugInformationDisplay.Draw();
    }

    protected override void Dispose(bool disposing)
    {
        _assetManager.Dispose();
        _fmodAudioManager.Dispose();
        _debugInformationDisplay.Dispose();

        base.Dispose(disposing);
    }
}