using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
    Transform GetTransform();
}

public class Interactor : MonoBehaviour
{
    public KeyCode InteractionKey = KeyCode.E;

    void Update()
    {
        if (Input.GetKeyDown(InteractionKey))
        {
            List<IInteractable> interactables = new List<IInteractable>();
            float interactRange = 1f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out IInteractable interactObj))
                {
                    interactables.Add(interactObj);
                }
            }

            IInteractable closestInteractable = null;
            foreach(IInteractable interactable in interactables)
            {
                if (closestInteractable == null)
                {
                    closestInteractable = interactable;
                }
                else
                {
                    if (Vector3.Distance(transform.position, interactable.GetTransform().position) < Vector3.Distance(transform.position, closestInteractable.GetTransform().position))
                    {
                        closestInteractable = interactable;
                    }
                }
            }
            if (closestInteractable != null)
            {
                closestInteractable.Interact();
            }
        }
    }
}
