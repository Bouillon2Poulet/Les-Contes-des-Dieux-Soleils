using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDPorte : MonoBehaviour
{
    public Miroir miroirAlpha;
    public Miroir miroirBeta;
    public Miroir miroirDelta;

    public Animator oeilAlpha; 
    public Animator oeilBeta; 
    public Animator oeilDelta;

    public Transform nouvelEmplacementDeLaPorte;

    public bool hasDoorBeenOpened = false;
    public bool hasOeilAlphaBeenTriggered = false;
    public bool hasOeilBetaBeenTriggered = false;
    public bool hasOeilDeltaBeenTriggered = false;

    void Update()
    {
        if (!hasDoorBeenOpened)
        {
            if (miroirAlpha.hasBeenTriggered && !hasOeilAlphaBeenTriggered)
            {
                oeilAlpha.SetTrigger("TriggerOpenEye");
                hasOeilAlphaBeenTriggered = true;
            }
            if (miroirBeta.hasBeenTriggered && !hasOeilBetaBeenTriggered)
            {
                oeilBeta.SetTrigger("TriggerOpenEye");
                hasOeilBetaBeenTriggered = true;
            }
            if (miroirDelta.hasBeenTriggered && !hasOeilDeltaBeenTriggered)
            {
                oeilDelta.SetTrigger("TriggerOpenEye");
                hasOeilDeltaBeenTriggered = true;
            }
            if (hasOeilAlphaBeenTriggered && hasOeilBetaBeenTriggered && hasOeilDeltaBeenTriggered)
            {
                hasDoorBeenOpened = true;
                OpenDoor();
            }
        }
    }

    private void OpenDoor()
    {
        Debug.Log("Opening the door");
        transform.position = nouvelEmplacementDeLaPorte.position;
        transform.rotation = nouvelEmplacementDeLaPorte.rotation;
    }
}
