using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse.Models;

[Table("Buyer")]
public partial class Buyer
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [Required]
    [StringLength(50)]
    public string Email { get; set; }

    [InverseProperty("Buyer")]
    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();
}
