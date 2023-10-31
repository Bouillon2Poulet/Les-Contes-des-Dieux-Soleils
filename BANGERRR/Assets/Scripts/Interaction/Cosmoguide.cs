using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cosmoguide : Note, IInteractable
{
    [Header("Bubble stuff")]
    public GameObject bubble;
    private float interactRange;

    public void Interact()
    {
        PlayerStatus P = FindObjectOfType<PlayerStatus>();
        if (!P.hasCosmoGuide)
        {
            P.giveCosmoguide();
            string message = "Vous trouvez un CosmoGuide ! Appuyez sur C pour découvrir l'univers.";
            FindObjectOfType<DialogManager>().OpenMessage(message, "Objet trouvé", "Neutre");
            Destroy(transform.gameObject);
        }
    }

    private void Start()
    {
        interactRange = FindObjectOfType<Interactor>().interactRange;
    }

    private void Update()
    {
        if (CheckPlayer())
        {
            ToggleBubble(true);
        }
        else
        {
            ToggleBubble(false);
        }
    }

    public void ToggleBubble(bool state)
    {
        bubble.SetActive(state);
    }

    private bool CheckPlayer()
    {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
