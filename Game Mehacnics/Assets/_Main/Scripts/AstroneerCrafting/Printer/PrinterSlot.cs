using _Main.Scripts.AstroneerCrafting.Materials;

namespace _Main.Scripts.AstroneerCrafting.Printer
{
    public class PrinterSlot : MaterialSlot
    {
        public void RemoveMaterial()
        {
            if(!HasMaterialAttached) return;
            
            MaterialAttached.Reset();
            ResetAttachValues();
        }
    }
}