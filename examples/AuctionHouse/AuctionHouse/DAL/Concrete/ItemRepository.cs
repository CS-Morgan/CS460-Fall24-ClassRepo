using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using AuctionHouse.DAL.Abstract;
using AuctionHouse.Models;

namespace AuctionHouse.DAL.Concrete
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(AuctionHouseDbContext ctx) : base(ctx)
        {

        }

        public List<Item> GetItems()
        {
            return _dbSet.ToList();
        }

        public List<string> ItemNames()
        {
            return GetAll().Select(i => i.Name).ToList();
        }
    }
}