using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Channel_Nine.models.AllContent
{
    // Represents all the episode on a particular page.
    class AllContentResult
    {
        public int currentPage { get; set; }

        public ObservableCollection<SingleResult> result { get; set; }

        public DateTime lastFetchedTime { get; set; }

        public int totalPages { get; set; }
    }
}
