using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace KLoggy.Web.Infrastructure 
{
    public interface IBadgesManager 
    {
        Task<IEnumerable<Badge>> GetAllAsync();
    }
    
    public class BadgesManager : IBadgesManager
    {
        private static readonly IEnumerable<Badge> _badges = new List<Badge> 
        {
            new Badge { Name = "Twitter", Url = "http://twitter.com/tourismgeek", FaName = "twitter" },
            new Badge { Name = "linkedIn", Url = "http://www.linkedin.com/in/tugberk", FaName = "linkedin" },
            new Badge { Name = "GitHub", Url = "http://github.com/tugberkugurlu", FaName = "github" },
            new Badge { Name = "Stackoverflow", Url = "http://stackoverflow.com/users/463785/tugberk", FaName = "stack-exchange" }
        };
        
        public Task<IEnumerable<Badge>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Badge>>(_badges);
        }
    }
}