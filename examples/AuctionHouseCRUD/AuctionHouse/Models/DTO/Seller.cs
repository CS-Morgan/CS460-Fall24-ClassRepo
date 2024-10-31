using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuctionHouse.Models.DTO
{
    /// <summary>
    /// This class is a Data Transfer Object (DTO) for the Seller class.  It is used for _external_ communication
    /// about our Seller entity, which remains private to our application.  Objects of this class are serialized
    /// to JSON and sent out over the wire to our clients.
    /// 
    /// This is your opportunity to customize what the external world sees about this entity.  You can choose to
    /// include the related entities (e.g. Items) or not.  You can rename properties, or exclude them entirely.
    /// 
    /// Here it makes sense to name it the same as the entity, but with a different namespace.  Use the fully qualified 
    /// name to disambiguate between the two.
    /// 
    /// It is unfortunate that doing this requires duplicating the properties from the internal Seller class.  It would be nice 
    /// if C# allowed you to "extend" another class and choose which properties to inherit or not, but it doesn't.
    /// </summary>
    public class Seller
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(15)]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [StringLength(12)]
        public string TaxIdnumber { get; set; }
    }
}

// Add a using to enable extension methods wherever you want to use them
namespace AuctionHouse.ExtensionMethods
{
    /// <summary>
    /// This class contains extension methods for the Seller class.  Extension methods allow you to add methods
    /// to existing classes without modifying the original class.  This is useful when you want to add functionality
    /// such as this: to convert from the internal Seller class to the external Seller DTO class.
    /// 
    /// This becomes a ToSeller() method in the Seller class.
    /// 
    /// You'll have to be very careful using these when your model has a Foreign Key property.  If you leave it out
    /// and then update/save changes then you'll lose the relationship.  If you modify a FK value and it's invalid
    /// you'll either get the wrong relation or you'll violate a constraint if it doesn't exist.
    /// </summary>
    public static class SellerExtensions
    {
        public static AuctionHouse.Models.DTO.Seller ToSellerDTO(this AuctionHouse.Models.Seller seller)
        {
            return new AuctionHouse.Models.DTO.Seller
            {
                Id = seller.Id,
                FirstName = seller.FirstName,
                LastName = seller.LastName,
                Email = seller.Email,
                Phone = seller.Phone,
                TaxIdnumber = seller.TaxIdnumber
            };
        }

        public static AuctionHouse.Models.Seller ToSeller(this AuctionHouse.Models.DTO.Seller seller)
        {
            return new AuctionHouse.Models.Seller
            {
                Id = seller.Id,
                FirstName = seller.FirstName,
                LastName = seller.LastName,
                Email = seller.Email,
                Phone = seller.Phone,
                TaxIdnumber = seller.TaxIdnumber
            };
        }
    }
}

