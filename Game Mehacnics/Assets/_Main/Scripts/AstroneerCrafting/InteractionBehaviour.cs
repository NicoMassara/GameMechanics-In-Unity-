using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Main.Scripts.AstroneerCrafting
{
    public class InteractionBehaviour : MonoBehaviour
    {
        private bool _hasInteractableInRange = false;
        private IInteractable _interactableInRange = null;
        
        public UnityAction<bool> OnInteract;

        public void StartInteraction()
        {
            if(!_hasInteractableInRange) return;
            
            _interactableInRange.Interact();
            
            if (_interactableInRange.IsQuick == false)
            {
                Debug.Log("Quick Interaction Performed");
                return;
            }

            _interactableInRange.OnInteract += Interactable_OnInteractHandler;
            OnInteract?.Invoke(true);
            
            Debug.Log("Interaction Start");
        }

        public void StopInteraction()
        {
            _interactableInRange.OnInteract -= Interactable_OnInteractHandler;
            OnInteract?.Invoke(false);
            
            Debug.Log("Interaction End");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out IInteractable interactable))
            {
                _hasInteractableInRange = true;
                _interactableInRange = interactable;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out IInteractable interactable))
            {
                _hasInteractableInRange = false;
                _interactableInRange = null;
            }
        }
        
        private void Interactable_OnInteractHandler(bool hasStarted)
        {
            if (hasStarted == false)
            {
                StopInteraction();
            }
        }
    }
}