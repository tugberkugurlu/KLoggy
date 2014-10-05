using System.Collections.Generic;
using System.Threading.Tasks;

namespace KLoggy.Web.Infrastructure
{
    public interface IBadgesManager
    {
        Task<IEnumerable<Badge>> GetAllAsync();
    }
}