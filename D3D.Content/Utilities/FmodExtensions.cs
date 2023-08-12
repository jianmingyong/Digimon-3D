using System;
using FMOD;

namespace D3D.Content.Utilities;

public static class FmodExtensions
{
    public static void ThrowOnError(this RESULT result)
    {
        if (result != RESULT.OK)
        {
            throw new Exception(Error.String(result));
        }
    }
}