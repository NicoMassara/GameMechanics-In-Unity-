using UnityEngine;

namespace _Main.Scripts.AstroneerCrafting.Printer
{
    [CreateAssetMenu(fileName = "PrinterData", menuName = "Scriptable Object/Astroneer/Printer", order = 0)]
    public class PrinterDataSo : ScriptableObject
    {
        [Range(1, 15)] [SerializeField] private int timeToPrint;
        [SerializeField] private MaterialEnum[] availableRecipes;

        public MaterialEnum[] AvailableRecipes => availableRecipes;
        public int TimeToPrint => timeToPrint;
    }
}