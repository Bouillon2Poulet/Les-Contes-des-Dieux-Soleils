using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEventsManager : MonoBehaviour
{
    [SerializeField] public NPC Nere;

    public void updateNPCPages() {
        if (Nere.isPageARead)
        {
            Nere.pageB = true;
        }
    }
}
