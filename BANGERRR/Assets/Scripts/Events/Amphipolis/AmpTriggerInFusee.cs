using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpTriggerInFusee : MonoBehaviour
{
    private bool hublotClosed = false;

    public GameObject fusee_body;
    public GameObject fusee_couvercle;

    public Transform base_fusee_pince;
    public Transform anim_fusee;

    public Transform StartPos;
    public Transform EndPos;
    private bool isDescending = false;
    private float animationProgress = 0f;

    private GameObject player;

    public GameObject gaInt;
    public GameObject gaExt;
    public GameObject MurInvisible2;
    public GameObject gaFusee;
    public Transform gaUpRot;
    private Quaternion initialGaRotation;

    public GameObject capuchon;

    public Transform fuseeDirector;
    private Quaternion initialFuseeRotation;
    private bool initRotationSet = false;
    private bool dyingMusicStarted = false;
    private bool startPointingTowardsSun = false;
    private float rotationProgress = 0f;

    private bool GOFUSEE = false;
    private float goingSpeed = 0f;
    private bool goingIsMaxed = false;

    public GameObject FuseeKiller;

    private bool jetpackMessageSent = false;
    readonly string jetpackLine = "[!] RÉSERVÉ AU PERSONNEL QUALIFIÉ [!]<br>CONTRÔLES: [A] et [E]<br>[!] RISQUES D’EXPLOSION [!]<br>[!] ACTIVATION AUTOMATIQUE [!]";
    readonly string engJetpackLine = "[!] QUALIFIED PERSONNEL ONLY [!]<br>CONTROLS: [A] and [E]<br>[!] EXPLOSION HAZARDS [!]<br>[!] AUTOMATIC ACTIVATION [!]";

    private void OnCollisionEnter(Collision collision)
    {
        if (!hublotClosed)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                hublotClosed = true;

                Debug.Log("Jpars dans ma fusée hoooo hoooo");
                AmpAnimationFusee.instance.CloseHublot();

                gameObject.layer = 8;
                fusee_body.layer = 8;
                fusee_couvercle.layer = 8;

                base_fusee_pince.SetParent(anim_fusee);

                StartPos.position = fusee_body.transform.position;
                EndPos.position = StartPos.position;
                EndPos.localPosition += new Vector3(0f, -30f, 0f);

                //StickPlayer();
                gaFusee.SetActive(true);

                Invoke(nameof(Descend), 2);
            }
        }
    }

    private void FixedUpdate()
    {
        fuseeDirector.position = fusee_body.transform.position;
        if (isDescending)
        {
            //Debug.Log(System.Math.Round(animationProgress,1));
            fusee_body.transform.position = Vector3.Lerp(StartPos.position, EndPos.position, animationProgress);

            animationProgress += .001f;

            if (animationProgress >= .4f && !jetpackMessageSent)
            {
                jetpackMessageSent = true;
                AudioManager.instance.Play("paper");
                StartCoroutine(DialogManager.instance.EphemeralMessage("Jetpack", jetpackLine, engJetpackLine, 12f, "Amphipolis"));
            }

            if (animationProgress >= .5f)
            {
                if (!initRotationSet)
                {
                    initialFuseeRotation = fusee_body.transform.rotation;
                    initialGaRotation = gaFusee.transform.rotation;
                    initRotationSet = true;
                    startPointingTowardsSun = true;
                }
            }

            if (animationProgress >= .7f)
            {
                if (!dyingMusicStarted)
                {
                    dyingMusicStarted = true;
                    AudioManager.instance.FadeOut("amphipolis_int", 120);
                    AudioManager.instance.FadeOut("amphipolis", 120);
                    AudioManager.instance.FadeIn("dying", 120);
                }
            }

            if (animationProgress >= 1)
            {
                isDescending = false;
                AudioManager.instance.Stop("elevatorloop");
                Debug.Log("Fusee down B)");
                fusee_body.transform.SetParent(null);
            }
        }

        if (startPointingTowardsSun)
        {
            fusee_body.transform.rotation = Quaternion.Slerp(initialFuseeRotation, fuseeDirector.rotation, rotationProgress);

            gaFusee.transform.rotation = Quaternion.Slerp(initialGaRotation, gaUpRot.rotation, rotationProgress);

            rotationProgress += 0.002f;
            if (rotationProgress >= 1f)
            {
                Debug.Log("fusee pointing sun");
                startPointingTowardsSun = false;
                GOFUSEE = true;
                AudioManager.instance.FadeIn("thruster", 360);
                StartFuseeParticles();
                FuseeKiller.SetActive(true);
            }
        }

        if (GOFUSEE)
        {
            //Debug.Log("GOING!!!");
            fusee_body.transform.rotation = fuseeDirector.rotation;
            fusee_body.transform.Translate(-transform.right * Time.fixedDeltaTime * goingSpeed);
            if (!goingIsMaxed)
            {
                goingSpeed += 0.05f;
                if (goingSpeed >= 20f)
                {
                    goingIsMaxed = true;
                }
            }
        }
    }

    private void Descend()
    {
        AmpAnimationFusee.instance.KillAnimator();
        gaExt.SetActive(false);
        gaInt.SetActive(false);
        capuchon.SetActive(false);
        MurInvisible2.SetActive(false);

        AudioManager.instance.Play("elevatorloop");

        isDescending = true;
    }

    public void Kill()
    {
        Debug.Log("KILL FUSEE");
        AudioManager.instance.Play("rocketblow");
        AudioManager.instance.Stop("thruster");
        fusee_body.SetActive(false);

        instance = null;
    }

    [SerializeField] public ParticleSystem[] pSys;

    private void Start()
    {
        goingIsMaxed = false;
        initRotationSet = false;
        startPointingTowardsSun = false;
        rotationProgress = 0f;

        GOFUSEE = false;
        goingSpeed = 0f;
        goingIsMaxed = false;
        isDescending = false;
        animationProgress = 0f;

        foreach (ParticleSystem psys in pSys)
        {
            psys.Play();
            ToggleEmission(psys, false);
        }
    }

    public void StartFuseeParticles()
    {
        foreach (ParticleSystem psys in pSys)
        {
            ToggleEmission(psys, true);
        }
    }

    public void ToggleEmission(ParticleSystem pSys, bool state)
    {
        var em = pSys.emission;
        em.enabled = state;
    }

    public static AmpTriggerInFusee instance { get; private set; }
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
