using AuctionHouse.Models;

namespace AuctionHouse.DAL.Abstract;

public interface ISellerRepository : IRepository<Seller>
{
    bool TaxIdAlreadyInUse(string taxID);

}