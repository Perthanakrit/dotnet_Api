﻿using System;
using System.Collections.Generic;

namespace dotnet_Api.Entities;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Name { get; set; }

    public DateTime Created { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
