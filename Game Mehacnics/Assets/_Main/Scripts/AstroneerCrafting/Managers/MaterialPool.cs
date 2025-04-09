using System;
using _Main.Scripts.AstroneerCrafting.Materials;
using _Tools.SingleHandledPool;
using UnityEngine;

namespace _Main.Scripts.AstroneerCrafting.Managers
{
    public class MaterialPool
    {
        private readonly Func<MaterialObject> _factoryMethod;

        private SingleHandlerList<MaterialObject> _materialPool;

        public MaterialPool(Func<MaterialObject> factoryMethod)
        {
            _factoryMethod = factoryMethod;
            
            Initialize();
        }

        private void Initialize()
        {
            _materialPool = new SingleHandlerList<MaterialObject>();
        }

        public MaterialObject GetItem()
        {
            if(_materialPool == null) return null;

            if (_materialPool.IsEmpty || _materialPool.IsFull)
            {
                _materialPool.AddItem(_factoryMethod());
            }
            
            return _materialPool.GetFirstItem();
        }
    }
    
}