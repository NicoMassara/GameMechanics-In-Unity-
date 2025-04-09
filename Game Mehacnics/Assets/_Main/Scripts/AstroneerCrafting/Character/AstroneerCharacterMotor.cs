using _Main.Scripts.AstroneerCrafting.FloatingHand;
using _Main.Scripts.Locomotion;
using UnityEngine;
using UnityEngine.Events;

namespace _Main.Scripts.AstroneerCrafting.Character
{
    [RequireComponent(typeof(CharacterHand))]
    public class AstroneerCharacterMotor : LocomotionMotor
    {
        [SerializeField] private InteractionBehaviour interactionBehaviour;
        
        private CharacterHand _hand;
        
        public UnityAction<bool> OnInteract;
        public UnityAction<bool> OnHandEnable;

        protected override void Awake()
        {
            base.Awake();

            _hand = GetComponent<CharacterHand>();

            _hand.OnHandEnable += OnHandEnable;
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

        public void CalculateMouseInWorld(Vector2 mousePosition)
        {
            _hand.CalculateMouseInWorld(mousePosition);
        }

        public void ToggleHand()
        {
            _hand.ToggleHand();
        }

        public void GrabHand()
        {
            _hand.TryGrab();
        }

        public void MoveHand(float input)
        {
            _hand.MoveHand(input);
        }
    }
}