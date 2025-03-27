using System.Collections.Generic;

namespace _Main.Scripts.AstroneerCrafting.Managers
{
    public class RecipeManager
    {
        private Dictionary<RecipeEnum, RecipeData> _recipes;

        public RecipeManager()
        {
            InitializeRecipeData();
        }

        private void InitializeRecipeData()
        {
            _recipes = new Dictionary<RecipeEnum, RecipeData>();
            MaterialEnum[] materialsNeeded = null;
            RecipeData newRecipeData = null;

            //Cyan
            materialsNeeded = new[] { MaterialEnum.Green, MaterialEnum.Blue };
            newRecipeData = new RecipeData(RecipeEnum.Cyan, materialsNeeded, MaterialEnum.Cyan);
            _recipes.Add(newRecipeData.SelfType, newRecipeData);
            
            //Magenta
            materialsNeeded = new[] { MaterialEnum.Red, MaterialEnum.Blue };
            newRecipeData = new RecipeData(RecipeEnum.Magenta, materialsNeeded, MaterialEnum.Magenta);
            _recipes.Add(newRecipeData.SelfType, newRecipeData);
            
            //Yellow
            materialsNeeded = new[] { MaterialEnum.Green, MaterialEnum.Red };
            newRecipeData = new RecipeData(RecipeEnum.Yellow, materialsNeeded, MaterialEnum.Yellow);
            _recipes.Add(newRecipeData.SelfType, newRecipeData);
            
            //White
            materialsNeeded = new[] { MaterialEnum.Green, MaterialEnum.Blue, MaterialEnum.Red };
            newRecipeData = new RecipeData(RecipeEnum.White, materialsNeeded, MaterialEnum.White);
            _recipes.Add(newRecipeData.SelfType, newRecipeData);
            
            //Black
            materialsNeeded = new[] { MaterialEnum.Green, MaterialEnum.Blue, MaterialEnum.Red, MaterialEnum.White };
            newRecipeData = new RecipeData(RecipeEnum.Black, materialsNeeded, MaterialEnum.Black);
            _recipes.Add(newRecipeData.SelfType, newRecipeData);
            
            //LightRed
            materialsNeeded = new[] { MaterialEnum.Red};
            newRecipeData = new RecipeData(RecipeEnum.LightRed, materialsNeeded, MaterialEnum.LightRed);
            _recipes.Add(newRecipeData.SelfType, newRecipeData);
            
            //LightGreen
            materialsNeeded = new[] { MaterialEnum.Green};
            newRecipeData = new RecipeData(RecipeEnum.LightGreen, materialsNeeded, MaterialEnum.LightGreen);
            _recipes.Add(newRecipeData.SelfType, newRecipeData);
            
            //LightBlue
            materialsNeeded = new[] { MaterialEnum.Blue};
            newRecipeData = new RecipeData(RecipeEnum.LightBlue, materialsNeeded, MaterialEnum.LightBlue);
            _recipes.Add(newRecipeData.SelfType, newRecipeData);
            
            //Template
            /*materialsNeeded = new[] { };
            newRecipeData = new RecipeData(RecipeEnum.Cyan, materialsNeeded, MaterialEnum.Red);
            _recipes.Add(newRecipeData.SelfType, newRecipeData);*/

        }
        
        
        public RecipeData GetRecipeDataByEnum(RecipeEnum recipeEnum)
        {
            return _recipes.ContainsKey(recipeEnum) ? _recipes[recipeEnum] : null;
        }
    }

    public class RecipeData
    {
        public RecipeEnum SelfType { get; private set; }
        public MaterialEnum[] MaterialNeeded { get; private set;}
        public MaterialEnum MaterialGotten { get; private set; }
        
        public int MaterialNeededCount => MaterialNeeded.Length;

        public RecipeData(RecipeEnum selfType, MaterialEnum[] materialNeeded, MaterialEnum materialGotten)
        {
            SelfType = selfType;
            MaterialNeeded = materialNeeded;
            MaterialGotten = materialGotten;
        }
    }
}