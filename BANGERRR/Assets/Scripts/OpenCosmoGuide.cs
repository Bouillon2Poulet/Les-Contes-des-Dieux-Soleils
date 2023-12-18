using UnityEngine;
using UnityEngine.UI;

public class OpenCosmoGuide : MonoBehaviour
{
    public GameObject PixelatedImage;
    public bool CosmoGuideIsOpen = false;
    public Texture CosmoGuide;
    public Texture Background;
    public PlayerStatus playerStatus;
    public GameObject timeObject;

    private RawImage rawImage;
    public int nbOpenings;

    void Start()
    {
        rawImage = PixelatedImage.GetComponent<RawImage>();
        rawImage.texture = Background;
        if (rawImage == null)
        {
            Debug.LogError("Le composant RawImage n'est pas attaché au cosmoguideCanvas !");
        }
        timeObject.SetActive(CosmoGuideIsOpen);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && playerStatus.hasCosmoGuide && !playerStatus.isAnimated)
        {
            CosmoGuideIsOpen = !CosmoGuideIsOpen;
            if (CosmoGuideIsOpen)
                FindAnyObjectByType<ThirdPersonMovement>().blockPlayerMoveInputs();
            else
                FindAnyObjectByType<ThirdPersonMovement>().unblockPlayerMoveInputs();

            if (rawImage != null)
            {
                // playerObject.SetActive(!CosmoGuideIsOpen);
                timeObject.SetActive(CosmoGuideIsOpen);
                if (CosmoGuideIsOpen)
                {
                    nbOpenings++;
                    FindAnyObjectByType<NPCEventsManager>().updateNPCPages();
                }
                //Debug.Log("nombre d'ouverture du cosmoguide : " + nbOpenings);

                rawImage.texture = CosmoGuideIsOpen ? CosmoGuide : Background;
            }
        }
    }

    public void ForceCloseCosmoguide()
    {
        CosmoGuideIsOpen = !CosmoGuideIsOpen;
        FindAnyObjectByType<ThirdPersonMovement>().unblockPlayerMoveInputs();
        if (rawImage != null)
        {
            timeObject.SetActive(CosmoGuideIsOpen);
            rawImage.texture = CosmoGuideIsOpen ? CosmoGuide : Background;
        }
    }

    public static OpenCosmoGuide instance { get; private set; }

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
