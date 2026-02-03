using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Budget.Mobile.Services;
using Budget.Mobile.Models;
using System.Collections.ObjectModel;
using System.Globalization; // Pour avoir les noms des mois en français

namespace Budget.Mobile.ViewModels
{
    public partial class CalendarViewModel : ObservableObject
    {
        private readonly DepenseService _service;

        public ObservableCollection<ResumeMois> MoisDeLAnnee { get; } = new();

        // Totaux Annuels
        [ObservableProperty] private int _anneeSelectionnee;
        [ObservableProperty] private decimal _totalRevenusAnnee;
        [ObservableProperty] private decimal _totalDepensesAnnee;
        [ObservableProperty] private decimal _soldeAnnee;

        public CalendarViewModel(DepenseService service)
        {
            _service = service;
            AnneeSelectionnee = DateTime.Now.Year; // Par défaut : cette année
        }

        [RelayCommand]
        public async Task ChargerDonnees()
        {
            var dataBrute = await _service.GetResumeAnnuelAsync(AnneeSelectionnee);

            MoisDeLAnnee.Clear();
            decimal totalRev = 0;
            decimal totalDep = 0;

            // On boucle sur les données reçues
            // NOTE : SQL ne renvoie que les mois où il y a eu des mouvements.
            // Si tu veux afficher les mois "vides" aussi, il faudra une boucle for(1 à 12).
            foreach (var mois in dataBrute)
            {
                // On met un joli nom (Ex: 1 -> "Janvier")
                mois.NomMois = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mois.NumeroMois);
                // On met la première lettre en majuscule (Optionnel)
                mois.NomMois = char.ToUpper(mois.NomMois[0]) + mois.NomMois.Substring(1);

                // On cumule pour le total annuel
                totalRev += mois.TotalRevenus;
                totalDep += mois.TotalDepenses;

                MoisDeLAnnee.Add(mois);
            }

            TotalRevenusAnnee = totalRev;
            TotalDepensesAnnee = totalDep;
            SoldeAnnee = totalRev - totalDep;
        }

        // Pour changer d'année (Boutons Précédent/Suivant)
        [RelayCommand]
        public async Task ChangerAnnee(int increment)
        {
            AnneeSelectionnee += increment;
            await ChargerDonnees();
        }
    }
}