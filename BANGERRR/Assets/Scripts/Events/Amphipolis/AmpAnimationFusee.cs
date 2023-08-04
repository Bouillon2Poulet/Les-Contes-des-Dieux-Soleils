using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpAnimationFusee : MonoBehaviour
{
    private bool hasBeenTriggered = false;
    private Animator Animator;

    private void Start()
    {
        Animator = GetComponent<Animator>();
    }

    public void StartAnimation()
    {
        if (!hasBeenTriggered)
        {
            Animator.SetTrigger("animateFusee");
            hasBeenTriggered = true;
        }
    }

    public void CloseHublot()
    {
        Animator.SetTrigger("fermerHublot");
    }

    public void KillAnimator()
    {
        Animator.enabled = false;
    }

    public static AmpAnimationFusee instance { get; private set; }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
}
