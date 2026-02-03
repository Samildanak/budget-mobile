using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Budget.Mobile.Services;
using Budget.Mobile.Models;
using System.Collections.ObjectModel;

namespace Budget.Mobile.ViewModels
{
    public partial class TransactionsViewModel : ObservableObject
    {
        private readonly DepenseService _service;
        private List<Depense> _listeComplete; // Cache pour filtrer sans rappeler la BD

        public ObservableCollection<Depense> TransactionsAffichees { get; } = new();

        // --- FILTRES ---
        [ObservableProperty] private TypeCategorie? _categorieFiltre;
        [ObservableProperty] private bool _afficherRevenus = true;
        [ObservableProperty] private bool _afficherDepenses = true;

        // Pour la liste déroulante des catégories
        public ObservableCollection<TypeCategorie> CategoriesDisponibles { get; } = new();

        public TransactionsViewModel(DepenseService service)
        {
            _service = service;
            // On remplit la liste des catégories pour le filtre
            foreach (TypeCategorie cat in Enum.GetValues(typeof(TypeCategorie)))
                CategoriesDisponibles.Add(cat);
        }

        [RelayCommand]
        public async Task ChargerDonnees()
        {
            // 1. Charger tout depuis la BD
            var data = await _service.GetDepensesAsync();
            _listeComplete = data.OrderByDescending(d => d.Date_Depense).ToList();

            // 2. Appliquer les filtres
            Filtrer();
        }

        [RelayCommand]
        public void Filtrer()
        {
            if (_listeComplete == null) return;

            var result = _listeComplete.AsEnumerable();

            // Filtre Catégorie
            if (CategorieFiltre.HasValue)
                result = result.Where(d => d.Categorie == CategorieFiltre.Value);

            // Filtre Type (Revenu / Dépense)
            // Si on veut voir les revenus, on garde Est_Revenu=true. Sinon on les enlève.
            if (!AfficherRevenus) result = result.Where(d => !d.Est_Revenu);
            if (!AfficherDepenses) result = result.Where(d => d.Est_Revenu);

            TransactionsAffichees.Clear();
            foreach (var item in result)
                TransactionsAffichees.Add(item);
        }

        // Appelé quand on reset les filtres
        [RelayCommand]
        public void ResetFiltres()
        {
            CategorieFiltre = null;
            AfficherRevenus = true;
            AfficherDepenses = true;
            Filtrer();
        }

        [RelayCommand]
        private async Task Supprimer(Depense depenseASupprimer)
        {
            if (depenseASupprimer == null) return;

            // Correction 1 : C'est DisplayAlert, pas DisplayAlertAsync
            bool confirm = await Shell.Current.DisplayAlertAsync("Confirmation",
                $"Veux-tu vraiment supprimer : {depenseASupprimer.Description} ?",
                "Oui", "Non");

            if (confirm)
            {
                bool success = await _service.DeleteDepenseAsync(depenseASupprimer.Id);

                if (success)
                {
                    TransactionsAffichees.Remove(depenseASupprimer);

                    _listeComplete?.Remove(depenseASupprimer);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Oups", "Erreur lors de la suppression", "OK");
                }
            }
        }
    }
}