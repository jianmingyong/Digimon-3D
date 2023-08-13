using System.Collections.Generic;

namespace D3D.Resources.Interface;

public interface IAssetManager
{
    T LoadAsset<T>(string assetName);

    void UnloadAsset(string assetName);

    void UnloadAssets(IList<string> assetNames);

    void UnloadAllAssets();
}