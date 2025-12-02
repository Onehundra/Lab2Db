using System;
using System.Collections.Generic;

namespace Lab2Db.Models;

public partial class Kunder
{
    public int KundId { get; set; }

    public string Förnamn { get; set; } = null!;

    public string Efternamn { get; set; } = null!;

    public string? Email { get; set; }

    public virtual ICollection<Ordrar> Ordrars { get; set; } = new List<Ordrar>();
}
