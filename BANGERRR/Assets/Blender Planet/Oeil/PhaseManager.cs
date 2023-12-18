using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseManager : MonoBehaviour
{
    Blow blow;
    Aspire aspire;
    public Blink upBlink;
    public Blink downBlink;
    Missile missileLauncher;
    LineInstantiator laser;
    DialogManager dialog;
    ThirdPersonMovement player;

    readonly string dialogName = "Omnio";
    public bool hitByMissile = false;

    private void Start()
    {
        blow = FindAnyObjectByType<Blow>();
        aspire = FindAnyObjectByType<Aspire>();
        missileLauncher = FindAnyObjectByType<Missile>();
        laser = FindAnyObjectByType<LineInstantiator>();
        dialog = FindAnyObjectByType<DialogManager>();
        player = FindAnyObjectByType<ThirdPersonMovement>();

        CentreMat = CentreRenderer.materials[2];
        paupiereUpMat = PaupiereUp.material;
        paupiereDownMat = PaupiereDown.material;
        CentreMat.EnableKeyword("_EmissionColor");
        paupiereUpMat.EnableKeyword("_EmissionColor");
        paupiereDownMat.EnableKeyword("_EmissionColor");

        StartCoroutine(Phase0());
        //StartCoroutine(Phase4());
    }

    public Renderer PaupiereUp;
    public Renderer PaupiereDown;
    public Renderer CentreRenderer;
    public float hueRotationSpeed = 200.0f;

    private float hueValue = 0.0f;
    private Material paupiereUpMat;
    private Material paupiereDownMat;
    private Material CentreMat;
    private readonly float[] hueSpeeds = { 200f, 300f, 548f, 1096f, 4770f };


    void Update()
    {
        hueValue = (hueValue + hueRotationSpeed * Time.deltaTime) % 360.0f;

        Color newColor = Color.HSVToRGB(hueValue / 360.0f, 1.0f, 1.0f);
        CentreMat.SetColor("_EmissionColor", newColor);
        paupiereUpMat.SetColor("_EmissionColor", newColor);
        paupiereDownMat.SetColor("_EmissionColor", newColor);
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            TriggerReset();
        }
    }*/

    // Reset Events
    private IEnumerator Fade(bool fadeIn, float duration)
    {
        if (fadeIn)
            FadeToBlack.instance.FadeInBlack(duration);
        else
            FadeToBlack.instance.FadeOutBlack(duration);
        yield return new WaitUntil(() => !FadeToBlack.instance.fading);
    }
    public void TriggerReset()
    {
        Debug.Log("RESET");
        StopAllCoroutines();
        StartCoroutine(ResetPlayer());
    }
    private IEnumerator ResetPlayer()
    {
        AudioManager.instance.Play("bulledead");
        AudioManager.instance.FadeOut("fight", 60);
        yield return Fade(true, 1f);

        player.TeleportPlayerTo(new Vector3(-86f, -21f, -86f));
        player.blockPlayerMoveInputs();
        player.JETPACKMODE = true;
        yield return new WaitForSeconds(2f);

        player.unblockPlayerMoveInputs();
        player.JETPACKMODE = false;
        Centre.instance.playerTouched = false;
        yield return Fade(false, 1f);

        StartCoroutine(Phase0());
        //StartCoroutine(PhaseTest());
    }

    // Boss Actions
    private IEnumerator Blink(int amount, float mo, float speed)
    {
        BlinkEye(amount, speed);
        yield return new WaitUntil(() => !upBlink.IsBlinking());
        yield return new WaitForSeconds(mo);
    }
    private IEnumerator Talk(string text, string engText, float duration, float mo)
    {
        AudioManager.instance.Play("omnio_talk");
        StartCoroutine(dialog.EphemeralMessage(dialogName, text, engText, duration, "Oeil"));
        yield return new WaitUntil(() => !dialog.ephemeralMessageGoing);
        yield return new WaitForSeconds(mo);
    }
    private IEnumerator Laser(float duration, float mo)
    {
        AudioManager.instance.Play("omnio_lasercharge");
        laser.Trigger(duration);
        yield return new WaitUntil(() => !laser.laserGoing);
        yield return new WaitForSeconds(mo);
    }
    private IEnumerator Blow(int amount, float interval, float mo)
    {
        Centre.instance.hit = false;
        hitByMissile = false;
        AudioManager.instance.Play("omnio_blow");
        blow.Trigger();

        int di = 0;

        while (!Centre.instance.hit && !hitByMissile)
        {
            Debug.Log("INTERVAL " + ++di);
            float startingTime = Time.time;
            float endInterval = startingTime + interval;
            bool missile1 = false;
            bool missile2 = false;
            bool missile3 = false;
            while (Time.time < endInterval && !Centre.instance.hit && !hitByMissile)
            {
                if (!missile1)
                {
                    Debug.Log("LAUNCH 1");
                    missileLauncher.Trigger();
                    missile1 = true;
                }
                else if (amount > 1 && Time.time > startingTime + .5f && !missile2)
                {
                    Debug.Log("LAUNCH 2");
                    missileLauncher.Trigger();
                    missile2 = true;
                }
                else if (amount > 2 && Time.time > startingTime + 1f && !missile3)
                {
                    Debug.Log("LAUNCH 3");
                    missileLauncher.Trigger();
                    missile3 = true;
                }
                yield return null;
            }
        }
        if (hitByMissile)
        {
            Debug.Log("player BLOWN!!");
            PlayerStatus.instance.HitBlink();
            TriggerReset();
        }
        else
        {
            Debug.Log("Normal END OF BLOW");
            yield return new WaitForSeconds(mo);
        }
    }

    // Phases
    private IEnumerator PhaseTest()
    {
        Debug.Log("Phase Test");
        yield return new WaitForSeconds(2);
        yield return Blow(1, 10, 2);
        yield return Blow(2, 5, 2);
        yield return Blow(3, 8, 2);
    }

    private IEnumerator Phase0()
    {
        Debug.Log("Phase 0");
        yield return new WaitUntil(() => Centre.instance.playerTouched);
        yield return new WaitForSeconds(6);
        hueRotationSpeed = hueSpeeds[0];

        yield return Blink(1, 2, 15f);
        yield return Talk("Qui ose perturber mon sommeil ?!", "Who dares disrupt my sleep?!", 6, 2);
        AudioManager.instance.FadeIn("fight", 200);
        yield return Talk("Crains ma col�re...", "Fear my wrath...", 4, 2);
        StartCoroutine(Phase1());
    }
    private IEnumerator Phase1()
    {
        Debug.Log("Phase 1");
        hueRotationSpeed = hueSpeeds[1];

        yield return Blink(1, .5f, 17f);
        yield return Laser(3, 1);
        yield return Blow(1, 10, 1);
        yield return Talk("Je t'aurais pr�venu...", "I warned you...", 4, 2);
        StartCoroutine(Phase2());
    }
    private IEnumerator Phase2()
    {
        Debug.Log("Phase 2");
        hueRotationSpeed = hueSpeeds[2];

        yield return Blink(2, .4f, 23f);
        yield return Laser(6, .8f);
        yield return Blow(2, 8, 2);
        yield return Talk("Cette fois, tu es fini !", "This time, you're done for!", 4.5f, 2);
        StartCoroutine(Phase3());
    }
    private IEnumerator Phase3()
    {
        Debug.Log("Phase 3");
        hueRotationSpeed = hueSpeeds[3];

        yield return Blink(3, .3f, 30f);
        yield return Laser(12, .7f);
        yield return Blow(3, 7, 3);
        yield return Talk("Aaaaaah maudit sois-tu !!!", "Aaaaaah damn you!!!!", 4.7f, 1);
        StartCoroutine(Phase4());
    }

    private IEnumerator Phase4()
    {
        Debug.Log("Phase 4");
        hueRotationSpeed = hueSpeeds[4];
        AudioManager.instance.FadeOut("fight", 140);

        Debug.Log("Fin");
        yield return FadeToBlack.instance.FadeWhiteEdition(true, .15f);
        yield return new WaitForSeconds(1f);
        LoadSceneManager.instance.LoadScene(2, false);
        yield return null;
    }

    // Code optimization
    private void BlinkEye(int amount, float speed)
    {
        upBlink.Trigger(amount, speed);
        downBlink.Trigger(amount, speed);
    }

    public static PhaseManager instance { get; private set; }
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
