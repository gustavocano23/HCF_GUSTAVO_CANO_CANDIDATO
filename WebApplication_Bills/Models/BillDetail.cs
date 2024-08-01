using System;
using System.Collections.Generic;

namespace WebApplication_Bills.Models;

public partial class BillDetail
{
    public int Id { get; set; }

    public int BillId { get; set; }

    public string ItemName { get; set; } = null!;

    public string? ItemDescription { get; set; }

    public int Amount { get; set; }

    public decimal UnitValue { get; set; }

    public decimal SubTotal { get; set; }

    public virtual Bill Bill { get; set; } = null!;
}
