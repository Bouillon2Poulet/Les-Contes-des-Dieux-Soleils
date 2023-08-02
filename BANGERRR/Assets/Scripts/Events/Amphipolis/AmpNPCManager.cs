using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpNPCManager : MonoBehaviour
{
    [SerializeField] public NPC Sip;
    private bool SipEnd = false;

    [SerializeField] public NPC Ordi;
    private bool OrdiEnd = false;

    public GameObject[] InteriorNPCObjects;

    public void updateNPCPages()
    {
        if (!SipEnd)
        {
            if (Sip.isPageARead)
            {
                Sip.pageB = true;
                SipEnd = true;
            }
        }

        if (!OrdiEnd)
        {
            if (Ordi.isPageARead)
            {
                Ordi.pageB = true;
            }
            if (Ordi.isPageBRead)
            {
                Ordi.pageC = true;
                OrdiEnd = true;
            }
        }
    }
    
    private void Start()
    {
        ToggleInteriorObjects(false);
    }

    public void ToggleInteriorObjects(bool state)
    {
        foreach (GameObject o in InteriorNPCObjects)
        {
            o.SetActive(state);
        }
    }

    public static AmpNPCManager instance { get; private set; }
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
