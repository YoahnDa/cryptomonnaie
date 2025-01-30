using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Backend_Crypto.Services
{
    public class ExternalApiService
    {
        private readonly HttpClient _httpClient;

        // Injecter HttpClient via le constructeur
        public ExternalApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Exemple : Récupérer des données depuis une API externe
        public async Task<JObject>? GetDataFromApiAsync(string endpoint,string token = "")
        {
            try
            {
                // Ajouter le token à l'en-tête Authorization
                if(!token.Trim().Equals(""))
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    // Si la réponse est OK (status 2xx), lire les données
                    var jsonResponse = await response.Content.ReadAsStringAsync();  

                    if (string.IsNullOrEmpty(jsonResponse))
                    {
                        return JObject.Parse(jsonResponse);
                    }
                    return null;
                }
                else
                {
                    // Gérer les erreurs et récupérer le message d'erreur de l'API
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    throw new ApiException(errorResponse, (int)response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                // Gérer les exceptions réseau
                Console.WriteLine($"Erreur réseau : {ex.Message}");
                throw new ApiException("Une erreur réseau est survenue.", 500, ex);
            }
        }

        // Exemple : Poster des données vers une API externe
        public async Task<JObject>? PostDataToApiAsync<T>(string endpoint, T data)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(endpoint, data);

                if (response.IsSuccessStatusCode)
                {
                    // Si la requête est réussie, retourner true
                    // Si la réponse est OK (status 2xx), lire les données
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(jsonResponse))
                    {
                        return JObject.Parse(jsonResponse);
                    }
                    return null; 
                }
                else
                {
                    // Gérer les erreurs et récupérer le message d'erreur de l'API
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    throw new ApiException($"Erreur lors de l'appel API : {response.StatusCode} - {errorResponse}", (int)response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                // Gérer les exceptions réseau
                Console.WriteLine($"Erreur réseau : {ex.Message}");
                throw new ApiException("Une erreur réseau est survenue.", 500, ex);
            }
        }
    }

    public class ApiException : Exception
    {
        // Code d'erreur HTTP
        public int StatusCode { get; }

        // Constructeur avec message et code d'erreur HTTP
        public ApiException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        // Constructeur avec message, code d'erreur HTTP et exception interne
        public ApiException(string message, int statusCode, Exception innerException) : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }

}
