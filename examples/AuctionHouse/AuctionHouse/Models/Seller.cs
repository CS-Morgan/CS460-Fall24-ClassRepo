using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse.Models;

[Table("Seller")]
public partial class Seller
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

    [StringLength(15)]
    public string Phone { get; set; }

    [Required]
    [Column("TaxIDNumber")]
    [StringLength(12)]
    public string TaxIdnumber { get; set; }

    [InverseProperty("Seller")]
    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
