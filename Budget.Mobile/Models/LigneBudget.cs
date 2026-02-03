using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.Mobile.Models
{
    public class LigneBudget
    {
        public TypeCategorie Categorie { get; set; }
        public string NomCategorie { get; set; }

        public decimal PlafondDefini { get; set; }
        public decimal MoyenneDepenseReelle { get; set; }

        public decimal TotalDepenseAnnee { get; set; }
    }
}