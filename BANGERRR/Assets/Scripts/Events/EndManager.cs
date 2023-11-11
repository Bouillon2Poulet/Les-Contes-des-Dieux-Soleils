using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    public GameObject mainCamera;

    public PlanetTag fin;

    [SerializeField] bool playingTheEnd = true;

    public Canvas ChoixCanvas;
    private Func<bool> DialogManagerFinished;
    public GameObject Laser;
    public FollowingTargetMore Oeil;
    public GameObject NouvelleCible;

    public static int choice = 0;

    private void Awake()
    {
        DialogManagerFinished = () => !DialogManager.instance.isItActive();
        if (ChapterManager.maxChapterIndexDiscoveredByPlayer == 6)
        {
            fin.DiscoverPlanet();
        }
        ChapterManager.currentChapterIndex = 6;
    }

    void Start()
    {
        ChoixCanvas.enabled = false;

        StartCoroutine(nameof(MakeMainCameraWork));
        if (playingTheEnd)
        {
            StartCoroutine(TheEnd());
        }
    }

    IEnumerator TheEnd()
    {
        Debug.Log("The End Coroutine");
        //yield return FadeToBlack.instance.FadeWhiteEdition(false, .1f);
        yield return new WaitForSeconds(.2f);

        DialogManager.instance.OpenMonologue(debugPhrases, "Omnio", "Fin");
        FindObjectOfType<ThirdPersonMovement>().unblockPlayerMoveInputs();

        yield return new WaitUntil(DialogManagerFinished);
        ChoixCanvas.enabled = true;
        PlayerStatus.instance.GameMenuCursor(true);

        yield return new WaitUntil(() => ChoixCanvas.enabled == false);
        Oeil.target = NouvelleCible;
        yield return new WaitForSeconds(6f);
        Laser.SetActive(true);

        yield return null;
    }

    private IEnumerator MakeMainCameraWork()
    {
        Debug.Log("Make Main Camera Work");
        yield return new WaitForSeconds(.2f);
        mainCamera.SetActive(false);
        yield return new WaitForSeconds(.2f);
        mainCamera.SetActive(true);
    }

    private string[] debugPhrases =
    {
        "Quelle détermination…",
        "Devrais-je extirper le Soleil Rouge de l'existence, délivrant l'univers de sa maudite lueur, au risque d'annihiler les lézards en même temps ?",
        "Ou bien devrions-nous laisser le destin guider les événements, puisque c'est lui qui t'a conduit jusqu'ici ?"
    };

    private string[] Monologue = {
        "Quelle détermination…",
        "Ainsi, c'est toi qui m'as arraché à l'abîme du désespoir. Je te remercie, mon enfant.",
        "Il s'est écoulé une éternité depuis que j'ai contemplé mon univers. J'avais oublié à quel point la vie que j'ai tissée peut être éblouissante.",
        "Ainsi, te voilà une grenouille, peut-être l'ultime héritière de ton peuple… Malgré quelques différences qui te distinguent de tes ancêtres.",
        "Sache-le, je les ai jadis renié. Égarés par leur quête de progrès, les membres de ta race se sont détournés de moi. Consumé par la colère, j'ai cédé à l'impensable.",
        "Les méandres du temps ne peuvent se défaire, ton peuple a forgé un dieu artificiel dont il a perdu le contrôle, et j'ai refusé mon aide. J'ai détourné mon regard, les abandonnant à leur funeste destin.",
        "Quelle déchéance pour un dieu tel que moi. Quand j'ai enfin trouvé la force d'accorder mon pardon aux Grenouilles, il était déjà trop tard.", 
        "La surface d'Amphipolis s'était vidée, laissant place à ces automates corrompus par l'éclat du dieu écarlate.",
        "Même les Poissons, qui m'avaient toujours témoigné respect, avaient succombé. Je n'ai rien fait pour les sauver. Ô, mes enfants, accordez-moi votre pardon.",
        "Alors, j'ai pleuré, laissant mes larmes menacer de submerger l'univers. J'ai voulu engloutir ma propre création, effacer mon échec… Depuis combien d'âges n'avais-je point contemplé la lueur des étoiles ?",
        "Et te voici, ayant entrepris un long périple pour me rejoindre. D'où viens-tu, en vérité ?",
        "La planète Triton ? Ainsi, tu as baptisé ce modeste lopin de terre ? Cette nouvelle m'emplit d'allégresse, qu'enfin quelqu'un l'habite.",
        "Mes racines en toi se dessinent plus clairement, mon enfant. Cependant, dis-moi, quelle est ta quête ici ?",
        "Une comète a heurté ta planète et, depuis, tu t'efforces de retrouver ton foyer ? Ne me dis pas que…",
        "Quelle pitoyable incarnation divine suis-je devenue. J'ai négligé mon propre cosmos, la symphonie cosmique est maintenant en chaos. C'est ma douleur qui t'a guidée ici, mais hélas, je ne peux plus t'assister. ",
        "Tu as franchi le soleil, la frontière sacrée, et tu m'as été d'un secours incommensurable. Toutefois, désormais, tu es prisonnière de cette dimension, tout comme moi. Je te prie de bien vouloir accepter mes humbles excuses, Grenouille.",
        "Désormais, nous sommes les divinités d'un univers désert, condamnées à contempler l'étendue de mon fiasco.",
        "Comment ?! ",
        "La vie perdure en une contrée nommée Solimont, ainsi que sur son satellite Solisède ?!",
        "Je comprends. Quelques Poissons ont survécu. Quelle ravissement.", 
        "Pourtant, il semble que les âmes de Solimont ne soient plus que des ombres d'elles-mêmes, victimes de ce maudit Soleil Rouge. Devrais-je réduire cette étoile en néant ?",
        "Ils périront sans sa lumière, dis-tu ? Je suis las de ce pouvoir, arbitre de vies et de trépas pour les êtres que j'ai créés. L'énergie me manque pour une telle décision.", 
        "Mais maintenant que tu es là, Grenouille, pourrais-tu m'aider à rectifier mes errements passés ?",
        "Devrais-je extirper le Soleil Rouge de l'existence, délivrant l'univers de sa maudite lueur, au risque d'annihiler les lézards en même temps ?",
        "Ou bien devrions-nous laisser le destin guider les événements, puisque c'est lui qui t'a conduit jusqu'ici ?"
    };
}
