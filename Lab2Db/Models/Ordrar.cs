using System;
using System.Collections.Generic;

namespace Lab2Db.Models;

public partial class Ordrar
{
    public int OrderId { get; set; }

    public int KundId { get; set; }

    public DateOnly OrderDatum { get; set; }

    public virtual Kunder Kund { get; set; } = null!;
}
