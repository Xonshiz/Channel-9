using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Channel_Nine.models.Shows
{
    class Shows
    {
        public int currentPage { get; set; }

        public ObservableCollection<Show> result { get; set; }

        public DateTime lastFetchedTime { get; set; }

        public int totalPages { get; set; }
    }
}
