namespace D3D.Resources.Interface;

public interface IContentManager
{
    T LoadAsset<T>(string assetName);
}