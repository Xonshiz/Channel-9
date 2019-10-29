using System;
using System.Collections.Generic;
using System.Text;

namespace Channel_Nine.models.SearchResults
{
    public class Rating
    {
        public double average { get; set; }
        public int count { get; set; }
    }

    public class Views
    {
        public int total { get; set; }
        public int thisMonth { get; set; }
        public int thisWeek { get; set; }
    }

    public class Datum
    {
        public double searchScore { get; set; }
        public string id { get; set; }
        public string url { get; set; }
        public DateTime published { get; set; }
        public DateTime modified { get; set; }
        public string previewImage { get; set; }
        public string mediumPreviewImage { get; set; }
        public string largePreviewImage { get; set; }
        public string title { get; set; }
        public string groupName { get; set; }
        public string groupType { get; set; }
        public string type { get; set; }
        public string groupUrl { get; set; }
        public string language { get; set; }
        public string summary { get; set; }
        public int mediaDuration { get; set; }
        public List<string> tags { get; set; }
        public List<string> authors { get; set; }
        public List<string> captionLanguages { get; set; }
        public object primaryAudience { get; set; }
        public Rating rating { get; set; }
        public Views views { get; set; }
        public int commentCount { get; set; }
        public bool hasCaptions { get; set; }
    }

    public class Paging
    {
        public string firstPage { get; set; }
        public string nextPage { get; set; }
        public object previousPage { get; set; }
        public string lastPage { get; set; }
    }

    public class SearchResult
    {
        public string id { get; set; }
        public int total { get; set; }
        public List<Datum> data { get; set; }
        public Paging paging { get; set; }
    }
}
