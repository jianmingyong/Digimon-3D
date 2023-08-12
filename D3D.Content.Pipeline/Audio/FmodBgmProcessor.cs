﻿using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Audio;
using MonoGame.Framework.Content.Pipeline.Builder;

namespace D3D.Content.Pipeline.Audio;

[ContentProcessor(DisplayName = "BGM - FMOD")]
internal class FmodBgmProcessor : ContentProcessor<AudioContent, FmodBgmContent>
{
    /// <summary>
    /// Gets or sets the target format quality of the audio content.
    /// </summary>
    /// <value>The ConversionQuality of this audio data.</value>
    public ConversionQuality Quality { get; set; } = ConversionQuality.Best;

    public uint LoopStart { get; set; }
    
    public uint LoopLength { get; set; }
    
    public override FmodBgmContent Process(AudioContent input, ContentProcessorContext context)
    {
        var songFileName = context.OutputFilename;
        
        var profile = AudioProfile.ForPlatform(context.TargetPlatform);
        var finalQuality = profile.ConvertStreamingAudio(context.TargetPlatform, Quality, input, ref songFileName);

        var loopEnd = LoopStart + LoopLength;

        if (loopEnd * 1000.0 / input.Format.SampleRate > input.Duration.TotalMilliseconds)
        {
            throw new PipelineException($"{nameof(LoopLength)} is greater than the length of the sound.");
        }
        
        context.AddOutputFile(songFileName);

        if (Quality != finalQuality)
        {
            context.Logger.LogMessage("Failed to convert using \"{0}\" quality, used \"{1}\" quality", Quality, finalQuality);
        }
        
        return new FmodBgmContent(PathHelper.GetRelativePath(Path.GetDirectoryName(context.OutputFilename) + Path.DirectorySeparatorChar, songFileName), LoopStart, loopEnd);
    }
}