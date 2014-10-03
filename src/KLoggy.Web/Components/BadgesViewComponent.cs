using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;
using System;

namespace KLoggy.Web.Components
{
    [ViewComponent(Name = "Badges")]
    public class BadgesViewComponent : ViewComponent
    {
        private readonly IBadgesManager _badgeManager;
        
        public BadgesViewComponent(IBadgesManager badgeManager)
        {
            if(badgeManager == null)
            {
                throw new ArgumentNullException("badgeManager");
            }
            
            _badgeManager = badgeManager;
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var badges = await _badgeManager.GetAllAsync();            
            return View("BadgeList", badges);
        }
    }
}