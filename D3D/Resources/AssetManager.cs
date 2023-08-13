using System;
using System.Collections.Generic;
using System.Diagnostics;
using D3D.Resources.Interface;
using Microsoft.Xna.Framework.Content;

namespace D3D.Resources;

public sealed class AssetManager : IAssetManager, IDisposable
{
    private readonly List<ContentManager> _contentManagers = new();
    
    public AssetManager(ContentManager baseContentManager)
    {
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