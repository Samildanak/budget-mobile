using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Budget.Mobile.Services;
using Budget.Mobile.Models;

namespace Budget.Mobile.ViewModels
{
    public partial class DepensesViewModel : ObservableObject
    {
        private readonly DepenseService _depenseService;

        public ObservableCollection<Depense> ListeDepenses { get; } = new();

        [ObservableProperty]
        private string _nouvelleDescription;

        [ObservableProperty]
        private decimal _montant;

        [ObservableProperty]
        private TypeCategorie _categorie;

        public List<CategorieAffichage> CategoriesDisponibles { get; } = Enum.GetValues(typeof(TypeCategorie))
            .Cast<TypeCategorie>()
            .Select(c => new CategorieAffichage
            {
                Valeur = c,
                Texte = c.ToString()
                         .Replace("AbonnementJeux", "Abonnement Jeux")
                         .Replace("AbonnementTV", "Abonnement TV")
                         .Replace("JeuxVideo", "Jeux Vidéo")
                         .Replace("TelephoneInternet", "Téléphone/Internet")
                         .Replace("DepensesPrinceEdouard2026", "Dépenses Prince Edouard 2026")
            })
            .ToList();

        [ObservableProperty]
        private CategorieAffichage _categorieSelectionnee;

        [ObservableProperty]
        private DateTime _date = DateTime.Now;

        [ObservableProperty]
        private bool _estRevenu;

        [ObservableProperty]
        private decimal _solde;

        [ObservableProperty]
        private decimal _totalRevenus;

        [ObservableProperty]
        private decimal _totalDepenses;

        private int _idEnEndition = 0;

        public DepensesViewModel(DepenseService depenseService)
        {
            _depenseService = depenseService;
            CategorieSelectionnee = CategoriesDisponibles.First();
        }

        private void CalculerSolde()
        {
            decimal total = 0;
            TotalRevenus = 0;
            TotalDepenses = 0;

            foreach (var d in ListeDepenses)
            {
                if (d.Est_Revenu)
                {
                    total += d.Montant;
                    TotalRevenus += d.Montant;

                }
                else
                {
                    total -= d.Montant;
                    TotalDepenses += d.Montant;
                }
            }
            Solde = total;
        }


        [RelayCommand]
        public async Task ChargerDepenses()
        {
            try
            {
                var depenses = await _depenseService.GetDepensesAsync();

                ListeDepenses.Clear();

                foreach (var depense in depenses)
                {
                    ListeDepenses.Add(depense);
                }

                CalculerSolde();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur : {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task AjouterDepense()
        {
            if (NouvelleDescription is null)
            {
                System.Diagnostics.Debug.WriteLine("Erreur : La description ne peut pas être vide ");
                return;
            }
            if (Montant == 0)
            {
                System.Diagnostics.Debug.WriteLine("Erreur : Le montant ne peut pas être 0 ");
                return;
            }

            bool response;

            Depense depense = new();

            depense.Date_Depense = Date;
            depense.Categorie = CategorieSelectionnee.Valeur;
            depense.Montant = Montant;
            depense.Description = NouvelleDescription;
            depense.Est_Revenu = EstRevenu;

            try
            {
                if (_idEnEndition != 0)
                {
                    depense.Id = _idEnEndition;
                    response = await _depenseService.UpdateDepenseAsync(depense);
                }
                else
                {
                    response = await _depenseService.AddDepenseAsync(depense);
                }

                if (response)
                {
                    Date = DateTime.Now;
                    CategorieSelectionnee = CategoriesDisponibles.First();
                    Montant = 0;
                    NouvelleDescription = "";
                    _idEnEndition = 0;
                    EstRevenu = false;

                    await ChargerDepenses();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Erreur");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur : {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task SupprimerDepense(Depense depense)
        {
            try
            {
                var response = await _depenseService.DeleteDepenseAsync(depense.Id);
                if (response) 
                {
                    ListeDepenses.Remove(depense);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Erreur");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur : {ex.Message}");
            }
        }

        [RelayCommand]
        public void RemplirFormulaire(Depense depense)
        {
            _idEnEndition = depense.Id;
            Date = depense.Date_Depense;
            CategorieSelectionnee = CategoriesDisponibles.FirstOrDefault(c => c.Valeur == depense.Categorie);
            Montant = depense.Montant;
            NouvelleDescription = depense.Description;
            EstRevenu = depense.Est_Revenu;
        }
    }

}
