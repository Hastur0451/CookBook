using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RecipeManager.BusinessLogic
{
    public class TheMealDBClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://www.themealdb.com/api/json/v1/1/";

        // Properties for recipe details
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        // Returns the name of the recipe for display purposes
        public override string ToString()
        {
            return Name;
        }

        // Initializes HttpClient
        public TheMealDBClient()
        {
            _httpClient = new HttpClient();
        }

        // Searches for meals by a given term and returns a list of search results
        public async Task<List<RecipeSearchResult>> SearchMeals(string searchTerm)
        {
            var response = await _httpClient.GetStringAsync($"{BaseUrl}search.php?s={searchTerm}");
            var jsonDoc = JsonDocument.Parse(response);
            var meals = jsonDoc.RootElement.GetProperty("meals");

            var results = new List<RecipeSearchResult>();
            if (meals.ValueKind != JsonValueKind.Null)
            {
                foreach (var meal in meals.EnumerateArray())
                {
                    results.Add(new RecipeSearchResult
                    {
                        Id = meal.GetProperty("idMeal").GetString(),
                        Name = meal.GetProperty("strMeal").GetString(),
                        Image = meal.GetProperty("strMealThumb").GetString()
                    });
                }
            }
            return results;
        }

        // Retrieves detailed information about a meal by its ID
        public async Task<Recipe> GetRecipeById(string id)
        {
            var response = await _httpClient.GetStringAsync($"{BaseUrl}lookup.php?i={id}");
            var jsonDoc = JsonDocument.Parse(response);
            var meal = jsonDoc.RootElement.GetProperty("meals")[0];

            var recipe = new Recipe
            {
                Id = meal.GetProperty("idMeal").GetString(),
                Name = meal.GetProperty("strMeal").GetString(),
                Category = meal.GetProperty("strCategory").GetString(),
                Area = meal.GetProperty("strArea").GetString(),
                Instructions = meal.GetProperty("strInstructions").GetString(),
                Image = meal.GetProperty("strMealThumb").GetString(),
                Ingredients = new List<string>(),
                Measures = new List<string>()
            };

            // Parses up to 20 ingredients and measures
            for (int i = 1; i <= 20; i++)
            {
                var ingredient = meal.GetProperty($"strIngredient{i}").GetString();
                var measure = meal.GetProperty($"strMeasure{i}").GetString();

                if (!string.IsNullOrWhiteSpace(ingredient))
                {
                    recipe.Ingredients.Add(ingredient);
                    recipe.Measures.Add(measure);
                }
            }

            return recipe;
        }
    }

    // Model for a recipe search result
    public class RecipeSearchResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }

    // Model for detailed recipe information
    public class Recipe
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Area { get; set; }
        public string Instructions { get; set; }
        public string Image { get; set; }
        public List<string> Ingredients { get; set; }
        public List<string> Measures { get; set; }
    }
}
