using System;
using System.Collections.Generic;
using System.Text;

namespace Channel_Nine.models.AllContent
{
    // Represents info for a particular video appeared in the All Content.

    class SingleResult
    {
        public string videoID { get; set; }
        public string videoTitle { get; set; }
        public string videoImage { get; set; }
        public string videoUrl { get; set; }
        public string videoShow { get; set; }
        public string videoDuration { get; set; }
        public string videoLanguage { get; set; }
    }
}
