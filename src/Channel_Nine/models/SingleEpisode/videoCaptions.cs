using System;
using System.Collections.Generic;
using System.Text;

namespace Channel_Nine.models.SingleEpisode
{
    class videoCaptions
    {
        public int videoID { get; set; }
        // Language, URL
        public Dictionary<string, string> captionStreams { get; set; }
    }
}
