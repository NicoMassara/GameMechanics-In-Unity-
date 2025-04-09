using _Main.Scripts.AstroneerCrafting.Materials;
using UnityEngine;

namespace _Main.Scripts.AstroneerCrafting
{
    public class Trash : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<MaterialObject>(out var materialObject))
            {
                materialObject.TriggerRelease();
                materialObject.Recycle();
            }
        }
    }
}