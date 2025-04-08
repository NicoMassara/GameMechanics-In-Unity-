using System;
using System.Collections.Generic;
using _Main.Scripts.AstroneerCrafting.Materials;
using UnityEngine;

namespace _Main.Scripts.AstroneerCrafting.Managers
{
    public class MaterialManager
    {
        private readonly MaterialDataManager _data;
        private readonly MaterialPool _pool;

        public MaterialManager(Func<MaterialObject> factoryMethod)
        {
            _pool = new MaterialPool(factoryMethod);
            _data = new MaterialDataManager();
        }

        public MaterialObject GetItem(MaterialEnum materialEnum)
        {
            var item = _pool.GetItem();
            var color = _data.GetMaterialDataByEnum(materialEnum).SelfColor;
            item.UpdateMaterialColor(color);
            return item;
        }

        public MaterialData GetMaterialDataByEnum(MaterialEnum materialEnum)
        {
            return _data.GetMaterialDataByEnum(materialEnum);
        }
    }



}