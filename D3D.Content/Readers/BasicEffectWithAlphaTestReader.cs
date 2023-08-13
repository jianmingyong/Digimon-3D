using D3D.Content.Shaders;
using Microsoft.Xna.Framework.Content;

namespace D3D.Content.Readers;

public class BasicEffectWithAlphaTestReader : ContentTypeReader<BasicEffectWithAlphaTest>
{
    protected override BasicEffectWithAlphaTest Read(ContentReader input, BasicEffectWithAlphaTest existingInstance)
    {
        var dataLength = input.ReadInt32();
        return new BasicEffectWithAlphaTest(input.GetGraphicsDevice(), input.ReadBytes(dataLength));
    }
}