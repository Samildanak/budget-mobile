namespace Budget.Mobile.Models;

public class Projet
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public DateTime Date_Creation { get; set; }
    public int[] Categories_Entrees { get; set; } = Array.Empty<int>();
    public int[] Categories_Sorties { get; set; } = Array.Empty<int>();
}

public class ProjetDetailsDto
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public DateTime DateCreation { get; set; }
    public decimal TotalEntrees { get; set; }
    public decimal TotalSorties { get; set; }
    public decimal Solde { get; set; }
    public List<DepenseDto> Transactions { get; set; } = new();
}

public class CreateProjetDto
{
    public string Nom { get; set; } = string.Empty;
    public List<int> CategoriesEntrees { get; set; } = new();
    public List<int> CategoriesSorties { get; set; } = new();
}

public class DepenseDto
{
    public int Id { get; set; }
    public DateTime Date_Depense { get; set; }
    public int Categorie { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Montant { get; set; }
    public bool Est_Revenu { get; set; }
}