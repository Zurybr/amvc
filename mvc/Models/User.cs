using System;
using System.Collections.Generic;

namespace mvc.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public DateTime? Birthday { get; set; }

    public string Username { get; set; } = null!;
}
