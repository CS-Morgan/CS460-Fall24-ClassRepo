using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using AuctionHouse2.DAL.Abstract;
using AuctionHouse2.Models;

namespace AuctionHouse2.DAL.Concrete
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