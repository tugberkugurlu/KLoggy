using System.Collections.Generic;
using System.Threading.Tasks;

namespace KLoggy.Domain
{
    public class InMemoryProfileLinkManager : IProfileLinkManager
    {
        private static readonly IEnumerable<ProfileLink> _profileLinks = new List<ProfileLink> 
        {
            new ProfileLink { Name = "Twitter", Url = "http://twitter.com/tourismgeek" },
            new ProfileLink { Name = "linkedIn", Url = "http://www.linkedin.com/in/tugberk" },
            new ProfileLink { Name = "GitHub", Url = "http://github.com/tugberkugurlu" },
            new ProfileLink { Name = "Stackoverflow", Url = "http://stackoverflow.com/users/463785/tugberk" }
        };
        
        public Task<IEnumerable<ProfileLink>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<ProfileLink>>(_profileLinks);
        }
    }
}