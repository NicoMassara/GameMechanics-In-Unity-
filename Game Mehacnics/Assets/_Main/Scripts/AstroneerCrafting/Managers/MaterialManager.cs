using System.Collections.Generic;
using UnityEngine;

namespace _Main.Scripts.AstroneerCrafting.Managers
{
    public class MaterialManager
    {
        private Dictionary<MaterialEnum, MaterialData> _materialData;


        public MaterialManager()
        {
            InitializeMaterialData();
        }


        private void InitializeMaterialData()
        {
            _materialData = new Dictionary<MaterialEnum, MaterialData>();

            MaterialData newMaterialData = null;
            
            //Red
            newMaterialData = new MaterialData(MaterialEnum.Red, Color.red, true);
            _materialData.Add(newMaterialData.SelfType, newMaterialData);
            
            //Green
            newMaterialData = new MaterialData(MaterialEnum.Green, Color.green, true);
            _materialData.Add(newMaterialData.SelfType, newMaterialData);
            
            //Blue
            newMaterialData = new MaterialData(MaterialEnum.Blue, Color.blue, true);
            _materialData.Add(newMaterialData.SelfType, newMaterialData);
            
            //White
            newMaterialData = new MaterialData(MaterialEnum.White, Color.white, true);
            _materialData.Add(newMaterialData.SelfType, newMaterialData);
            
            //Cyan
            newMaterialData = new MaterialData(MaterialEnum.Cyan, Color.cyan, true);
            _materialData.Add(newMaterialData.SelfType, newMaterialData);
            
            //Magenta
            newMaterialData = new MaterialData(MaterialEnum.Magenta, Color.magenta, true);
            _materialData.Add(newMaterialData.SelfType, newMaterialData);
            
            //Yellow
            newMaterialData = new MaterialData(MaterialEnum.Yellow, Color.yellow, true);
            _materialData.Add(newMaterialData.SelfType, newMaterialData);
            
            //Black
            newMaterialData = new MaterialData(MaterialEnum.Black, Color.black, true);
            _materialData.Add(newMaterialData.SelfType, newMaterialData);
            
            //LightRed
            newMaterialData = new MaterialData(MaterialEnum.LightRed, new Color(1f,.35f,.35f), true);
            _materialData.Add(newMaterialData.SelfType, newMaterialData);
            
            //LightBlue
            newMaterialData = new MaterialData(MaterialEnum.LightBlue, new Color(.35f,.35f,1), true);
            _materialData.Add(newMaterialData.SelfType, newMaterialData);
            
            //LightGreen
            newMaterialData = new MaterialData(MaterialEnum.LightGreen, new Color(.35f,1,.35f) , true);
            _materialData.Add(newMaterialData.SelfType, newMaterialData);
            
            
            /*//Template
            newMaterialData = new MaterialData(MaterialEnum.LightBlue, Color.red, true);
            _materialData.Add(newMaterialData.SelfType, newMaterialData);*/
        }

        public MaterialData GetMaterialDataByEnum(MaterialEnum materialEnum)
        {
            return _materialData.ContainsKey(materialEnum) ? _materialData[materialEnum] : null;
        }
    }

    public class MaterialData
    {
        public MaterialEnum SelfType { get; private set; }
        public bool CanBeUsedToCraft { get; private set; }
        public Color SelfColor { get; private set; }

        public MaterialData(MaterialEnum selfType, Color selfColor, bool canBeUsedToCraft = true)
        {
            SelfType = selfType;
            SelfColor = selfColor;
            CanBeUsedToCraft = canBeUsedToCraft;
        }
    }


}