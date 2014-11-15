using Xunit;
using System;
using KLoggy.Web.Components;

namespace KLoggy.Web.Test
{
    public class BadgesViewComponentTests 
    {
        [Fact]
        public void BadgesViewComponent_Ctor_Should_Throw_When_badgeManager_Param_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new BadgesViewComponent(null));
        }
    }
}