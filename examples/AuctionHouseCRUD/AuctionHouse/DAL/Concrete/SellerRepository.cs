using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AuctionHouse.DAL.Abstract;
using AuctionHouse.Models;

namespace AuctionHouse.DAL.Concrete;

public class SellerRepository : Repository<Seller>, ISellerRepository
{
    public SellerRepository(AuctionHouseDbContext ctx) : base(ctx)
    {

    }

    public bool TaxIdAlreadyInUse(string taxID)
    {
        return GetAll().Any(s => s.TaxIdnumber == taxID);
    }

}