﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Models
{
    public class CartVM
    {
        public IEnumerable<CartItem> CartItems { get; set; }
        public double Total { get; set; }
    }
}
