using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.Mobile.Models
{
    public class ResumeMois
    {
        public int NumeroMois { get; set; } // 1 à 12
        public string NomMois { get; set; } // "Janvier", "Février"...
        public decimal TotalRevenus { get; set; }
        public decimal TotalDepenses { get; set; }

        // Propriété calculée pour l'affichage facile
        public decimal Solde => TotalRevenus - TotalDepenses;
    }
}