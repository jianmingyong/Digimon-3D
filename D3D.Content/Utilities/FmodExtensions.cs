using System.Runtime.CompilerServices;
using FMOD;

namespace D3D.Content.Utilities;

public static class FmodExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowOnError(this RESULT result)
    {
        if (result != RESULT.OK)
        {
            throw new Exception(Error.String(result));
        }
    }
}