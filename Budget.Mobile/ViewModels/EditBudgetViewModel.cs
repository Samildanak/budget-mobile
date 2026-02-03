using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Budget.Mobile.Services;
using Budget.Mobile.Models;
using System.Collections.ObjectModel;

namespace Budget.Mobile.ViewModels
{
    public partial class EditBudgetViewModel : ObservableObject
    {
        private readonly DepenseService _service;

        public ObservableCollection<LigneBudget> LignesEdition { get; } = new();

        [ObservableProperty] private decimal _totalBudgetConfigure;
        [ObservableProperty] private decimal _revenuEstime;
        [ObservableProperty] private bool _budgetEstDeficitaire;
        [ObservableProperty] private bool _afficherMoyennes;
        [ObservableProperty] private Color _couleurTotalBudget;

        public EditBudgetViewModel(DepenseService service)
        {
            _service = service;
        }

        [RelayCommand]
        public async Task ChargerDonnees()
        {
            var budgetsExistants = await _service.GetBudgetDefinitionAsync();

            LignesEdition.Clear();

            var toutesLesCategories = Enum.GetValues(typeof(TypeCategorie)).Cast<TypeCategorie>();

            foreach (var cat in toutesLesCategories)
            {
                if (cat == TypeCategorie.Salaire) continue;

                var budgetTrouve = budgetsExistants.FirstOrDefault(b => b.Categorie == cat);

                if (budgetTrouve != null)
                {
                    budgetTrouve.NomCategorie = cat.ToString(); // Helper d'affichage si besoin
                    LignesEdition.Add(budgetTrouve);
                }
                else
                {
                    LignesEdition.Add(new LigneBudget
                    {
                        Categorie = cat,
                        NomCategorie = cat.ToString(),
                        PlafondDefini = 0,      // Pas encore de limite
                        MoyenneDepenseReelle = 0 // On n'a pas l'info SQL pour celui-là (acceptable pour l'instant)
                    });
                }
            }

            var ligneRevenu = budgetsExistants.FirstOrDefault(b => b.Categorie == TypeCategorie.Salaire);
            if (ligneRevenu != null)
            {
                RevenuEstime = ligneRevenu.PlafondDefini;
            }

            RecalculerTotaux();
        }

        partial void OnRevenuEstimeChanged(decimal value)
        {
            RecalculerTotaux();
        }

        public void RecalculerTotaux()
        {
            TotalBudgetConfigure = LignesEdition.Sum(l => l.PlafondDefini);

            BudgetEstDeficitaire = TotalBudgetConfigure > RevenuEstime;
            CouleurTotalBudget = BudgetEstDeficitaire ? Colors.Red : Colors.Green;
        }
        [RelayCommand]
        public async Task SauvegarderTout()
        {
            foreach (var ligne in LignesEdition)
            {
                await _service.SaveObjectifAsync(ligne);
            }

            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public void MettreAJourTotaux()
        {
            RecalculerTotaux();
        }
    }
}