namespace D3D.Content.Pipeline.Audio;

internal class FmodBgmContent
{
    public string FileName { get; }

    public uint LoopStart { get; }
    
    public uint LoopEnd { get; }

    public FmodBgmContent(string fileName, uint loopStart, uint loopEnd)
    {
        FileName = fileName;
        LoopStart = loopStart;
        LoopEnd = loopEnd;
    }
}