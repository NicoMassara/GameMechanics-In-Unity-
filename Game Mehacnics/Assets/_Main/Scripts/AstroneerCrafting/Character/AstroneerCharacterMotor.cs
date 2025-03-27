using _Main.Scripts.Locomotion;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace _Main.Scripts.AstroneerCrafting.Character
{
    public class AstroneerCharacterMotor : LocomotionMotor
    {
        [SerializeField] private InteractionBehaviour interactionBehaviour;
        
        public UnityAction<bool> OnInteract;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            
            interactionBehaviour.OnInteract += OnInteract;
        }

        public void StartInteraction()
        {
            interactionBehaviour?.StartInteraction();
        }
    }
}