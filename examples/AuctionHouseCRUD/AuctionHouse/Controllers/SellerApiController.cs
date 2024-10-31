using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuctionHouse.Models;
using AuctionHouse.DAL.Abstract;
using AuctionHouse.ExtensionMethods;

namespace AuctionHouse.Controllers
{
    [Route("api/seller")]
    [ApiController]
    public class SellerApiController : ControllerBase
    {
        private readonly ISellerRepository _sellerRepository;

        private readonly AuctionHouseDbContext _context;

        public SellerApiController(ISellerRepository sellerRepo)
        {
            _sellerRepository = sellerRepo;
        }

        // GET: api/seller/sellers
        [HttpGet("sellers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AuctionHouse.Models.DTO.Seller>))]
        public ActionResult<IEnumerable<AuctionHouse.Models.DTO.Seller>> GetSellers()
        {
            var sellers = _sellerRepository.GetAll()
                                           .Select(s => s.ToSellerDTO())
                                           .ToList();
            return sellers;
        }

        // GET: api/seller/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuctionHouse.Models.DTO.Seller))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<AuctionHouse.Models.DTO.Seller> GetSeller(int id)
        {

            var seller = _sellerRepository.FindById(id);
            if (seller == null)
            {
                return NotFound();
            }

            return seller.ToSellerDTO();
        }

        // It really would be easier to separate these into two different endpoints, one for CREATE and one for UPDATE
        // but I put it all in one as that is the "correct" behavior of a PUT request.  You don't always have to be "correct".

        // PUT acts as CREATE and UPDATE ALL, i.e. replace; PATCH means update some (see: https://developer.mozilla.org/en-US/docs/Web/HTTP/Methods/PUT)
        // PUT: api/seller/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status201Created), ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PutSeller(int id, AuctionHouse.Models.DTO.Seller seller)
        {
            // Model validation is applied automatically inside ApiController's and result in an automatic 400 (BadRequest) response
            if (id != seller.Id)
            {
                return Problem(detail: "Invalid ID", statusCode: 400);
            }
            Seller sellerEntity;
            if (seller.Id == 0)     // CREATE
            {
                // Disallow using an existing Tax ID for a new Seller
                if (_sellerRepository.TaxIdAlreadyInUse(seller.TaxIdnumber))
                {
                    return Problem(detail: "The Tax ID entered is invalid.  Please enter your correct ID.", statusCode: 400);
                }
                sellerEntity = seller.ToSeller();
            }
            else                    // UPDATE
            {
                // Retrieve existing entity from database
                sellerEntity = _sellerRepository.FindById(seller.Id);
                if (sellerEntity == null)
                {
                    return Problem(detail: "Invalid ID", statusCode: 400);
                }
                // Don't let them change their TaxIdnumber
                if(seller.TaxIdnumber != sellerEntity.TaxIdnumber)
                {
                    return Problem(detail: "The Tax ID entered is invalid.  Please enter your correct ID.", statusCode: 400);
                }
                // Update the entity from the DTO (can put this in a method if you have a lot of properties)
                sellerEntity.FirstName = seller.FirstName;
                sellerEntity.LastName = seller.LastName;
                sellerEntity.Email = seller.Email;
                sellerEntity.Phone = seller.Phone;
                // don't copy over the TaxIdnumber since it can't change
            }
            try
            {
                // If you have FK's missing or other data then this is the time to retrieve it before updating, otherwise
                // you can lose data
                _sellerRepository.AddOrUpdate(sellerEntity);
            }
            catch (DbUpdateConcurrencyException e)
            {
                
                return Problem(detail: "The server experienced a problem.  Please try again.", statusCode: 500);
            }
            catch (DbUpdateException e)
            {
                return Problem(detail: "The server experienced a problem.  Please try again.", statusCode: 500);
            }
            // Return accepted status codes depending on Create or Update
            if(id == 0)
            {
                return CreatedAtAction("GetSeller", new { id = seller.Id }, seller);
            }
            else
            {
                return NoContent();
            }   
        }

        // POSTing a Seller makes no sense, so remove it

        // DELETE: api/seller/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteSeller(int id)
        {
            var seller = _sellerRepository.FindById(id);
            if (seller == null)
            {
                return NotFound();
            }

            try
            {
                _sellerRepository.Delete(seller);
            }
            catch (Exception e)
            {
                return Problem(detail: "The server experienced a problem.  Please try again.", statusCode: 500);
            }

            return NoContent();
        }

    }
}
