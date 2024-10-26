using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace RecipeManager.BusinessLogic
{
    public class FatSecretApiClient
    {
        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        private readonly HttpClient _httpClient;

        public FatSecretApiClient(string consumerKey, string consumerSecret)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _httpClient = new HttpClient();
        }

        public async Task<NutritionInfo> GetFoodNutritionAsync(string query)
        {
            var parameters = new Dictionary<string, string>
            {
                {"method", "foods.search"},
                {"search_expression", query},
                {"format", "json"}
            };

            var response = await MakeApiRequestAsync(parameters);
            var searchResult = JsonSerializer.Deserialize<JsonElement>(response);

            if (searchResult.TryGetProperty("foods", out var foods) &&
                foods.TryGetProperty("food", out var foodArray) &&
                foodArray.GetArrayLength() > 0)
            {
                var firstFood = foodArray[0];
                return ParseNutritionInfo(firstFood);
            }

            return new NutritionInfo();
        }

        private async Task<string> MakeApiRequestAsync(Dictionary<string, string> parameters)
        {
            var apiUrl = "https://platform.fatsecret.com/rest/server.api";
            var oauthParameters = GenerateOAuthParameters();

            foreach (var param in parameters)
            {
                oauthParameters.Add(param.Key, param.Value);
            }

            var signature = GenerateSignature("GET", apiUrl, oauthParameters);
            oauthParameters.Add("oauth_signature", signature);

            var queryString = string.Join("&", oauthParameters.Select(p => $"{UrlEncode(p.Key)}={UrlEncode(p.Value)}"));
            var requestUrl = $"{apiUrl}?{queryString}";

            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private Dictionary<string, string> GenerateOAuthParameters()
        {
            return new Dictionary<string, string>
            {
                {"oauth_consumer_key", _consumerKey},
                {"oauth_signature_method", "HMAC-SHA1"},
                {"oauth_timestamp", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()},
                {"oauth_nonce", Guid.NewGuid().ToString("N")},
                {"oauth_version", "1.0"}
            };
        }

        private string GenerateSignature(string method, string url, Dictionary<string, string> parameters)
        {
            var parameterString = string.Join("&",
                parameters.OrderBy(p => p.Key)
                          .Select(p => $"{UrlEncode(p.Key)}={UrlEncode(p.Value)}"));

            var signatureBaseString = $"{method.ToUpper()}&{UrlEncode(url)}&{UrlEncode(parameterString)}";

            using var hmac = new HMACSHA1(Encoding.ASCII.GetBytes($"{UrlEncode(_consumerSecret)}&"));
            var signatureBytes = hmac.ComputeHash(Encoding.ASCII.GetBytes(signatureBaseString));
            return Convert.ToBase64String(signatureBytes);
        }

        private string UrlEncode(string value)
        {
            return HttpUtility.UrlEncode(value)?.Replace("+", "%20").Replace("%7E", "~") ?? string.Empty;
        }

        private NutritionInfo ParseNutritionInfo(JsonElement food)
        {
            var nutrition = new NutritionInfo();

            if (food.TryGetProperty("food_description", out var descriptionElement))
            {
                var description = descriptionElement.GetString();
                var parts = description.Split('-');

                foreach (var part in parts)
                {
                    var trimmedPart = part.Trim();
                    if (trimmedPart.EndsWith("kcal"))
                    {
                        nutrition.Calories = ParseNutritionValue(trimmedPart);
                    }
                    else if (trimmedPart.EndsWith("g") && trimmedPart.Contains("fat"))
                    {
                        nutrition.Fat = ParseNutritionValue(trimmedPart);
                    }
                    else if (trimmedPart.EndsWith("g") && trimmedPart.Contains("carbs"))
                    {
                        nutrition.Carbohydrates = ParseNutritionValue(trimmedPart);
                    }
                    else if (trimmedPart.EndsWith("g") && trimmedPart.Contains("protein"))
                    {
                        nutrition.Protein = ParseNutritionValue(trimmedPart);
                    }
                }
            }

            return nutrition;
        }

        private double ParseNutritionValue(string text)
        {
            var numberPart = new string(text.TakeWhile(c => char.IsDigit(c) || c == '.').ToArray());
            return double.TryParse(numberPart, out var result) ? result : 0;
        }
    }

    public class NutritionInfo
    {
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbohydrates { get; set; }
        public double Fat { get; set; }
    }
}