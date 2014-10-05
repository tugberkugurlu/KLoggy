using System.Collections.Generic;
using System.Threading.Tasks;

namespace KLoggy.Domain
{
    public interface IProfileLinkManager
    {
        Task<IEnumerable<ProfileLink>> GetAllAsync();
    }
}