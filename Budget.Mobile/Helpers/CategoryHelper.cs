using Budget.Mobile.Models; // Ton namespace où est l'Enum

namespace Budget.Mobile.Helpers
{
    public static class CategoryHelper
    {
        public static string GetNomAffichage(TypeCategorie cat)
        {
            return cat switch
            {
                TypeCategorie.Alimentation => "Alimentation",
                TypeCategorie.Loyer => "Loyer & Charges",
                TypeCategorie.Transport => "Transport",
                TypeCategorie.JeuxVideo => "Jeux Vidéo",
                TypeCategorie.Restaurant => "Restaurant",
                TypeCategorie.Salaire => "Salaire / Revenus",
                TypeCategorie.Epargne => "Épargne",
                TypeCategorie.Autres => "Divers",
                TypeCategorie.RemboursementPrinceEdouard2026 => "Remboursement Prince Edouard 2026",
                TypeCategorie.AbonnementTV => "Abonnement TV",
                TypeCategorie.AbonnementMusique => "Abonnement Musique",
                TypeCategorie.AbonnementJeux => "Abonnement Jeux",
                TypeCategorie.Beaute => "Beauté",
                TypeCategorie.TelephoneInternet => "Téléphone/Internet",
                TypeCategorie.DepensesPrinceEdouard2026 => "Dépense Prince Edouard 2026",
                TypeCategorie.Electricite => "Électricité",
                _ => cat.ToString() // Par défaut, renvoie le nom technique
            };
        }
    }
}