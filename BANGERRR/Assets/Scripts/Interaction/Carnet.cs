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
        "Des missions pour récupérer des matériaux ont été menées par des grenouilles courageuses. Elles reviennent, les bras chargés de métal, mais celui-ci a déjà commencé à s'oxyder. La température en surface est devenue si intense que les structures gonflables du parc aquatique ont commencé à s'effondrer. Le sol brûle leurs pieds, mais le pire reste la malédiction du soleil rouge : plus de la moitié des grenouilles qui partent en mission s'éteignent dans les jours qui suivent leur retour. La base est presque terminée, nous pourrons bientôt commencer la construction du premier prototype de fusée.",
        "La construction de la première fusée avance, mais le manque de ressources nous pousse à envisager que toutes les grenouilles ne pourront pas partir. Nous gardons toutefois l'espoir de sauver 23% de la population actuelle. Le problème réside dans la destination des fusées, sans coordonnées astrales, nous sommes incapables de décider où les orienter. L'espoir de la survie de notre peuple serait alors laissé au hasard, en les envoyant de manière aléatoire dans l'espace, en espérant qu'une d'elles atterrisse sur une planète habitable...",
        "Nous ne sommes plus qu'une dizaine de grenouilles. Le conseil s'est réuni hier et a élaboré un nouveau plan. La mission des fusées n'est plus de transporter les survivants, mais des œufs. Ceux-ci seront maintenus dans un état de stase, leur permettant de survivre des milliers d'années en attendant de se rapprocher d'une planète habitable. Cependant, les calculs de Solétude indiquent que d'ici 50 jours, nous serons tous morts, et pour l'instant, aucun œuf n'est prêt pour un tel voyage.",
        "Je suis le seul survivant. La chaleur rend la culture des œufs impossible. Mon temps est compté. Je vais devoir tenter le tout pour le tout et envoyer l'œuf le plus prometteur à bord d'une fusée. Me reposer sur le hasard et sur Omnio n'est pas dans mes habitudes de scientifique, et pourtant, aujourd'hui, je me suis surpris à prier pour la première fois. Que l'Esprit d'Omnio protège cet œuf et le guide vers un endroit saint, loin des maux du soleil rouge. Bonne chance, gamin."
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
