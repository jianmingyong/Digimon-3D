using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace D3D.Content.Pipeline.Shaders;

internal class BasicEffectWithAlphaTestContent : CompiledEffectContent
{
    public BasicEffectWithAlphaTestContent(byte[] effectCode) : base(effectCode)
    {
    }
}