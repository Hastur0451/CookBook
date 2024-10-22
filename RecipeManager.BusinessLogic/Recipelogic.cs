using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeManager.BusinessLogic
{
    public class RecipeLogic
    {
        private readonly TheMealDBClient _mealDbClient;
        // private readonly FatSecretApiClient _fatSecretClient;

        public RecipeLogic(string fatSecretConsumerKey, string fatSecretConsumerSecret)
        {
            _mealDbClient = new TheMealDBClient();
            // _fatSecretClient = new FatSecretApiClient(fatSecretConsumerKey, fatSecretConsumerSecret);
        }

        public async Task<List<RecipeSearchResult>> SearchRecipes(string searchTerm)
        {
            return await _mealDbClient.SearchMeals(searchTerm);
        }

        public async Task<Recipe> GetRecipeDetails(string recipeId)
        {
            return await _mealDbClient.GetRecipeById(recipeId);
        }

        public List<(string Measure, string Ingredient)> GetFormattedIngredients(Recipe recipe)
        {
            var formattedIngredients = new List<(string Measure, string Ingredient)>();

            for (int i = 0; i < recipe.Ingredients.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(recipe.Ingredients[i]))
                {
                    formattedIngredients.Add((recipe.Measures[i].Trim(), recipe.Ingredients[i].Trim()));
                }
            }

            return formattedIngredients;
        }

        /*public async Task<(List<IngredientNutrition> IngredientsNutrition, NutritionInfo TotalNutrition)> CalculateRecipeNutrition(Recipe recipe)
        {
            var ingredientsNutrition = new List<IngredientNutrition>();
            var totalNutrition = new NutritionInfo();
            var random = new Random();

            foreach (var (measure, ingredient) in GetFormattedIngredients(recipe))
            {
                // 使用mock数据替代API调用
                var mockNutrition = new NutritionInfo
                {
                    Calories = random.Next(50, 300),
                    Protein = random.Next(1, 20),
                    Carbohydrates = random.Next(5, 50),
                    Fat = random.Next(1, 15)
                };

                ingredientsNutrition.Add(new IngredientNutrition
                {
                    Ingredient = $"{measure} {ingredient}",
                    Nutrition = mockNutrition
                });

                totalNutrition.Calories += mockNutrition.Calories;
                totalNutrition.Protein += mockNutrition.Protein;
                totalNutrition.Carbohydrates += mockNutrition.Carbohydrates;
                totalNutrition.Fat += mockNutrition.Fat;
            }

            // 模拟API调用的延迟
            await Task.Delay(1000);

            return (ingredientsNutrition, totalNutrition);
        }*/
    }

    public class IngredientNutrition
    {
        public string Ingredient { get; set; }
        public NutritionInfo Nutrition { get; set; }
    }

    /*public class NutritionInfo
    {
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbohydrates { get; set; }
        public double Fat { get; set; }
    }*/
}