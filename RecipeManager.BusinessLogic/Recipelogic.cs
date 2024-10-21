using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeManager.BusinessLogic
{
    public class RecipeLogic
    {
        private readonly TheMealDBClient _mealDbClient;
        private readonly FatSecretApiClient _fatSecretClient;

        public RecipeLogic(string fatSecretConsumerKey, string fatSecretConsumerSecret)
        {
            _mealDbClient = new TheMealDBClient();
            _fatSecretClient = new FatSecretApiClient(fatSecretConsumerKey, fatSecretConsumerSecret);
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

        public async Task<(List<IngredientNutrition> IngredientsNutrition, NutritionInfo TotalNutrition)> CalculateRecipeNutrition(Recipe recipe)
        {
            var ingredientsNutrition = new List<IngredientNutrition>();
            var totalNutrition = new NutritionInfo();

            foreach (var (measure, ingredient) in GetFormattedIngredients(recipe))
            {
                var query = $"{measure} {ingredient}";
                var nutrition = await _fatSecretClient.GetFoodNutritionAsync(query);

                ingredientsNutrition.Add(new IngredientNutrition
                {
                    Ingredient = $"{measure} {ingredient}",
                    Nutrition = nutrition
                });

                totalNutrition.Calories += nutrition.Calories;
                totalNutrition.Protein += nutrition.Protein;
                totalNutrition.Carbohydrates += nutrition.Carbohydrates;
                totalNutrition.Fat += nutrition.Fat;
            }

            return (ingredientsNutrition, totalNutrition);
        }
    }

    public class IngredientNutrition
    {
        public string Ingredient { get; set; }
        public NutritionInfo Nutrition { get; set; }
    }
}