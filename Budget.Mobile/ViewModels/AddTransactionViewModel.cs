using Budget.Mobile.Models;
using Budget.Mobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Budget.Mobile.ViewModels
{
    public partial class AddTransactionViewModel : ObservableObject
    {
        private readonly DepenseService _service;

        [ObservableProperty] private string _description;
        [ObservableProperty] private decimal _montant;
        [ObservableProperty] private DateTime _dateTransaction = DateTime.Now;
        [ObservableProperty] private TypeCategorie _categorieSelectionnee;
        [ObservableProperty] private bool _estRevenu;

        public ObservableCollection<TypeCategorie> Categories { get; } = new();
        public ObservableCollection<Depense> DepensesEnAttente { get; } = new();

        public bool IsDesktop => DeviceInfo.Idiom == DeviceIdiom.Desktop;
        public bool IsMobile => !IsDesktop;

        public AddTransactionViewModel(DepenseService service)
        {
            _service = service;
            foreach (TypeCategorie cat in Enum.GetValues(typeof(TypeCategorie)))
                Categories.Add(cat);
            CategorieSelectionnee = TypeCategorie.Alimentation;
        }

        [RelayCommand]
        public async Task Sauvegarder()
        {
            if (string.IsNullOrWhiteSpace(Description) || Montant <= 0)
                return; // Validation basique

            var nouvelleDepense = new Depense
            {
                Description = Description,
                Montant = Montant,
                Date_Depense = DateTransaction,
                Categorie = CategorieSelectionnee,
                Est_Revenu = EstRevenu
            };

            if (IsDesktop)
            {
                DepensesEnAttente.Add(nouvelleDepense);

                Description = string.Empty;
                Montant = 0;
            }
            else
            {
                await _service.AddDepenseAsync(nouvelleDepense);
                await Shell.Current.GoToAsync("//DashboardPage");
            }
  
        }

        [RelayCommand]
        public async Task ValiderLeLot()
        {
            if (DepensesEnAttente.Count == 0) return;

            bool confirm = await Shell.Current.DisplayAlertAsync("Validation",
                $"Voulez-vous envoyer ces { DepensesEnAttente.Count } dépenses ?", "Oui", "Non");

            if (!confirm) return;

            foreach (var dep in DepensesEnAttente)
            {
                await _service.AddDepenseAsync(dep);
            }

            DepensesEnAttente.Clear();
            await Shell.Current.DisplayAlertAsync("Succès", "Tout a été envoyé !", "OK");
        }

        [RelayCommand]
        void SupprimerLigne(Depense depense)
        {
            if (DepensesEnAttente.Contains(depense))
            {
                DepensesEnAttente.Remove(depense);
            }
        }

        partial void OnCategorieSelectionneeChanged(TypeCategorie value)
        {
            EstRevenu = false;
            if (value == TypeCategorie.Salaire || value ==TypeCategorie.Remboursement)
            {
                EstRevenu = true;
            }
        }
    }
}