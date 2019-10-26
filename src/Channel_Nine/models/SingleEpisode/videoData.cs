using System;
using System.Collections.Generic;
using System.Text;

namespace Channel_Nine.models.SingleEpisode
{
    class videoData
    {
        public string videoID { get; set; }

        public Dictionary<Common.InternalTypes.videoQuality, string> videoStreams { get; set; }

        public Dictionary<Common.InternalTypes.audioQuality, string> audioStreams { get; set; }

        public Dictionary<string, string> subtitleStreams { get; set; }
    }
}
