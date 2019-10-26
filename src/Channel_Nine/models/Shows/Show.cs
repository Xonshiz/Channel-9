using System;
using System.Collections.Generic;
using System.Text;

namespace Channel_Nine.models.Shows
{
    class Show
    {
        public string showID { get; set; }
        public string showTitle { get; set; }
        //Show URL will be just the part needed to go to /shows/{showURL} and get the details.
        public string showUrl { get; set; }
        public string showThumbnail { get; set; }
        public string showLanguage { get; set; }
        public int showNumber { get; set; }
    }
}
