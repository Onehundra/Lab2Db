using System;
using System.Collections.Generic;

namespace Lab2Db.Models;

public partial class Butiker
{
    public int Id { get; set; }

    public string ButikensNamn { get; set; } = null!;

    public string GatuAdress { get; set; } = null!;

    public string PostNummer { get; set; } = null!;

    public string Stad { get; set; } = null!;

    public virtual ICollection<LagerSaldo> LagerSaldos { get; set; } = new List<LagerSaldo>();
}
