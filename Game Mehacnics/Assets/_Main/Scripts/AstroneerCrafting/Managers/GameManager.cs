using System;
using _Main.Scripts.AstroneerCrafting.Materials;
using UnityEngine;

namespace _Main.Scripts.AstroneerCrafting.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public MaterialManager MaterialManager { get; private set; }
        public RecipeManager RecipeManager { get; private set; }
        
        [SerializeField] private MaterialObject materialPrefab;

        private void Awake()
        {
            MakeSingleton();
            
            MaterialManager = new MaterialManager();
            RecipeManager = new RecipeManager();
        }

        private void MakeSingleton()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public MaterialObject CreateMaterialObject()
        {
            return Instantiate(materialPrefab);
        }
    }
}