using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AuctionHouse2.Models;

namespace AuctionHouse2.DAL.Abstract
{
    public interface IBidRepository : IRepository<Bid>
    {
        int NumberOfBids();
    }
}