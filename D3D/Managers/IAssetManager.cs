using System.Diagnostics.CodeAnalysis;

namespace D3D.Managers;

public interface IAssetManager
{
    T LoadAsset<T>(string assetName);

    bool TryLoadAsset<T>(string assetName, [MaybeNullWhen(false)] out T asset);

    void UnloadAsset(string assetName);

    void UnloadAssets(IList<string> assetNames);

    void UnloadAllAssets();
}