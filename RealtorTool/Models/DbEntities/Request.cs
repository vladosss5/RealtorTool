using System;
using System.Collections.Generic;

namespace RealtorTool.Models.DbEntities;

public partial class Request
{
    public int Id { get; set; }

    public int? ClientId { get; set; }

    public int EmployeeId { get; set; }

    public int TypeId { get; set; }

    public string? RequestData { get; set; }

    public int? IdRealtyInDeal { get; set; }

    public virtual Client? Client { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Realty? Realty { get; set; }

    public virtual DictionaryValue Type { get; set; } = null!;
}
