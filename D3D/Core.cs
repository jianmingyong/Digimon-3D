using D3D.Audio;
using D3D.Content.Audio;
using D3D.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace D3D;

public class Core : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private readonly FmodBgmAudioManager _fmodBgmAudioManager;

    public Core()
    {
        _graphics = new GraphicsDeviceManager(this);
        _ = new GameContentManager(this);
        _fmodBgmAudioManager = new FmodBgmAudioManager(this);
    }

    protected override void Initialize()
    {
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
    }

    protected override void LoadContent()
    {
        var test = Content.Load<FmodBgmSound>("Audio/BGM/TestTitle");
        test.Play();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        base.Draw(gameTime);
    }
}