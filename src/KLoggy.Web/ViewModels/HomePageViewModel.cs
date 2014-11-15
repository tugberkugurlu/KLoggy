using KLoggy.Web.Models;
using System.Collections.Generic;

namespace KLoggy.Web.ViewModels
{
    public class HomePageViewModel
    {
        public IEnumerable<BlogPostReferenceModel> BlogPosts { get; set; }
    }
}