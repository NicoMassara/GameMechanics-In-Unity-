using UnityEngine;

namespace _Main.Scripts.AstroneerCrafting.Printer
{
    public class DualPrinter : PrinterModel
    {
        protected override void SetRecipeList()
        {
            AvailableRecipes = new[]
            {
                RecipeEnum.Cyan,
                RecipeEnum.Magenta,
                RecipeEnum.Yellow
            };
        }
    }
}