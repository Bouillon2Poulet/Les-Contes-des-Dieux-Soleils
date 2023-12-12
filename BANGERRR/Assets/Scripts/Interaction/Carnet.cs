using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Carnet : MonoBehaviour, IInteractable
{
    public GameObject CarnetBox;
    private bool hasAlreadyChangedStateThisFrame = false;

    public TextMeshProUGUI JourText;
    public TextMeshProUGUI CorpsText;

    private static int page = 0;
    private readonly string[] jours = { "Jour 157", "Jour 239", "Jour 275", "Jour 292" };
    private readonly string[] texts = {
        "Des missions pour r�cup�rer des mat�riaux ont �t� men�es par des grenouilles courageuses. Elles reviennent, les bras charg�s de m�tal, mais celui-ci a d�j� commenc� � s'oxyder. La temp�rature en surface est devenue si intense que les structures gonflables du parc aquatique ont commenc� � s'effondrer. Le sol br�le leurs pieds, mais le pire reste la mal�diction du soleil rouge : plus de la moiti� des grenouilles qui partent en mission s'�teignent dans les jours qui suivent leur retour. La base est presque termin�e, nous pourrons bient�t commencer la construction du premier prototype de fus�e.",
        "La construction de la premi�re fus�e avance, mais le manque de ressources nous pousse � envisager que toutes les grenouilles ne pourront pas partir. Nous gardons toutefois l'espoir de sauver 23% de la population actuelle. Le probl�me r�side dans la destination des fus�es, sans coordonn�es astrales, nous sommes incapables de d�cider o� les orienter. L'espoir de la survie de notre peuple serait alors laiss� au hasard, en les envoyant de mani�re al�atoire dans l'espace, en esp�rant qu'une d'elles atterrisse sur une plan�te habitable...",
        "Nous ne sommes plus qu'une dizaine de grenouilles. Le conseil s'est r�uni hier et a �labor� un nouveau plan. La mission des fus�es n'est plus de transporter les survivants, mais des �ufs. Ceux-ci seront maintenus dans un �tat de stase, leur permettant de survivre des milliers d'ann�es en attendant de se rapprocher d'une plan�te habitable. Cependant, les calculs de Sol�tude indiquent que d'ici 50 jours, nous serons tous morts, et pour l'instant, aucun �uf n'est pr�t pour un tel voyage.",
        "Je suis le seul survivant. La chaleur rend la culture des �ufs impossible. Mon temps est compt�. Je vais devoir tenter le tout pour le tout et envoyer l'�uf le plus prometteur � bord d'une fus�e. Me reposer sur le hasard et sur Omnio n'est pas dans mes habitudes de scientifique, et pourtant, aujourd'hui, je me suis surpris � prier pour la premi�re fois. Que l'Esprit d'Omnio prot�ge cet �uf et le guide vers un endroit saint, loin des maux du soleil rouge. Bonne chance, gamin."
    };


    public GameObject noteSprite;

    [Header("Rotation")]
    public float rotationSpeed = 100f;
    private float angle = 0f;

    [Header("Up and Down")]
    public float amplitude = .1f;
    public float frequency = .5f;
    private Vector3 startPos;

    private void Update()
    {
        if (hasAlreadyChangedStateThisFrame)
            hasAlreadyChangedStateThisFrame = false;

        IdleRotateNote();
        IdleMoveNote();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CarnetBox.activeSelf && !hasAlreadyChangedStateThisFrame)
            {
                hasAlreadyChangedStateThisFrame = true;
                AudioManager.instance.Play("paper");
                CarnetBox.SetActive(false);
                PlayerStatus.instance.GameMenuCursor(false);
                PlayerStatus.instance.isAnimated = false;
                FindAnyObjectByType<ThirdPersonMovement>().unblockPlayerMoveInputs();
            }
        }
    }

    private void IdleRotateNote()
    {
        angle += Time.fixedDeltaTime * rotationSpeed;
        noteSprite.transform.localRotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void IdleMoveNote()
    {
        float verticalOffset = amplitude * Mathf.Sin(Time.time * 2 * Mathf.PI * frequency);
        noteSprite.transform.localPosition = startPos + new Vector3(0f, verticalOffset, 0f);
    }

    private void Start()
    {
        CarnetBox.SetActive(false);
        JourText.text = jours[page];
        CorpsText.text = texts[page];

        noteSprite = GetComponentInChildren<SpriteRenderer>().gameObject;
        startPos = noteSprite.transform.localPosition;
    }

    public void Interact()
    {
        if(!hasAlreadyChangedStateThisFrame)
        {
            hasAlreadyChangedStateThisFrame = true;
            AudioManager.instance.Play("paper");
            bool newState = !CarnetBox.activeSelf;
            CarnetBox.SetActive(newState);
            PlayerStatus.instance.GameMenuCursor(newState);
            PlayerStatus.instance.isAnimated = newState;

            if (newState)
            {
                FindAnyObjectByType<ThirdPersonMovement>().blockPlayerMoveInputs();
            }
            else
            {
                FindAnyObjectByType<ThirdPersonMovement>().unblockPlayerMoveInputs();
            }
        }
    }

    public void NextPage()
    {
        Debug.Log("NextPage");
        page++;
        UpdatePageTexts();
    }

    public void PreviousPage()
    {
        Debug.Log("PreviousPage");
        page--;
        UpdatePageTexts();
    }

    public void UpdatePageTexts()
    {
        Debug.Log("UpdatePageTexts");
        page = Mathf.Abs(page % 4);
        JourText.text = jours[page];
        CorpsText.text = texts[page];
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
