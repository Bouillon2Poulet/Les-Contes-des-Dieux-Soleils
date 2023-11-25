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
            AudioManager.instance.Play("pickup");
            M.Isador_PuzzlePieceFound = true;
            M.updateNPCPages();

            string frString = "La voilà, la pièce manquante ! Elle est minuscule...";
            string engString = "There it is, the missing piece! It's tiny...";

            DialogManager.instance.OpenMessage(frString, engString, "Puzzle", "Solisede");
            
            GetComponent<InteractionBubble>().TurnOff();
            
            gameObject.SetActive(false);
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
