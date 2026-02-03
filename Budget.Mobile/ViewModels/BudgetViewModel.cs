using Budget.Mobile.Helpers;
using Budget.Mobile.Models;
using Budget.Mobile.Services;
using Budget.Mobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics; // Pour les couleurs
using System.Collections.ObjectModel;

namespace Budget.Mobile.ViewModels
{
    public partial class BudgetViewModel : ObservableObject
    {
        private readonly DepenseService _service;

        // La liste que l'écran va afficher
        public ObservableCollection<BudgetRow> LignesBudget { get; } = new();

        public BudgetViewModel(DepenseService service)
        {
            _service = service;
        }

        [RelayCommand]
        public async Task ChargerBudget()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                // 1. On récupère l'année et le mois en cours
                var now = DateTime.Now;
                int moisEnCours = now.Month; // Ex: 2 pour Février
                int anneeEnCours = now.Year;


                // 2. On récupère les données brutes (Dépenses cumulées de l'année)
                var data = await _service.GetBudgetSuiviAsync(anneeEnCours);
                var toutesDepenses = await _service.GetDepensesAsync();

                var listeTemporaire = new List<BudgetRow>();

                foreach (var ligne in data)
                {
                    // --- LA FORMULE MAGIQUE DU ROLLOVER ---

                    // Budget théorique depuis le 1er Janvier = Plafond mensuel * Mois en cours
                    decimal budgetCumuleTheorique = ligne.PlafondDefini * moisEnCours;

                    

                    if (budgetCumuleTheorique == 0)
                    {
                        continue;
                    }

                    var depensesCategorie = toutesDepenses
                        .Where(d => d.Categorie == ligne.Categorie && !d.Est_Revenu)
                        .ToList();

                    decimal totalAnnee = depensesCategorie
                        .Where(d => d.Date_Depense.Year == now.Year)
                        .Sum(d => d.Montant);

                    decimal x = depensesCategorie
                        .Where(d => d.Date_Depense.Year == now.Year && d.Date_Depense.Month == moisEnCours)
                        .Sum(d => d.Montant);

                    decimal depensesPrecedente = depensesCategorie
                        .Where(d => d.Date_Depense.Year == now.Year && d.Date_Depense.Month < moisEnCours)
                        .Sum(d => d.Montant);

                    // Combien il reste VRAIMENT
                    decimal reste = budgetCumuleTheorique - totalAnnee;
                    decimal budgetRestant = budgetCumuleTheorique - depensesPrecedente;

                    // Calcul du pourcentage pour la barre (0.0 à 1.0)
                    // Si budgetCumuleTheorique est 0, on évite la division par zéro
                    double progress = budgetCumuleTheorique == 0 ? 1 : (double)(x / budgetCumuleTheorique);

                    // On prépare l'affichage
                    var row = new BudgetRow
                    {
                        Nom = CategoryHelper.GetNomAffichage(ligne.Categorie), // Ou utilise ton helper pour le joli nom

                        // Ex: "700 $ / 1200 $"
                        DetailsCompteur = $"{x:C0} / {budgetRestant:C0}",

                        // Ex: "Reste : 500 $"
                        DetailsReste = $"Reste : {reste:C}",

                        Progression = progress,

                        // COULEUR : Rouge si on a dépassé le cumul, Vert sinon
                        CouleurBarre = reste < 0 ? Colors.Red : Colors.Green,

                        // Si on a dépassé, la barre doit être pleine (1.0) sinon elle dépasse de l'écran
                        ProgressionBarre = progress > 1 ? 1.0 : progress
                    };

                    listeTemporaire.Add(row);
                }

                var listeTriee = listeTemporaire.OrderByDescending(x => x.Progression).ToList();

                LignesBudget.Clear();

                foreach (var row in listeTriee)
                {
                    LignesBudget.Add(row);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task AllerVersEdition()
        {
            await Shell.Current.GoToAsync(nameof(EditBudgetPage));
        }

        [ObservableProperty]
        private bool _isBusy;
    }

    // --- PETITE CLASSE INTERNE POUR L'AFFICHAGE ---
    public class BudgetRow
    {
        public string Nom { get; set; }
        public string DetailsCompteur { get; set; } // "700 / 1200"
        public string DetailsReste { get; set; }    // "Reste : 500"
        public double Progression { get; set; }     // Valeur brute
        public double ProgressionBarre { get; set; } // Valeur plafonnée à 1 pour l'UI
        public Color CouleurBarre { get; set; }
    }


}