using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Budget.Mobile.Services;
using System.Collections.ObjectModel;
using Microcharts;
using SkiaSharp;
using Budget.Mobile.Models;

namespace Budget.Mobile.ViewModels
{ 
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly DepenseService _depenseService;

        [ObservableProperty]
        private decimal _solde;

        [ObservableProperty]
        private decimal _totalRevenus;

        [ObservableProperty]
        private decimal _totalDepenses;

        [ObservableProperty]
        private Chart _graphiqueDepenses;

        public ObservableCollection<ItemLegende> MaLegende { get; } = new();

        public DashboardViewModel(DepenseService depenseService)
        {
            _depenseService = depenseService;
        }

        [RelayCommand]
        public async Task ChargerDonnees()
        {
            var listeComplete = await _depenseService.GetDepensesAsync();

            var now = DateTime.Now;
            var listeMois = listeComplete
                .Where(d => d.Date_Depense.Month == now.Month && d.Date_Depense.Year == now.Year)
                .ToList();

            var dataTriee = listeMois // Utilise bien ta liste filtrée par mois !
                .Where(d => !d.Est_Revenu)
                .GroupBy(d => d.Categorie)
                .Select(g => new { Categorie = g.Key, Total = g.Sum(d => d.Montant)})
                .OrderByDescending(x => x.Total)
                .ToList();

            float totalDepensesMois = dataTriee.Sum(x => (float)x.Total);

            decimal revenus = 0;
            decimal depenses = 0;

            foreach (var transaction in listeMois) {
                if (transaction.Est_Revenu)
                {
                    revenus += transaction.Montant;
                }
                else
                {
                    depenses += transaction.Montant;
                }

            }

            TotalRevenus = listeMois.Where(d => d.Est_Revenu).Sum(d => d.Montant);
            TotalDepenses = listeMois.Where(d => !d.Est_Revenu).Sum(d => d.Montant);
            Solde = revenus - depenses;

            var entries = new List<ChartEntry>();
            MaLegende.Clear();

            var couleurs = new[] { SKColors.Orange, SKColors.Blue, SKColors.Red, SKColors.Green, SKColors.Purple, SKColors.Teal, SKColors.Gray };
            int indexCouleur = 0;

            foreach (var item in dataTriee)
            {
                var color = couleurs[indexCouleur % couleurs.Length];
                var pourcentage = totalDepensesMois > 0 ? (float)item.Total / totalDepensesMois : 0;

                entries.Add(new ChartEntry((float)item.Total)
                {
                    Label = "",

                    // On met le pourcentage dans le ValueLabel pour qu'il s'affiche sur le graph
                    ValueLabel = "",

                    Color = color,
                    ValueLabelColor = color // Le texte du % aura la couleur de la part
                });

                // B. On remplit TA légende personnalisée (Nom + Couleur)
                MaLegende.Add(new ItemLegende
                {
                    Nom = item.Categorie.ToString(), // Utilise ton Helper pour le joli nom
                    Couleur = Color.FromRgb(color.Red, color.Green, color.Blue) // Conversion Skia -> Maui Color
                });

                indexCouleur++;
            }

            GraphiqueDepenses = new DonutChart
            {
                Entries = entries,
                LabelTextSize = 40, // Taille du % sur le graphique
                BackgroundColor = SKColors.Transparent,
                HoleRadius = 0.5f,

                // IMPORTANT : On demande à Microcharts de ne pas essayer d'être intelligent
                // On veut juste voir nos ValueLabel (les %)
                LabelMode = LabelMode.None,
                GraphPosition = GraphPosition.Center
            };
        }
    }
}