using System.Collections.Generic;
using System.Diagnostics;
using D3D.Resources.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace D3D.Resources;

public sealed class GameContentManager : IContentManager
{
    private readonly List<ContentManager> _contentManagers = new();
    
    public GameContentManager(Game game)
    {
        game.Services.AddService(typeof(IContentManager), this);
        _contentManagers.Add(game.Content);
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
}