using System.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Audio;

namespace D3D.Content.Pipeline.Audio;

[ContentProcessor(DisplayName = "Sound Effect - FMOD")]
public class FmodSoundEffectProcessor : ContentProcessor<AudioContent, FmodSoundEffectContent>
{
    /// <summary>
    /// Gets or sets the target format quality of the audio content.
    /// </summary>
    /// <value>The ConversionQuality of this audio data.</value>
    public ConversionQuality Quality { get; set; } = ConversionQuality.Best;
    
    public override FmodSoundEffectContent Process(AudioContent input, ContentProcessorContext context)
    {
        var profile = AudioProfile.ForPlatform(context.TargetPlatform);
        var finalQuality = profile.ConvertAudio(context.TargetPlatform, Quality, input);

        if (Quality != finalQuality)
        {
            context.Logger.LogMessage("Failed to convert using \"{0}\" quality, used \"{1}\" quality", Quality, finalQuality);
        }

        return new FmodSoundEffectContent(input.Data.ToArray());
    }
}