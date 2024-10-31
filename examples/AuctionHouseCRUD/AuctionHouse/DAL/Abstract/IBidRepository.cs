using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AuctionHouse.Models;

namespace AuctionHouse.DAL.Abstract
{
    public interface IBidRepository : IRepository<Bid>
    {
        int NumberOfBids();
    }
}