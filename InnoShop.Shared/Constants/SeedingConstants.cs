using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Shared.Constants
{
    public class SeedingConstants
    {
        public static readonly Guid AdminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        public static readonly Guid User1Id = Guid.Parse("22222222-2222-2222-2222-222222222222");
        public static readonly Guid User2Id = Guid.Parse("33333333-3333-3333-3333-333333333333");

        public static readonly List<Guid> AllUserIds = new() { AdminId, User1Id, User2Id };
    }
}
