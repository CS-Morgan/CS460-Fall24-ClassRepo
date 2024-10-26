using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse.Models;

[Table("Bid")]
public partial class Bid
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column(TypeName = "decimal(17, 2)")]
    public decimal? Price { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TimeSubmitted { get; set; }

    [Column("BuyerID")]
    public int? BuyerId { get; set; }

    [Column("ItemID")]
    public int? ItemId { get; set; }

    [ForeignKey("BuyerId")]
    [InverseProperty("Bids")]
    public virtual Buyer Buyer { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("Bids")]
    public virtual Item Item { get; set; }
}
