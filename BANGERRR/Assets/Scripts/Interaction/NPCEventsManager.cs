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
    public SpriteRenderer lePuzzle;
    public Sprite puzzleFull;
    // Okaoka
    private bool OkaokaEnd = false;
    // Nepal
    private bool NepalCached = false;
    private bool NepalBack = false;
    private bool NepalEnd = false;


    [Header("Solimont")]
    public bool Soli_caillouRobbed = false;
    public bool Soli_songSung = false;
    public GameObject Soli_rocherWithFlegmardo;

    [SerializeField] public NPC Peridorm1;
    [SerializeField] public NPC Languix2;
    private bool PerilangEnd = false;
    [SerializeField] public NPC Endorsole3;
    [SerializeField] public NPC Placidon4;
    [SerializeField] public NPC Lethargio5;
    [SerializeField] public NPC Flemu6;
    [SerializeField] public NPC Somnoval7;
    private bool FlemovalEnd = false;
    [SerializeField] public NPC Hiberlus8;
    [SerializeField] public NPC Inersun9;
    private bool HibersunEnd = false;
    [SerializeField] public NPC Flegmardo10;
    private bool FlegmardoEnd = false;
    [SerializeField] public NPC Paressept11;

    public void updateNPCPages() {
        AmpNPCManager.instance.updateNPCPages();

        // Perilang
        if (!PerilangEnd)
        {
            if (Peridorm1.isPageARead || Languix2.isPageARead)
            {
                Peridorm1.pageB = true;
                Languix2.pageB = true;
                PerilangEnd = true;
            }
        }
        // Endorsole
        if (Endorsole3.isPageARead && !Endorsole3.pageB)
        {
            Endorsole3.pageB = true;
        }
        // Placidon
        if (Placidon4.isPageARead && !Placidon4.pageB)
        {
            Placidon4.pageB = true;
        }
        // Léthargio
        if (Lethargio5.isPageARead && !Lethargio5.pageB)
        {
            Lethargio5.pageB = true;
        }
        // Flemoval
        if (!FlemovalEnd)
        {
            if (Flemu6.isPageARead || Somnoval7.isPageARead)
            {
                Flemu6.pageB = true;
                Somnoval7.pageB = true;
                FlemovalEnd = true;
            }
        }
        // Hibersun
        if (!HibersunEnd)
        {
            if (Soli_caillouRobbed)
            {
                Hiberlus8.pageC = true;
                Inersun9.pageC = true;
                HibersunEnd = true;
            }
        }
        // Flegmardo
        if (!FlegmardoEnd)
        {
            if (Flegmardo10.isPageARead)
            {
                Flegmardo10.pageB = true;
            }
            if (Flegmardo10.isPageBRead)
            {
                Flegmardo10.pageC = true;
            }
            if (Soli_caillouRobbed && Flegmardo10.isPageBRead)
            {
                Flegmardo10.pageD = true;
            }
            if (Flegmardo10.isPageDRead)
            {
                FindAnyObjectByType<PlayerStatus>().RemoveRockOnHead();
                AudioManager.instance.Play("rock");
                Soli_rocherWithFlegmardo.SetActive(true);
                Flegmardo10.pageE = true;
                FlegmardoEnd = true;
            }
        }
        // Paressept
        if (Soli_songSung)
        {
            Paressept11.isPageARead = true;
            Paressept11.pageB = true;
        }

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
                lePuzzle.sprite = puzzleFull;
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
                StartCoroutine(NepalCache(true));
                NepalCached = true;
            }
            if (Nepti.isPageBRead && Coral.isPageBRead && !NepalBack)
            {
                Nepti.pageC = true;
                Coral.pageC = true;
                StartCoroutine(NepalCache(false));
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

    IEnumerator NepalCache(bool first)
    {
        FindAnyObjectByType<ThirdPersonMovement>().blockPlayerMoveInputs();
        FindAnyObjectByType<MainCameraManager>().blockMovement();

        yield return FadeToBlack.instance.Fade(true, 1);

        if (first)
        {
            FindAnyObjectByType<SolCoralTP>().Teleport();
            FindAnyObjectByType<SolNeptiTP>().Teleport();
        }
        else
        {
            FindAnyObjectByType<SolCoralTPback>().Teleport();
            FindAnyObjectByType<SolNeptiTPback>().Teleport();
        }

        yield return FadeToBlack.instance.Fade(false, 1);

        FindAnyObjectByType<ThirdPersonMovement>().unblockPlayerMoveInputs();
        FindAnyObjectByType<MainCameraManager>().unblockMovement();
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
