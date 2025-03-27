using System;
using UnityEngine;

namespace _Main.Scripts.AstroneerCrafting
{
    public interface IInteractable
    {
        public Transform SelfTransform { get;}

        public event Action<bool> OnInteract; //bool - false [Interaction End] / bool - true [Interaction Start] 
        
        public void Interact();
    }
}