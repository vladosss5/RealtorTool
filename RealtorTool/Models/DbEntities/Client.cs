using System;
using System.Collections.Generic;

namespace RealtorTool.Models.DbEntities;

/// <summary>
/// Клиенты.
/// </summary>
public partial class Client
{
    public int Id { get; set; }

    public string Fname { get; set; } = null!;

    public string Sname { get; set; } = null!;

    public string? Lname { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
