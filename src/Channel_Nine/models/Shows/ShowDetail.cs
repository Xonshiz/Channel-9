using System;
using System.Collections.Generic;
using System.Text;

namespace Channel_Nine.models.Shows
{
    class ShowDetail
    {
        public string showId { get; set; }
        public string showDescription { get; set; }
        public string showTitle { get; set; }
        public List<Authors> showAuthors { get; set; }
        public Shows showEpisodes { get; set; }
    }
}
