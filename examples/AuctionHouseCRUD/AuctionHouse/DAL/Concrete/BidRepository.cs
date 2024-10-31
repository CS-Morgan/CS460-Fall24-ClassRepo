using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using AuctionHouse.DAL.Abstract;
using AuctionHouse.Models;

namespace AuctionHouse.DAL.Concrete
{
    public class BidRepository : Repository<Bid>, IBidRepository
    {
        public BidRepository(AuctionHouseDbContext ctx) : base(ctx)
        {

        }

        public int NumberOfBids()
        {
            return GetAll().Count();
        }
    }
}