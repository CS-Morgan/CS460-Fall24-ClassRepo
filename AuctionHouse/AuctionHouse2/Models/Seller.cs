using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse2.Models;

[Table("Seller")]
public partial class Seller
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    public string Email { get; set; } = null!;

    [StringLength(15)]
    public string? Phone { get; set; }

    [Column("TaxID")]
    [StringLength(12)]
    [Unicode(false)]
    public string TaxId { get; set; } = null!;

    [InverseProperty("Seller")]
    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
