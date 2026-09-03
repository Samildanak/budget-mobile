namespace Budget.Mobile.Models;

public class ProjetItem
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public bool EstBoutonAjout { get; set; }

    // Propriété d'affichage directe
    public bool EstProjetNormal => !EstBoutonAjout;

    public static ProjetItem CreerBoutonAjout() => new() { EstBoutonAjout = true };
    public static ProjetItem DepuisProjet(Projet p) => new() { Id = p.Id, Nom = p.Nom, EstBoutonAjout = false };
}