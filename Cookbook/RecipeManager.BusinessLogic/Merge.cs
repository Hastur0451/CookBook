using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CookBook.RecipeManager.GUI.Models;

namespace RecipeManager.BusinessLogic
{
    public class ShoppingListMerger
    {
        // Pattern to match quantity, unit, and name in ingredient strings
        private static readonly Regex MeasurementPattern = new(
            @"^(?<quantity>\d+)\s*(?<unit>g|kg|ml|l|gram|grams|kilo|kilos|kilogram|kilograms|milliliter|milliliters|liter|liters|oz|ounce|ounces|lb|lbs|pound|pounds)\s+(?<name>.+)$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // Merges duplicate items in the shopping list by summing quantities with matching units
        public List<ShoppingListItem> MergeItems(List<ShoppingListItem> items)
        {
            if (items == null || items.Count <= 1)
                return items ?? new List<ShoppingListItem>();

            var result = new List<ShoppingListItem>();
            var measurementItems = new Dictionary<string, (decimal totalQuantity, string unit, string name)>();

            foreach (var item in items)
            {
                var match = MeasurementPattern.Match(item.Name);
                if (match.Success)
                {
                    // Parse items with measurements
                    if (decimal.TryParse(match.Groups["quantity"].Value, out decimal quantity))
                    {
                        var unit = NormalizeUnitName(match.Groups["unit"].Value);
                        var name = match.Groups["name"].Value.Trim();

                        // Normalize unit and quantity
                        quantity = NormalizeQuantity(quantity, unit, out string normalizedUnit);
                        var key = name.ToLower();

                        if (measurementItems.ContainsKey(key))
                        {
                            var existing = measurementItems[key];
                            measurementItems[key] = (existing.totalQuantity + quantity, normalizedUnit, name);
                        }
                        else
                        {
                            measurementItems.Add(key, (quantity, normalizedUnit, name));
                        }
                    }
                    else
                    {
                        result.Add(item); // Add item if parsing fails
                    }
                }
                else
                {
                    result.Add(item); // Add item without measurements
                }
            }

            // Add merged items to result list
            foreach (var measurementItem in measurementItems)
            {
                var (totalQuantity, unit, name) = measurementItem.Value;

                // Format quantity to remove unnecessary decimals
                string formattedQuantity = totalQuantity % 1 == 0
                    ? ((int)totalQuantity).ToString()
                    : totalQuantity.ToString("0.##");

                result.Add(new ShoppingListItem
                {
                    Name = $"{formattedQuantity}{unit} {name}",
                    IsSelected = true
                });
            }

            return result.OrderBy(x => x.Name).ToList();
        }

        // Normalizes units to a standard form
        private string NormalizeUnitName(string unit)
        {
            switch (unit.ToLower())
            {
                case "gram":
                case "grams":
                    return "g";

                case "kilo":
                case "kilos":
                case "kilogram":
                case "kilograms":
                case "kg":
                    return "kg";

                case "milliliter":
                case "milliliters":
                case "ml":
                    return "ml";

                case "liter":
                case "liters":
                case "l":
                    return "l";

                case "ounce":
                case "ounces":
                case "oz":
                    return "oz";

                case "pound":
                case "pounds":
                case "lb":
                case "lbs":
                    return "lb";

                default:
                    return unit.ToLower();
            }
        }

        // Converts quantities based on the unit (e.g., kg to g)
        private decimal NormalizeQuantity(decimal quantity, string unit, out string normalizedUnit)
        {
            switch (unit)
            {
                case "kg":
                    normalizedUnit = "g";
                    return quantity * 1000;

                case "l":
                    normalizedUnit = "ml";
                    return quantity * 1000;

                case "oz":
                    normalizedUnit = "g";
                    return quantity * 28.35m; // 1 ounce ≈ 28.35 grams

                case "lb":
                    normalizedUnit = "g";
                    return quantity * 453.592m; // 1 pound ≈ 453.592 grams

                default:
                    normalizedUnit = unit;
                    return quantity;
            }
        }
    }
}
