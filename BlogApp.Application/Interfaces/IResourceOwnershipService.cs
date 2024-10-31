using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Interfaces
{
    public interface IResourceOwnershipService
    {
        Task<bool> IsOwnerAsync(int resourceId, int userId);
    }
}
