using Budget.Mobile.Models;
using System.Net.Http.Json;

namespace Budget.Mobile.Services
{  
    public class DepenseService
    {
        public readonly HttpClient _httpClient;

        public DepenseService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://budget-api-1072325117496.northamerica-northeast1.run.app/");
        }

        public async Task<List<Depense>> GetDepensesAsync()
        {
            try
            {
                var depenses = await _httpClient.GetFromJsonAsync<List<Depense>>("api/depenses");
                return depenses ?? new List<Depense>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERREUR API : {ex.Message}");
                return new List<Depense>();
            }
        }

        public async Task<Boolean> AddDepenseAsync(Depense depense)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/depenses", depense);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERREUR API : {ex.Message}");
                return false;
            }
        }

        public async Task<Boolean> DeleteDepenseAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/depenses/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERREUR API : {ex.Message}");
                return false;
            }
        }

        public async Task<Boolean> UpdateDepenseAsync(Depense depense)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/depenses/{depense.Id}", depense);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERREUR API : {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<LigneBudget>> GetBudgetSuiviAsync(int annee)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<LigneBudget>>($"api/depenses/budget/suivi/{annee}");
        }

        public async Task<IEnumerable<LigneBudget>> GetBudgetDefinitionAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<LigneBudget>>("api/depenses/budget/definition");
        }

        public async Task SaveObjectifAsync(LigneBudget ligne)
        {
            await _httpClient.PostAsJsonAsync("api/depenses/budget/update", ligne);
        }

        public async Task<IEnumerable<ResumeMois>> GetResumeAnnuelAsync(int annee)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ResumeMois>>($"api/depenses/calendrier/{annee}");

        }

        public async Task<List<Projet>> GetProjetsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Projet>>("api/projets");
                return response ?? new List<Projet>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur GetProjetsAsync: {ex.Message}");
                return new List<Projet>();
            }
        }

        public async Task<ProjetDetailsDto?> GetProjetDetailsAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<ProjetDetailsDto>($"api/projets/{id}/details");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur GetProjetDetailsAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<int?> CreateProjetAsync(CreateProjetDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/projets", dto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<int>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur CreateProjetAsync: {ex.Message}");
            }
            return null;
        }
    }
}
