using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Application.Contracts.Infrastructure
{
    public interface IProductApiClient
    {
        Task SetUserProductsVisibilityAsync(Guid userId, bool isVisible);
    }
}
