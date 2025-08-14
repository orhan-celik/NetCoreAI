using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreAI.Project03_RapidAPIConsume
{
    public class Movie
    {


        public string seasonId { get; set; }

        public List<Episode> Episodes { get; set; }

        public class Episode
        {
            public string episodeId { get; set; }
            public Availability availability { get; set; }
            public int bookmarkPosition { get; set; }
            public Contextualsynopsis contextualSynopsis { get; set; }
            public int displayRuntime { get; set; }
            public int runtime { get; set; }
            public string title { get; set; }
            public Summary summary { get; set; }
            public Interestingmoment interestingMoment { get; set; }
        }

        public class Availability
        {
            public bool isPlayable { get; set; }
            public string availabilityDate { get; set; }
            public long availabilityStartTime { get; set; }
            public object unplayableCause { get; set; }
        }

        public class Contextualsynopsis
        {
            public string text { get; set; }
            public string evidenceKey { get; set; }
        }

        public class Summary
        {
            public string type { get; set; }
            public int id { get; set; }
            public bool isOriginal { get; set; }
            public int idx { get; set; }
            public int episode { get; set; }
            public int season { get; set; }
            public bool isPlayable { get; set; }
        }

        public class Interestingmoment
        {
            public _342X192 _342x192 { get; set; }
        }

        public class _342X192
        {
            public Webp webp { get; set; }
        }

        public class Webp
        {
            public string type { get; set; }
            public Value value { get; set; }
        }

        public class Value
        {
            public int width { get; set; }
            public int height { get; set; }
            public string url { get; set; }
            public string image_key { get; set; }
        }

    }
}
