using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LezardSpriteSwitcher : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        animator.SetBool("Awake", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("Awake", true);
            //Debug.Log("Awaking Lezard");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("Awake", false);
            //Debug.Log("Lezard goes back to sleep");
        }
    }
}
