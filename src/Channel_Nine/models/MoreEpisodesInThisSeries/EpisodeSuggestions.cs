using Channel_Nine.models.AllContent;
using System;
using System.Collections.Generic;
using System.Text;

namespace Channel_Nine.models.MoreEpisodesInThisSeries
{
    class EpisodeSuggestions
    {
        public int videoID { get; set; } // Video ID of the episode this suggestion belongs to.

        public List<SingleResult> moreEpisodesInThisList { get; set; } // List of SingleResult, because that structure is same.

        public List<SingleResult> relatedEpisodesList { get; set; } // List of SingleResult, because that structure is same.
    }
}
