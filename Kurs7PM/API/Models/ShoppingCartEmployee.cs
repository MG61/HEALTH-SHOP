﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs7PM.API.Models
{
    internal class ShoppingCartEmployee
    {
        public int ID_cart { get; set; }

        public string Название { get; set; }

        public string Количество { get; set; }

        public string Цена { get; set; }
    }
}
