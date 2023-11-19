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
        AudioManager.instance.Play("closehublot");
    }

    public void KillAnimator()
    {
        Animator.enabled = false;
    }

    public void PinceSound()
    {
        AudioManager.instance.Play("pince");
    }

    public void DescenteSound()
    {
        AudioManager.instance.Play("hydraulicdown");
    }

    public void OpenHublot()
    {
        AudioManager.instance.Play("openhublot");
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
