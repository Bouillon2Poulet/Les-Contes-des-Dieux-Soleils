using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LezardSpriteSwitcher : MonoBehaviour
{
    private Animator animator;
    private bool hasDialog = true;
    private GameObject particleSystemObject;

    private void Awake()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        animator.SetBool("Awake", false);

        if (this.GetComponent<NPC>().messagesA.Length == 0)
        {
            hasDialog = false;
        }

        particleSystemObject = GetComponentInChildren<ParticleSystem>().gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasDialog)
        {
            if (other.CompareTag("Player"))
            {
                animator.SetBool("Awake", true);
                particleSystemObject.SetActive(false);
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
                particleSystemObject.SetActive(true);
                //Debug.Log("Lezard goes back to sleep");
            }
        }
    }
}
