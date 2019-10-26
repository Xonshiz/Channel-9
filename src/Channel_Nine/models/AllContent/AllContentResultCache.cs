using System;
using System.Collections.Generic;
using System.Text;

namespace Channel_Nine.models.AllContent
{
    // This will be used as a cache mechanism to store all the results.
    // We can save multiple calls to Channel 9 by using cache.
    // This will have all the pages results. Whatever user has browsed, will be saved here.
    class AllContentResultCache
    {
        public int totalPages { get; set; }

        public List<AllContentResult> data { get; set; }
    }
}
