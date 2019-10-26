using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Channel_Nine.models.SingleEpisode
{
    // Detailed class for storing data related to an episode.
    class SingleEpisode
    {
        public string episodeID { get; set; }

        public string episodeTitle { get; set; }

        public string episodeDescription { get; set; }

        public List<string> episodeTags { get; set; }

        public ObservableCollection<EpisodeComments> episodeComments { get; set; }

        public videoData epsiodeStreams { get; set; }

        public string episodeRunTime { get; set; }


    }
}
