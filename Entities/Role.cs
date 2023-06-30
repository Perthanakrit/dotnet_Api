﻿using System;
using System.Collections.Generic;

namespace dotnet_Api.Entities;

public partial class Role
{
    public int RoleId { get; set; }

    public string Name { get; set; }

    public DateTime Created { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
