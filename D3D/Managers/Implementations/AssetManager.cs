using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace D3D.Managers.Implementations;

public sealed class AssetManager : IAssetManager, IDisposable
{
    private readonly List<ContentManager> _contentManagers = new();

    private readonly ContentManager _baseContentManager;
    
    private ContentManager? _gameModeContentManager;
    
    public AssetManager(Game game)
    {
        _baseContentManager = game.Content;
        UpdateAssetManagers();
        
        game.Services.AddService(typeof(AssetManager), this);
        game.Services.AddService(typeof(IAssetManager), this);
    }

    public T LoadAsset<T>(string assetName)
    {
        ContentLoadException? exception = null;
        
        foreach (var contentManager in _contentManagers)
        {
            try
            {
                return contentManager.Load<T>(assetName);
            }
            catch (ContentLoadException ex)
            {
                exception = ex;
            }
        }
        
        Debug.Assert(exception != null, nameof(exception) + " != null");
        throw exception;
    }

    public bool TryLoadAsset<T>(string assetName, [MaybeNullWhen(false)] out T asset)
    {
        foreach (var contentManager in _contentManagers)
        {
            try
            {
                asset = contentManager.Load<T>(assetName);
                return true;
            }
            catch (ContentLoadException)
            {
            }
        }

        asset = default;
        return false;
    }
    
    public void UnloadAsset(string assetName)
    {
        foreach (var contentManager in _contentManagers)
        {
            contentManager.UnloadAsset(assetName);
        }
    }
    
    public void UnloadAssets(IList<string> assetNames)
    {
        foreach (var contentManager in _contentManagers)
        {
            contentManager.UnloadAssets(assetNames);
        }
    }

    public void UnloadAllAssets()
    {
        foreach (var contentManager in _contentManagers)
        {
            contentManager.Unload();
        }
    }

    private void UpdateAssetManagers()
    {
        _contentManagers.Clear();

        if (_gameModeContentManager != null)
        {
            _contentManagers.Add(_gameModeContentManager);
        }
        
        _contentManagers.Add(_baseContentManager);
    }

    public void Dispose()
    {
        foreach (var contentManager in _contentManagers)
        {
            contentManager.Dispose();
        }
    }
}