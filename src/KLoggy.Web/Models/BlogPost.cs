using System;

namespace KLoggy.Web.Models
{
    public class BlogPostModel
    {
    }

    public class BlogPostReferenceModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string BriefDescription { get; set; }
        public string Content { get; set; }
        public string RelativeUrl { get; set; }
        public int CommentCount { get; set; }
        public DateTimeOffset PublishedOn { get; set; }
        public DateTimeOffset LastUpdatedOn { get; set; }

        public AuthorReferenceModel Author { get; set; }
    }
}