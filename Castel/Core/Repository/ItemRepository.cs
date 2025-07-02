using AutoMapper;
using Castel.Core.Interface;
using Castel.DataBaseContext;
using Castel.Models;

namespace Castel.Core.Repository
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(StoreDbContext context, IMapper map) : base(context, map)
        {
        }
    }
}
