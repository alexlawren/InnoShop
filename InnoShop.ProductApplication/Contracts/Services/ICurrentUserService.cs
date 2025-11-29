using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.ProductApplication.Contracts.Services
{
    public interface ICurrentUserService
    {
        Guid? UserGuid { get; }
    }
}
