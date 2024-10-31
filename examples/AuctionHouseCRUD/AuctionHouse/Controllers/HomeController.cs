using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AuctionHouse.Models;
using Microsoft.AspNetCore.Http.Features;
using AuctionHouse.DAL.Abstract;
using AuctionHouse.ViewModels;

namespace AuctionHouse.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    AuctionHouseDbContext _context;
    private readonly IBuyerRepository _buyerRepository;
    private readonly IItemRepository _itemRepository;
    //public HomeController(ILogger<HomeController> logger, AuctionHouseDbContext context,IRepository<Buyer> buyerRepo)
    public HomeController(ILogger<HomeController> logger, AuctionHouseDbContext context, IBuyerRepository buyerRepo, IItemRepository itemRepo)
    {
        _logger = logger;
        _context = context;
        _buyerRepository = buyerRepo;
        _itemRepository = itemRepo;
    }

    public IActionResult Index()
    {
        
        List<Item> items = _context.Items.ToList();
        return View(items);
    }

    public IActionResult Buyers()
    {
        List<Buyer> buyers = _buyerRepository.GetAll().ToList();
        List<string> emailList = _buyerRepository.EmailList();

        BuyersVM vm = new BuyersVM();
        vm.EmailList = emailList;
        vm.Buyers = buyers;
        return View(vm);
    }

    public IActionResult Items()
    {
        List<string> itemNames = _itemRepository.ItemNames();
        return View(itemNames);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
