using System;

namespace KLoggy.Web.Models
{
    public class AuthorModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset LastUpdatedOn { get; set; }
    }

    public class AuthorReferenceModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
    }
}