using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolPuzzlePiece : Note, IInteractable
{
    public void Interact()
    {
        NPCEventsManager M = FindAnyObjectByType<NPCEventsManager>();
        if (!M.Isador_PuzzlePieceFound)
        {
            M.Isador_PuzzlePieceFound = true;
            M.updateNPCPages();
            string message = "La voil� ! Elle est minuscule�";
            DialogManager.instance.OpenMessage(message, "Pi�ce de puzzle", "Solisede");
            GetComponent<InteractionBubble>().TurnOff();
            transform.gameObject.SetActive(false);
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
