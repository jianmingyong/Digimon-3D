using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;

namespace D3D.Managers.Implementations;

public sealed class AssetManager : IAssetManager, IDisposable
{
    private readonly List<ContentManager> _contentManagers = new();

    private readonly ContentManager _baseContentManager;
    
    private ContentManager? _gameModeContentManager;
    
    public AssetManager(ContentManager baseContentManager)
    {
        _baseContentManager = baseContentManager;
        _contentManagers.Add(baseContentManager);
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

    public void Dispose()
    {
        foreach (var contentManager in _contentManagers)
        {
            contentManager.Dispose();
        }
    }
}