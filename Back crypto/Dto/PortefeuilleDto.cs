﻿using Backend_Crypto.Models;
using System.ComponentModel;

namespace Backend_Crypto.Dto
{
    public class PortefeuilleDto
    {
        public int IdPortefeuille { get; set; }

        [DefaultValue(0)]
        public double Fond { get; set; } = 0;
        public ICollection<StockDto> Stock { get; set; } = new List<StockDto>();
    }
}
