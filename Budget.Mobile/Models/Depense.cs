using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.Mobile.Models
{
    public class Depense
    {
        public int Id { get; set; }
        public DateTime Date_Depense { get; set; }
        public TypeCategorie Categorie { get; set; }
        public string Description { get; set; }
        public decimal Montant { get; set; }
        public bool Est_Revenu { get; set; }

        public Color CouleurAffichage => Est_Revenu ? Colors.Green : Colors.Red;
        public string MontantFormate => Est_Revenu ? $"+ {Montant:C}" : $"- {Montant:C}";
    }
}
