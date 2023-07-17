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

    public bool Sol_ritualStarted = false;

    //Nere
    private bool NereEnd = false;
    // Astrid
    private int Astrid_nbOpenings = 0;
    private bool Astrid_gotNbOpenings = false;
    private bool AstridEnd = false;
    // Isador
    public bool Isador_PuzzlePieceFound = false;
    private bool IsadorEnd = false;
    // Okaoka
    private bool OkaokaEnd = false;
    // Nepal
    private bool NepalCached = false;
    private bool NepalBack = false;
    private bool NepalEnd = false;

    public void updateNPCPages() {
        // Nere
        if (!NereEnd)
        {
            if (Nere.isPageARead)
            {
                Nere.pageB = true;
            }
            if (Nere.isPageARead && Astrid.isPageCRead && Isador.isPageCRead && Okaoka.isPageARead && Coral.isPageBRead && Nepti.isPageBRead)
            {
                Nere.pageC = true;
            }
            if (Sol_ritualStarted)
            {
                Nere.pageD = true;
                NereEnd = true;
            }
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

        // Isador
        if (!IsadorEnd)
        {
            if (Isador.isPageARead)
            {
                Isador.pageB = true;
            }
            if (Isador.isPageARead && Isador_PuzzlePieceFound)
            {
                Isador.pageC = true;
            }
            if (Isador.isPageCRead)
            {
                Isador.pageD = true;
                IsadorEnd = true;
            }
        }

        // Okaoka
        if (!OkaokaEnd)
        {
            if (Okaoka.isPageARead)
            {
                Okaoka.pageB = true;
            }
            if (Sol_ritualStarted)
            {
                Okaoka.pageC = true;
                OkaokaEnd = true;
            }
        }

        // Nepal
        if (!NepalEnd)
        {
            if ((Nepti.isPageARead || Coral.isPageARead) && !NepalCached)
            {
                Nepti.pageB = true;
                Coral.pageB = true;
                FindAnyObjectByType<SolCoralTP>().Teleport();
                FindAnyObjectByType<SolNeptiTP>().Teleport();
                NepalCached = true;
            }
            if (Nepti.isPageBRead && Coral.isPageBRead && !NepalBack)
            {
                FindAnyObjectByType<SolCoralTPback>().Teleport();
                FindAnyObjectByType<SolNeptiTPback>().Teleport();
                Nepti.pageC = true;
                Coral.pageC = true;
                NepalBack = true;
            }
            if ((Nepti.isPageCRead || Coral.isPageCRead) && NepalBack)
            {
                Nepti.pageD = true;
                Coral.pageD = true;
                NepalEnd = true;
            }
        }
    }

    public void SolDeactivateNPCs()
    {
        Nere.gameObject.SetActive(false);
        Astrid.gameObject.SetActive(false);
        Isador.gameObject.SetActive(false);
        Okaoka.gameObject.SetActive(false);
        Coral.gameObject.SetActive(false);
        Nepti.gameObject.SetActive(false);
    }
}
