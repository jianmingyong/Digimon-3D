using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Content;

namespace D3D.Content.Utilities;

internal static class ContentReaderExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IServiceProvider GetServiceProvider(this ContentReader reader)
    {
        return reader.ContentManager.ServiceProvider;
    }
}