using AuctionHouse.Models;

// Here is a starting point to help you use a good testable design for your application logic

// Put this in a DAL/Abstract folder (DAL stands for Data Access Layer)
namespace AuctionHouse.DAL.Abstract;

public interface IBuyerRepository : IRepository<Buyer>
{
    // Not needed as we moved it up into the base repo class
    //List<Buyer> Buyers();
    
    int NumberOfBuyers();

    List<string> EmailList();

}