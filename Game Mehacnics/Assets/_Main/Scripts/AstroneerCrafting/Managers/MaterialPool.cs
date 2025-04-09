using System;
using _Main.Scripts.AstroneerCrafting.Materials;
using _Main.Custom.Pool;

namespace _Main.Scripts.AstroneerCrafting.Managers
{
    public class MaterialPool
    {
        private GenericPool<MaterialObject> _pool;
        
        public MaterialPool(Func<MaterialObject> factoryMethod)
        {
            _pool = new GenericPool<MaterialObject>(factoryMethod);
        }

        public MaterialObject GetItem()
        {
            return _pool.GetPoolable();
        }
    }
}