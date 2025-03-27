using UnityEngine;

namespace _Main.Scripts.AstroneerCrafting.Printer
{
    public class SimplePrinter : PrinterModel
    {
        protected override void SetRecipeList()
        {
            AvailableRecipes = new[]
            {
                RecipeEnum.LightRed,
                RecipeEnum.LightBlue,
                RecipeEnum.LightGreen
            };
        }

        protected override void Update()
        {
            base.Update();
            
        }
    }
}