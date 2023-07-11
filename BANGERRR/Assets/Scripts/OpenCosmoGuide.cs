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
            if (rawImage != null)
            {
                // playerObject.SetActive(!CosmoGuideIsOpen);
                timeObject.SetActive(CosmoGuideIsOpen);

                rawImage.texture = CosmoGuideIsOpen ? CosmoGuide : Background;
            }
        }
    }
}
