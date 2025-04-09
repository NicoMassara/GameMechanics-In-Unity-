using System.Collections.Generic;

namespace _Main.Scripts.AstroneerCrafting.Managers
{
    public class RecipeManager
    {
        private Dictionary<MaterialEnum, RecipeData> _recipes;

        public RecipeManager()
        {
            InitializeRecipeData();
        }

        private void InitializeRecipeData()
        {
            _recipes = new Dictionary<MaterialEnum, RecipeData>();
            MaterialEnum[] materialsNeeded = null;
            RecipeData newRecipeData = null;

            //Red
            materialsNeeded = new[] { MaterialEnum.None };
            newRecipeData = new RecipeData(MaterialEnum.Red, materialsNeeded);
            _recipes.Add(newRecipeData.MaterialGotten, newRecipeData);
            
            //Blue
            materialsNeeded = new[] { MaterialEnum.None };
            newRecipeData = new RecipeData(MaterialEnum.Blue, materialsNeeded);
            _recipes.Add(newRecipeData.MaterialGotten, newRecipeData);
            
            //Green
            materialsNeeded = new[] { MaterialEnum.None };
            newRecipeData = new RecipeData(MaterialEnum.Green, materialsNeeded);
            _recipes.Add(newRecipeData.MaterialGotten, newRecipeData);
            
            //Cyan
            materialsNeeded = new[] { MaterialEnum.Green, MaterialEnum.Blue };
            newRecipeData = new RecipeData(MaterialEnum.Cyan, materialsNeeded);
            _recipes.Add(newRecipeData.MaterialGotten, newRecipeData);
            
            //Magenta
            materialsNeeded = new[] { MaterialEnum.Red, MaterialEnum.Blue };
            newRecipeData = new RecipeData(MaterialEnum.Magenta, materialsNeeded);
            _recipes.Add(newRecipeData.MaterialGotten, newRecipeData);
            
            //Yellow
            materialsNeeded = new[] { MaterialEnum.Green, MaterialEnum.Red };
            newRecipeData = new RecipeData(MaterialEnum.Yellow, materialsNeeded);
            _recipes.Add(newRecipeData.MaterialGotten, newRecipeData);
            
            //White
            materialsNeeded = new[] { MaterialEnum.Green, MaterialEnum.Blue, MaterialEnum.Red };
            newRecipeData = new RecipeData(MaterialEnum.White, materialsNeeded);
            _recipes.Add(newRecipeData.MaterialGotten, newRecipeData);
            
            //Black
            materialsNeeded = new[] { MaterialEnum.Green, MaterialEnum.Blue, MaterialEnum.Red, MaterialEnum.White };
            newRecipeData = new RecipeData(MaterialEnum.Black, materialsNeeded);
            _recipes.Add(newRecipeData.MaterialGotten, newRecipeData);
            
            //LightRed
            materialsNeeded = new[] { MaterialEnum.Red};
            newRecipeData = new RecipeData(MaterialEnum.LightRed, materialsNeeded);
            _recipes.Add(newRecipeData.MaterialGotten, newRecipeData);
            
            //LightGreen
            materialsNeeded = new[] { MaterialEnum.Green};
            newRecipeData = new RecipeData(MaterialEnum.LightGreen, materialsNeeded);
            _recipes.Add(newRecipeData.MaterialGotten, newRecipeData);
            
            //LightBlue
            materialsNeeded = new[] { MaterialEnum.Blue};
            newRecipeData = new RecipeData(MaterialEnum.LightBlue, materialsNeeded);
            _recipes.Add(newRecipeData.MaterialGotten, newRecipeData);
            
            //Template
            /*materialsNeeded = new[] { };
            newRecipeData = new RecipeData(MaterialEnum.Red, materialsNeeded);
            _recipes.Add(newRecipeData.MaterialGotten, newRecipeData);*/

        }
        
        
        public RecipeData GetRecipeDataByEnum(MaterialEnum recipeEnum)
        {
            return _recipes.ContainsKey(recipeEnum) ? _recipes[recipeEnum] : null;
        }
    }

    public class RecipeData
    {
        public MaterialEnum[] MaterialNeeded { get; private set;}
        public MaterialEnum MaterialGotten { get; private set; }
        
        public int MaterialNeededCount => MaterialNeeded.Length;

        public RecipeData(MaterialEnum materialGotten, MaterialEnum[] materialNeeded)
        {
            MaterialNeeded = materialNeeded;
            MaterialGotten = materialGotten;
        }
    }
}