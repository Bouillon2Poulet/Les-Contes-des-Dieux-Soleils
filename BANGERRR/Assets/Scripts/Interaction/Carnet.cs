using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Carnet : MonoBehaviour, IInteractable
{
    public GameObject CarnetBox;

    public TextMeshProUGUI JourText;
    public TextMeshProUGUI CorpsText;

    private static int page = 0;
    private readonly string[] jours = { "Jour 157", "Jour 239", "Jour 275", "Jour 292" };
    private readonly string[] engJours = { "Day 157", "Day 239", "Day 275", "Day 292" };
    private readonly string[] texts = {
        "Des missions pour récupérer des matériaux ont été menées par des grenouilles courageuses. Elles reviennent, les bras chargés de métal, mais celui-ci a déjà commencé à s'oxyder. La température en surface est devenue si intense que les structures gonflables du parc aquatique ont commencé à s'effondrer. Le sol brûle leurs pieds, mais le pire reste la malédiction du soleil rouge : plus de la moitié des grenouilles qui partent en mission s'éteignent dans les jours qui suivent leur retour. La base est presque terminée, nous pourrons bientôt commencer la construction du premier prototype de fusée.",
        "La construction de la première fusée avance, mais le manque de ressources nous pousse à envisager que toutes les grenouilles ne pourront pas partir. Nous gardons toutefois l'espoir de sauver 23% de la population actuelle. Le problème réside dans la destination des fusées, sans coordonnées astrales, nous sommes incapables de décider où les orienter. L'espoir de la survie de notre peuple serait alors laissé au hasard, en les envoyant de manière aléatoire dans l'espace, en espérant qu'une d'elles atterrisse sur une planète habitable...",
        "Nous ne sommes plus qu'une dizaine de grenouilles. Le conseil s'est réuni hier et a élaboré un nouveau plan. La mission des fusées n'est plus de transporter les survivants, mais des œufs. Ceux-ci seront maintenus dans un état de stase, leur permettant de survivre des milliers d'années en attendant de se rapprocher d'une planète habitable. Cependant, les calculs de Solétude indiquent que d'ici 50 jours, nous serons tous morts, et pour l'instant, aucun œuf n'est prêt pour un tel voyage.",
        "Je suis le seul survivant. La chaleur rend la culture des œufs impossible. Mon temps est compté. Je vais devoir tenter le tout pour le tout et envoyer l'œuf le plus prometteur à bord d'une fusée. Me reposer sur le hasard et sur Omnio n'est pas dans mes habitudes de scientifique, et pourtant, aujourd'hui, je me suis surpris à prier pour la première fois. Que l'Esprit d'Omnio protège cet œuf et le guide vers un endroit saint, loin des maux du soleil rouge. Bonne chance, gamin."
    };
    private readonly string[] engTexts = {
        "Missions to recover materials have been carried out by brave frogs. They return, arms full of metal, but it has already begun to oxidize. The surface temperature has become so intense that the inflatable structures of the water park have begun to collapse. The ground burns their feet, but worst of all is the curse of the red sun: more than half the frogs that go on missions die out within days of their return. The base is almost complete, and we'll soon be able to start building the first prototype rocket.",
        "Construction of the first rocket is progressing, but the lack of resources means that not all the frogs will be able to leave. However, we're still hopeful of saving 23% of the current population. The problem lies in the destination of the rockets: without astral coordinates, we're unable to decide where to direct them. Hope for the survival of our people would then be left to chance, by sending them randomly into space, in the hope that one of them would land on a habitable planet…",
        "We're down to a dozen frogs. The council met yesterday and came up with a new plan. The rockets' mission is no longer to transport survivors, but eggs. These will be kept in a state of stasis, enabling them to survive for thousands of years until they approach a habitable planet. However, Solétude's calculations indicate that within 50 days, we'll all be dead, and as yet, no eggs are ready for such a journey.",
        "I'm the only survivor. The heat makes egg cultivation impossible. My time is running out. I'll have to take a chance and send the most promising egg aboard a rocket. Relying on chance and Omnio is not my usual scientific practice, yet today I found myself praying for the first time. May Omnio's Spirit protect this egg and guide it to a holy place, far from the evils of the red sun. Good luck, kid."
    };

    private string[] currentJours;
    private string[] currentTexts;

    public GameObject noteSprite;

    [Header("Rotation")]
    public float rotationSpeed = 100f;
    private float angle = 0f;

    [Header("Up and Down")]
    public float amplitude = .1f;
    public float frequency = .5f;
    private Vector3 startPos;

    bool justClosed = false;
    bool justOpened = false;

    private void Update()
    {
        IdleRotateNote();
        IdleMoveNote();

        if (Input.GetKeyDown(KeyCode.E) && CarnetBox.activeSelf && !justOpened)
        {
            AudioManager.instance.Play("paper");
            CarnetBox.SetActive(false);
            PlayerStatus.instance.GameMenuCursor(false);
            PlayerStatus.instance.isAnimated = false;
            FindAnyObjectByType<ThirdPersonMovement>().unblockPlayerMoveInputs();
            justClosed = true;
            StartCoroutine(nameof(DisableJustClosed));
        }
    }

    IEnumerator DisableJustClosed()
    {
        yield return new WaitForSecondsRealtime(.1f);
        justClosed = false;
    }

    IEnumerator DisableJustOpened()
    {
        yield return new WaitForSecondsRealtime(.1f);
        justOpened = false;
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
        LanguageManager.Lang lang = (LanguageManager.Lang)GlobalVariables.Get<int>("lang");
        if (lang == LanguageManager.Lang.French)
        {
            currentJours = jours;
            currentTexts = texts;
        }
        else
        {
            currentJours = engJours;
            currentTexts = engTexts;
        }

        CarnetBox.SetActive(false);
        JourText.text = currentJours[page];
        CorpsText.text = currentTexts[page];

        noteSprite = GetComponentInChildren<SpriteRenderer>().gameObject;
        startPos = noteSprite.transform.localPosition;
    }

    public void Interact()
    {
        if(!CarnetBox.activeSelf && !justClosed)
        {
            AudioManager.instance.Play("paper");
            CarnetBox.SetActive(true);
            PlayerStatus.instance.GameMenuCursor(true);
            PlayerStatus.instance.isAnimated = true;
            FindAnyObjectByType<ThirdPersonMovement>().blockPlayerMoveInputs();
            justOpened = true;
            StartCoroutine(nameof(DisableJustOpened));
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
        JourText.text = currentJours[page];
        CorpsText.text = currentTexts[page];
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
