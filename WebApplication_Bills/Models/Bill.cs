using System;
using System.Collections.Generic;

namespace WebApplication_Bills.Models;

public partial class Bill
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public int Amount { get; set; }

    public decimal UnitValue { get; set; }

    public decimal SubTotal { get; set; }

    public decimal PriceTotal { get; set; }

    public string CreatedBy { get; set; } = null!;

    public byte[] CreatedAt { get; set; } = null!;

    public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();
}
