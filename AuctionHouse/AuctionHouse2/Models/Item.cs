using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse2.Models;

[Table("Item")]
public partial class Item
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(256)]
    public string Description { get; set; } = null!;

    [Column("SellerID")]
    public int? SellerId { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();

    [ForeignKey("SellerId")]
    [InverseProperty("Items")]
    public virtual Seller? Seller { get; set; }
}
