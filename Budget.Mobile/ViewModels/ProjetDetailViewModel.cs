using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Budget.Mobile.Helpers;
using Budget.Mobile.Models;
using Budget.Mobile.Services;
using Microsoft.Maui.Controls;

namespace Budget.Mobile.ViewModels;

// Reçoit le paramètre d'URL "id" transmis par Shell (ex: ProjetDetailPage?id=3)
[QueryProperty(nameof(ProjetId), "id")]
public class ProjetDetailViewModel : BindableObject
{
    private readonly DepenseService _depenseService;
    private int _projetId;
    private string _nomProjet = string.Empty;
    private decimal _totalEntrees;
    private decimal _totalSorties;
    private decimal _solde;
    private bool _isBusy;

    public int ProjetId
    {
        get => _projetId;
        set
        {
            _projetId = value;
            OnPropertyChanged();
        }
    }

    public string NomProjet
    {
        get => _nomProjet;
        set
        {
            _nomProjet = value;
            OnPropertyChanged();
        }
    }

    public decimal TotalEntrees
    {
        get => _totalEntrees;
        set
        {
            _totalEntrees = value;
            OnPropertyChanged();
        }
    }

    public decimal TotalSorties
    {
        get => _totalSorties;
        set
        {
            _totalSorties = value;
            OnPropertyChanged();
        }
    }

    public decimal Solde
    {
        get => _solde;
        set
        {
            _solde = value;
            OnPropertyChanged();
        }
    }

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<TransactionProjetItem> Transactions { get; } = new();

    public Command ChargerDetailsCommand { get; }

    public ProjetDetailViewModel(DepenseService depenseService)
    {
        _depenseService = depenseService;
        ChargerDetailsCommand = new Command(async () => await ChargerDetailsAsync());
    }

    public async Task ChargerDetailsAsync()
    {
        if (ProjetId <= 0 || IsBusy) return;

        try
        {
            IsBusy = true;
            var details = await _depenseService.GetProjetDetailsAsync(ProjetId);

            if (details != null)
            {
                NomProjet = details.Nom;
                TotalEntrees = details.TotalEntrees;
                TotalSorties = details.TotalSorties;
                Solde = details.TotalEntrees - details.TotalSorties;

                Transactions.Clear();
                foreach (var t in details.Transactions)
                {
                    // Récupération du libellé personnalisé via CategoryHelper
                    string nomCategorie = Enum.IsDefined(typeof(TypeCategorie), t.Categorie)
                        ? CategoryHelper.GetNomAffichage((TypeCategorie)t.Categorie)
                        : "Autre";

                    Transactions.Add(new TransactionProjetItem
                    {
                        Id = t.Id,
                        Date = t.Date_Depense,
                        Description = string.IsNullOrWhiteSpace(t.Description) ? nomCategorie : t.Description,
                        CategorieNom = nomCategorie,
                        Montant = t.Montant,
                        EstRevenu = t.Est_Revenu
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erreur ChargerDetails: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}

// Modèle d'affichage pour les lignes de transaction
public class TransactionProjetItem
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public string CategorieNom { get; set; } = string.Empty;
    public decimal Montant { get; set; }
    public bool EstRevenu { get; set; }
    public string MontantSigne => EstRevenu ? $"+{Montant:C2}" : $"-{Montant:C2}";
}