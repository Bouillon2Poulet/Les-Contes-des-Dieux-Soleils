using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolPuzzlePiece : Note, IInteractable
{
    public void Interact()
    {
        NPCEventsManager M = FindObjectOfType<NPCEventsManager>();
        if (!M.Isador_PuzzlePieceFound)
        {
            M.Isador_PuzzlePieceFound = true;
            M.updateNPCPages();
            string message = "Vous avez trouvé une pièce de puzzle ! Elle est minuscule !";
            FindObjectOfType<DialogManager>().OpenMessage(message, "Objet trouvé", "Solisede");
            transform.gameObject.SetActive(false);
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
