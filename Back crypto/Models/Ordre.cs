﻿using System.ComponentModel.DataAnnotations;

namespace Backend_Crypto.Models
{
    public class Ordre
    {
        public int IdOrdre { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Le prix doit être plus grand que 0.")]
        //[Required(ErrorMessage = "Le prix unitaire est nécéssaire")]
        public double PrixUnitaire { get; set; }

        [Required(ErrorMessage = "Quantité incorrecte")]
        public double AmountCrypto { get; set; }
        public int? IdTransaction { get; set; }
        public int IdCrypto { get; set; }
        public Crypto CryptoOrdre { get; set; } = new Crypto();
        public Transaction? Transac { get; set; }

    }
}
