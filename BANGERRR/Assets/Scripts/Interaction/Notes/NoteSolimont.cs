using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSolimont : Note, IInteractable
{
    public NPC Flegmardo;
    public GameObject murInvisible;

    private bool beenSung;

    public SpriteRenderer noteSpriteRenderer;

    [Header("Eleven Moving")]
    [SerializeField] Rigidbody Eleven;
    [SerializeField] Transform newPos;

    public void Interact()
    {
        if (!beenSung)
        {
            if (Flegmardo.isPageDRead)
            {
                // Fonction Chanter()
                AudioManager.instance.Play("chant");
                FindAnyObjectByType<NPCEventsManager>().Soli_songSung = true;
                beenSung = true;
                murInvisible.SetActive(false);
                ///DialogManager.instance.OpenMessage("*Chanson*", "DEBUG", "Solimont");
                GetComponent<InteractionBubble>().TurnOff();

                StartCoroutine(nameof(MoveEleven));
            }
            else
            {
                DialogManager.instance.OpenMessage("Ne touche pas à ça !", "Leave that alone!", "Flegmardo", "Solimont");
            }
        }
    }

    IEnumerator MoveEleven()
    {
        FindAnyObjectByType<ThirdPersonMovement>().blockPlayerMoveInputs();
        FindAnyObjectByType<MainCameraManager>().blockMovement();

        yield return new WaitForSeconds(5);

        yield return FadeToBlack.instance.Fade(true, 1);

        Eleven.position = newPos.position;
        Eleven.rotation = newPos.rotation;

        noteSpriteRenderer.enabled = false;

        yield return FadeToBlack.instance.Fade(false, 1);

        FindAnyObjectByType<ThirdPersonMovement>().unblockPlayerMoveInputs();
        FindAnyObjectByType<MainCameraManager>().unblockMovement();

        gameObject.SetActive(false);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
