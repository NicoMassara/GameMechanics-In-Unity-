using System;
using UnityEngine;

namespace _Main.Scripts.AstroneerCrafting.FloatingHand
{
    public interface IHandable
    {
        public bool CanBeGrabbed { get; }
        public event Action<IHandable> OnForcedRelease;
        public Transform SelfTransform { get; }
        public void Grab(Transform newParent);
        public void UnGrab();
    }
}