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
        if (Input.GetKeyDown(KeyCode.C) && playerStatus.hasCosmoGuide)
        {
            CosmoGuideIsOpen = !CosmoGuideIsOpen;
            if (CosmoGuideIsOpen)
                FindObjectOfType<ThirdPersonMovement>().blockPlayerMoveInputs();
            else
                FindObjectOfType<ThirdPersonMovement>().unblockPlayerMoveInputs();

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
}
