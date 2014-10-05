using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;
using KLoggy.Web.Infrastructure;
using System;
using KLoggy.Domain;
using System.Collections.Generic;
using System.Linq;

namespace KLoggy.Web.Components
{
    public class BadgesViewComponent : ViewComponent
    {
        private static readonly IDictionary<string, string> _profileNameToFaNameMappings = new Dictionary<string, string>
        {
            { "Twitter", "twitter" },
            { "linkedIn", "linkedin" },
            { "GitHub", "github" },
            { "Stackoverflow", "stack-exchange" }
        };

        private readonly IProfileLinkManager _badgeManager;
        
        public BadgesViewComponent(IProfileLinkManager badgeManager)
        {
            if (badgeManager == null)
            {
                throw new ArgumentNullException("badgeManager");
            }
            
            _badgeManager = badgeManager;
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<ProfileLink> profileLinks = await _badgeManager.GetAllAsync();
            IEnumerable<Badge> badges = profileLinks.Select(link => new Badge
            {
                Name = link.Name,
                FaName = _profileNameToFaNameMappings[link.Name],
                Url = link.Url
            });

            return View("BadgeList", badges);
        }
    }
}