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

    // Astrid
    private int Astrid_nbOpenings = 0;
    private bool Astrid_gotNbOpenings = false;
    private bool AstridEnd = false;

    public void updateNPCPages() {
        // Nere
        if (Nere.isPageARead)
        {
            Nere.pageB = true;
        }

        // Astrid
        if (!AstridEnd)
        {
            if (Astrid.isPageARead && !Astrid_gotNbOpenings)
            {
                Astrid.pageB = true;
                Astrid_nbOpenings = FindAnyObjectByType<OpenCosmoGuide>().nbOpenings;
                Astrid_gotNbOpenings = true;
            }
            if (FindAnyObjectByType<OpenCosmoGuide>().nbOpenings > Astrid_nbOpenings && Astrid.isPageARead)
            {
                Astrid.pageC = true;
            }
            if (Astrid.isPageCRead)
            {
                Astrid.pageD = true;
                AstridEnd = true;
            }
        }
    }
}
