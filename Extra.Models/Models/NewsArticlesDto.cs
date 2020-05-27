using System;
using System.Collections.Generic;
using System.Text;

namespace Extra.Models.Models
{
    public class NewsArticlesDto
    {
        public NewsSourceDto Source { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string url { get; set; }
        public string UrlToImage { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Content { get; set; }
    }
}
