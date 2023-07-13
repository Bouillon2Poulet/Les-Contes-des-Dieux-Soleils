using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEventsManager : MonoBehaviour
{
    [Header("Solisède")]
    [SerializeField] public NPC Nere;
    [SerializeField] public NPC Astrid;
    [SerializeField] public NPC Isador;
    [SerializeField] public NPC Okaoka;
    [SerializeField] public NPC Coral;
    [SerializeField] public NPC Nepti;

    public void updateNPCPages() {
        // Nere
        if (Nere.isPageARead)
        {
            Nere.pageB = true;
        }

        // Astrid
        if (Astrid.isPageARead)
        {
            Astrid.pageB = true;
        }
        if (FindAnyObjectByType<OpenCosmoGuide>().nbOpenings > 0 && Astrid.isPageARead)
        {
            Astrid.pageC = true;
        }
    }
}
