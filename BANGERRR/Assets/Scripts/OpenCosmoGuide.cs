using UnityEngine;
using UnityEngine.UI;

public class OpenCosmoGuide : MonoBehaviour
{
    public Canvas cosmoguideCanvas;
    public bool CosmoGuideIsOpen = false;
    public Texture CosmoGuide;
    public Texture Background;
    public GameObject playerObject;
    public GameObject timeObject;
    private RawImage rawImage;

    void Start()
    {
        rawImage = cosmoguideCanvas.GetComponent<RawImage>();
        rawImage.texture = Background;
        if (rawImage == null)
        {
            Debug.LogError("Le composant RawImage n'est pas attaché au cosmoguideCanvas !");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CosmoGuideIsOpen = !CosmoGuideIsOpen;
            if (rawImage != null)
            {
                playerObject.SetActive(!CosmoGuideIsOpen);
                timeObject.SetActive(CosmoGuideIsOpen);

                rawImage.texture = CosmoGuideIsOpen ? CosmoGuide : Background;
            }

        }
    }
}
