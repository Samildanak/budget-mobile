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
        private List<Depense> _historiqueDepenses = new(); // Cache local pour l'autocomplétion

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (SetProperty(ref _description, value))
                {
                    MettreAJourSuggestions(value);
                }
            }
        }

        [ObservableProperty] private decimal _montant;
        [ObservableProperty] private DateTime _dateTransaction = DateTime.Now;
        [ObservableProperty] private TypeCategorie _categorieSelectionnee;
        [ObservableProperty] private bool _estRevenu;

        public ObservableCollection<TypeCategorie> Categories { get; } = new();
        public ObservableCollection<Depense> DepensesEnAttente { get; } = new();
        public ObservableCollection<Depense> Suggestions { get; } = new(); // Pour la liste d'autocomplétion

        [ObservableProperty] private bool _aDesSuggestions; // Gère la visibilité de la liste

        public bool IsDesktop => DeviceInfo.Idiom == DeviceIdiom.Desktop;
        public bool IsMobile => !IsDesktop;

        public AddTransactionViewModel(DepenseService service)
        {
            _service = service;
            var categoriesTriees = Enum.GetValues(typeof(TypeCategorie))
                               .Cast<TypeCategorie>()
                               .OrderBy(c => c.ToString());

            foreach (var cat in categoriesTriees)
            {
                Categories.Add(cat);
            }

            CategorieSelectionnee = TypeCategorie.Alimentation;
            _ = InitialiserHistoriqueAsync();
        }

        private async Task InitialiserHistoriqueAsync()
        {
            _historiqueDepenses = await _service.GetDepensesAsync();
        }

        private void MettreAJourSuggestions(string texte)
        {
            if (string.IsNullOrWhiteSpace(texte) || texte.Length < 2)
            {
                Suggestions.Clear();
                ADesSuggestions = false;
                return;
            }

            // Recherche les descriptions uniques (ignorant la casse) qui commencent par le texte saisi
            var résultats = _historiqueDepenses
                .Where(d => d.Description != null && d.Description.StartsWith(texte, StringComparison.OrdinalIgnoreCase))
                .GroupBy(d => d.Description)
                .Select(g => g.First()) // Évite les doublons de "Maxi"
                .Take(5) // Max 5 suggestions
                .ToList();

            Suggestions.Clear();
            foreach (var dep in résultats)
            {
                Suggestions.Add(dep);
            }

            ADesSuggestions = Suggestions.Count > 0;
        }

        [RelayCommand]
        private void SelectionnerSuggestion(Depense suggestion)
        {
            if (suggestion == null) return;

            // On applique le nom et la catégorie associée
            Description = suggestion.Description;
            CategorieSelectionnee = suggestion.Categorie;
            EstRevenu = suggestion.Est_Revenu;

            // On ferme la liste
            Suggestions.Clear();
            ADesSuggestions = false;
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

            bool confirm = await Shell.Current.DisplayAlert("Validation",
                $"Voulez-vous envoyer ces { DepensesEnAttente.Count } dépenses ?", "Oui", "Non");

            if (!confirm) return;

            foreach (var dep in DepensesEnAttente)
            {
                await _service.AddDepenseAsync(dep);
            }

            DepensesEnAttente.Clear();
            await Shell.Current.DisplayAlert("Succès", "Tout a été envoyé !", "OK");
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
            // Évite de casser la sélection automatique lors du clic sur une suggestion
            if (Description?.Length > 0 && Suggestions.Count == 0) return;
            EstRevenu = false;
            if (value == TypeCategorie.Salaire || value == TypeCategorie.RemboursementPrinceEdouard2026) // Ajouté selon ton helper
            {
                EstRevenu = true;
            }
        }
    }
}