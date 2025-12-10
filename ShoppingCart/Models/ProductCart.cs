using System;
using System.Collections.Generic;

namespace ShoppingCart.Models;

public partial class ProductCart
{
    public int Id { get; set; }

    public string Name { get; set; }

    public double Price { get; set; }

    public string Photo { get; set; }
}
