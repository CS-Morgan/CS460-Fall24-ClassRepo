using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuctionHouse.Models;
using AuctionHouse.DAL.Abstract;

namespace AuctionHouse.Controllers
{
    public class SellerController : Controller
    {
        private readonly ISellerRepository _sellerRepository;

        public SellerController(ISellerRepository sellerRepo)
        {
            _sellerRepository = sellerRepo;
        }

        // GET: Seller
        public IActionResult Index()
        {
            var sellers = _sellerRepository.GetAll().ToList();
            return View(sellers);
        }

        // GET: Seller/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = _sellerRepository.FindById(id.Value);  // NOTE: this will NOT load navigation properties
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }

        // GET: Seller/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Seller/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstName,LastName,Email,Phone,TaxIdnumber")] Seller seller)
        {
            if (ModelState.IsValid)
            {
                if(_sellerRepository.TaxIdAlreadyInUse(seller.TaxIdnumber))
                {
                    ModelState.AddModelError("TaxIdnumber", "The Tax ID entered is invalid.  Please enter your correct ID.");
                    return View(seller);
                }
                try
                {
                    // AddOrUpdate in our repository calls SaveChanges
                    // the PK should already be 0 because we didn't bind it, but if we're creating a new one then it must be
                    // the default value for the given type (int = 0, string = null, etc.) or else the AddOrUpdate will try to perform 
                    // an update instead of an insert.  Just to be sure future changes don't cause a bug, we'll explicitly set it here
                    seller.Id = default(int);
                    // This is also where you can perform anything else needed server-side prior to creating the new Seller
                    _sellerRepository.AddOrUpdate(seller);
                }
                catch(DbUpdateConcurrencyException e)
                {
                    ViewBag.Message = "A concurrency error occurred while trying to add a new Seller.  Please try again.";
                    return View(seller);
                }
                catch(DbUpdateException e)
                {
                    ViewBag.Message = "An unknown database error occurred while trying to add a new Seller.  Please try again.";
                    return View(seller);
                }
                return RedirectToAction(nameof(Index));
            }
            // NOTE: if we get here, the model state is invalid and we need to redisplay the form
            return View(seller);
        }

        // GET: Seller/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = _sellerRepository.FindById(id.Value);
            if (seller == null)
            {
                return NotFound();
            }
            return View(seller);
        }

        // POST: Seller/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,FirstName,LastName,Email,Phone,TaxIdnumber")] Seller seller)
        {
            // Now we do want to bind the Id coming in so we can use it to compare to the one from the query string
            if (id != seller.Id || !_sellerRepository.Exists(seller.Id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Here's your chance to do anything that might need to be done prior to updating
                    var existingSeller = _sellerRepository.FindById(seller.Id);
                    existingSeller.FirstName = seller.FirstName;
                    existingSeller.LastName = seller.LastName;
                    existingSeller.Email = seller.Email;
                    existingSeller.Phone = seller.Phone;
                    // existingSeller.TaxIdnumber = seller.TaxIdnumber;    // let's say this one requires some other validation before updating so we won't do it here for example
                    _sellerRepository.AddOrUpdate(existingSeller);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_sellerRepository.Exists(seller.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;  // I left this as the template produced it.  Returns 5xx Server Error in this case.  Basically
                                // meaning you will have to figure out what to do based on what your app is doing if we get to this point
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(seller);
        }

        // GET: Seller/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = _sellerRepository.FindById(id.Value);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }

        // POST: Seller/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seller = _sellerRepository.FindById(id);
            if(seller == null)
            {
                return NotFound();
            }

            if (seller != null)
            {
                try
                {
                    _sellerRepository.Delete(seller);
                }
                catch(Exception e)
                {
                    throw;      // same here, you decide what makes sense here based on your app
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
