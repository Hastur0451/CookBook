using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CookBook.RecipeManager.GUI.Models;

namespace RecipeManager.BusinessLogic
{
    public class ShoppingListMerger
    {
        // 扩展匹配模式以支持更多单位
        private static readonly Regex MeasurementPattern = new(
            @"^(?<quantity>\d+)\s*(?<unit>g|kg|ml|l|gram|grams|kilo|kilos|kilogram|kilograms|milliliter|milliliters|liter|liters|oz|ounce|ounces|lb|lbs|pound|pounds)\s+(?<name>.+)$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

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
                    // 解析带计量单位的项目
                    if (decimal.TryParse(match.Groups["quantity"].Value, out decimal quantity))
                    {
                        var unit = NormalizeUnitName(match.Groups["unit"].Value);
                        var name = match.Groups["name"].Value.Trim();

                        // 标准化单位和数量
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
                        result.Add(item);
                    }
                }
                else
                {
                    result.Add(item);
                }
            }

            // 添加合并后的项目
            foreach (var measurementItem in measurementItems)
            {
                var (totalQuantity, unit, name) = measurementItem.Value;

                // 格式化数量以移除不必要的小数点
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

        private string NormalizeUnitName(string unit)
        {
            // 单位名称标准化映射
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
                    return quantity * 28.35m; // 1盎司 ≈ 28.35克

                case "lb":
                    normalizedUnit = "g";
                    return quantity * 453.592m; // 1磅 ≈ 453.592克

                default:
                    normalizedUnit = unit;
                    return quantity;
            }
        }
    }
}
