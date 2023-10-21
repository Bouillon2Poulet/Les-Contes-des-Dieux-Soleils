using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LezardSpriteSwitcher : MonoBehaviour
{
    private Animator animator;
    private bool hasDialog = true;

    private void Awake()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        animator.SetBool("Awake", false);

        // ne marche pas encore
        if (this.GetComponent<NPC>().messagesA.Length == 0)
        {
            hasDialog = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasDialog)
        {
            if (other.CompareTag("Player"))
            {
                animator.SetBool("Awake", true);
                    //Debug.Log("Awaking Lezard");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (hasDialog)
        {
            if (other.CompareTag("Player"))
            {
                animator.SetBool("Awake", false);
                //Debug.Log("Lezard goes back to sleep");
            }
        }
    }
}
