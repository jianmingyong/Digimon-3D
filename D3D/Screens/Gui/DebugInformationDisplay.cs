using System;
using System.Diagnostics;
using System.Text;
using D3D.Resources.Interface;
using D3D.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace D3D.Screens.Gui;

public sealed class DebugInformationDisplay : IDisposable
{
    private sealed class FrameMonitor
    {
        public int Value { get; private set; }
    
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        private int _frameDrawn;

        public void Update()
        {
            if (_stopwatch.ElapsedMilliseconds < 500) return;
            _stopwatch.Stop();
            
            Value = (int) Math.Round(_frameDrawn / _stopwatch.Elapsed.TotalSeconds);
            _frameDrawn = 0;
            
            _stopwatch.Restart();
        }

        public void Draw()
        {
            _frameDrawn++;
        }
    }
    
    private readonly IGraphicsDeviceService _graphicsDeviceService;
    private readonly IAssetManager _assetManager;
    
    private readonly FrameMonitor _frameMonitor = new();
    private readonly StringBuilder _stringBuilder = new();
    
    private SpriteBatch? _spriteBatch;
    private SpriteFont? _spriteFont;

    private bool _canDraw;
    
    public DebugInformationDisplay(IGraphicsDeviceService graphicsDeviceService, IAssetManager assetManager)
    {
        _graphicsDeviceService = graphicsDeviceService;
        _assetManager = assetManager;
    }

    public void Initialize()
    {
        _spriteBatch = new SpriteBatch(_graphicsDeviceService.GraphicsDevice);
        _canDraw = true;
    }
    
    public void LoadContent()
    {
        _spriteFont = _assetManager.LoadAsset<SpriteFont>("Fonts/Pixel Digivolve");
    }

    public void Update()
    {
        _frameMonitor.Update();
    }
    
    public void Draw()
    {
        Debug.Assert(_spriteBatch != null, nameof(_spriteBatch) + " != null");
        
        _frameMonitor.Draw();
        
        if (!_canDraw) return;
        
        _stringBuilder.AppendLine($"{ApplicationUtility.Name} {ApplicationUtility.Version} ({ApplicationUtility.FrameworkVersion}) / FPS: {_frameMonitor.Value:D}");
        
        _spriteBatch.Begin();
        _spriteBatch.DrawString(_spriteFont, _stringBuilder, new Vector2(10, 10), Color.Black);
        _spriteBatch.DrawString(_spriteFont, _stringBuilder, new Vector2(8, 8), Color.White);
        _spriteBatch.End();

        _stringBuilder.Clear();
    }

    public void Dispose()
    {
        _spriteBatch?.Dispose();
    }
}