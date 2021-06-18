using System.Collections.Generic;

namespace Base64ConverterCore.Models
{
    public class AnimationSet
    {
        public List<string> spriteSheet { get; set; }
        public FrameProperties properties { get; set; }
        public string animationType { get; set; }
        public int animationTypeID { get; set; }
    }
}
