using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AuctionHouse.DAL.Abstract;
using AuctionHouse.Models;

// Put this in folder DAL/Concrete
namespace AuctionHouse.DAL.Concrete;

public class BuyerRepository : Repository<Buyer>, IBuyerRepository
{ 
    public BuyerRepository(AuctionHouseDbContext ctx) : base(ctx)
    {

    }

    public int NumberOfBuyers()
    {
        return GetAll().Count();
    }

    public List<string> EmailList()
    {
        return GetAll().Select(b => b.Email).ToList();
    }

}