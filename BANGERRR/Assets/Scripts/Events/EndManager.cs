using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EndManager : MonoBehaviour
{
    public GameObject mainCamera;
    public CinemachineVirtualCamera amphiCamera;

    public PlanetTag fin;

    [SerializeField] bool playingTheEnd = true;

    public Canvas ChoixCanvas;
    public GameObject Laser;
    public FollowingTargetMore Oeil;
    public GameObject NouvelleCible;
    public GameObject NormalCible;
    public Animator AmphiAnimator;
    public Animator OmnioAnimator;
    public Animator Credits;

    public static int choice = 0;
    public static bool HasEnded = false;

    private void Awake()
    {
        if (ChapterManager.maxChapterIndexDiscoveredByPlayer == 6)
        {
            fin.DiscoverPlanet();
        }
        ChapterManager.currentChapterIndex = 6;
    }

    void Start()
    {
        ChoixCanvas.enabled = false;
        ThirdPersonMovement.ToggleBulleJetpack(false);

        StartCoroutine(nameof(MakeMainCameraWork));
        if (playingTheEnd)
        {
            StartCoroutine(TheEnd());
        }
    }

    IEnumerator TheEnd()
    {
        Debug.Log("The End Coroutine");

        /// FADE OUT BLANC
        yield return FadeToBlack.instance.FadeWhiteEdition(false, .1f);
        yield return new WaitForSeconds(6f);
        
        /// Monologue de début
        /// Joueur libre
        DialogManager.instance.OpenMonologue(Monologue, engMonologue, "Omnio", "Fin");
        FindAnyObjectByType<ThirdPersonMovement>().unblockPlayerMoveInputs();
        yield return new WaitUntil(() => !DialogManager.instance.isItActive());

        /// Présentation du choix
        /// GUI
        ChoixCanvas.enabled = true;
        PlayerStatus.instance.GameMenuCursor(true);
        yield return new WaitUntil(() => ChoixCanvas.enabled == false);
        PlayerStatus.instance.GameMenuCursor(false);

        /// Choix 1 - Destruction Soleil Rouge
        if (choice == 1)
        {
            /// Réaction d'Omnio
            /// Joueur libre
            DialogManager.instance.OpenMonologue(ApresChoix1, engApresChoix1, "Omnio", "Fin");
            FindAnyObjectByType<ThirdPersonMovement>().unblockPlayerMoveInputs();
            yield return new WaitUntil(() => !DialogManager.instance.isItActive());

            /// Animation destruction
            ///     Tir de laser d'Omnio
            ///     * si possible faire que la cam du joueur regarde Omnio
            Oeil.target = NouvelleCible;
            yield return new WaitForSeconds(6f);
            Laser.SetActive(true);
            yield return new WaitForSeconds(3f);

            ///     Switch sur l'animation
            yield return FadeToBlack.instance.Fade(true, .2f);
            amphiCamera.Priority = 100;
            AmphiAnimator.SetTrigger("go");
            yield return FadeToBlack.instance.Fade(false, .5f);

            ///     Bruit de l'impact
            ///     * yet to be implemented
            yield return new WaitUntil(() => AmphiAnimScript.laserHasHitSoleilRouge);
            Debug.Log("Laser Hit");
            Oeil.target = NormalCible;
            Laser.SetActive(false);
            yield return new WaitUntil(() => AmphiAnimScript.finito);

            /// Retour sur Omnio
            yield return FadeToBlack.instance.Fade(true, .5f);
            amphiCamera.Priority = 0;
            yield return FadeToBlack.instance.Fade(false, .2f);

            /// Dernière tirade d'Omnio
            /// Joueur libre
            DialogManager.instance.OpenMonologue(ApresChoix1ApresAnimation, engApresChoix1ApresAnimation, "Omnio", "Fin");
            FindAnyObjectByType<ThirdPersonMovement>().unblockPlayerMoveInputs();
            yield return new WaitUntil(() => !DialogManager.instance.isItActive());

            /// Fin de la fin
            HasEnded = true;
        }
        else
        {
            /// Réaction d'Omnio
            /// Joueur libre
            DialogManager.instance.OpenMonologue(ApresChoix2, engApresChoix2, "Omnio", "Fin");
            FindAnyObjectByType<ThirdPersonMovement>().unblockPlayerMoveInputs();
            yield return new WaitUntil(() => !DialogManager.instance.isItActive());

            /// Fin de la fin
            HasEnded = true;
        }

        /// Omnio Out
        yield return FadeToBlack.instance.FadeWhiteEdition(true, 20f);
        OmnioAnimator.SetBool("OmnioOut", true);
        yield return FadeToBlack.instance.FadeWhiteEdition(false, 2f);

        Debug.Log("HasEnded = " + HasEnded);

        /// Credits
        Credits.SetTrigger("go");
        
        yield return null;
    }

    private IEnumerator MakeMainCameraWork()
    {
        Debug.Log("Make Main Camera Work");
        yield return new WaitForEndOfFrame();
        mainCamera.SetActive(false);
        yield return new WaitForEndOfFrame();
        mainCamera.SetActive(true);
    }

    private string[] debugPhrases =
    {
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
        "Devrais-je extirper le Soleil Rouge de l'existence, délivrant l'univers de sa maudite lueur, au risque d'annihiler les Lézards en même temps ?",
        "Ou bien devrions-nous laisser le destin guider les événements, puisque c'est lui qui t'a conduit jusqu'ici ?"
    };

    private string[] ApresChoix1 = 
    {
        "Si tel est ton choix, je vais le détruire. Contemple le pouvoir d’un vrai dieu face à cet imposture.",
        "Adieu, Lézards."
    };

    private string[] ApresChoix1ApresAnimation =
    {
        "Merci infiniment, Grenouille. Tu sembles être un dieu plus que prometteur, me voilà rassuré.", 
        "J’aimerais te demander un dernier service, si tu le veux bien.",
        "Pourrais-tu prendre ma place, mon enfant ? J'ai créé beaucoup trop de malheur dans cet univers. J'aimerais maintenant aspirer à retrouver la paix que j'ai perdue jadis.",
        "Tu t'en sortiras parfaitement, ne doute pas de toi. Tu as eu le courage et la force de venir jusqu'ici, tu seras un Dieu parfait.",
        "Chéris cet univers qui t'a vu naître, et tout se passera bien.",
        "Adieu, mon ami. Merci…"
    };

    private string[] ApresChoix2 =
    {
        "Tu préfères ne rien faire ? Je comprend, ce peuple a déja beaucoup souffert. Laissons lui le repos qu’il mérite.",
        "J’aimerais néanmoins te demander un dernier service, si tu le veux bien.",
        "Pourrais-tu prendre ma place, mon enfant ? J'ai créé beaucoup trop de malheur dans cet univers. J'aimerais maintenant aspirer à retrouver la paix que j'ai perdue jadis.",
        "Tu t'en sortiras parfaitement, ne doute pas de toi. Tu as eu le courage et la force de venir jusqu'ici, tu seras un Dieu parfait.",
        "Chéris cet univers qui t'a vu naître, et tout se passera bien.",
        "Adieu, mon ami."
    };

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ENGLISH 

    private string[] engMonologue = {
        "Such determination...",
        "So, it's you who has pulled me from the abyss of despair. I thank you, my child.",
        "An eternity has passed since I contemplated my universe. I had forgotten how dazzling the life I've woven can be.",
        "So, here you are, a frog, perhaps the ultimate heir of your people... Despite some differences that distinguish you from your ancestors.",
        "Know this, I once disowned them. Lost in their pursuit of progress, the members of your race turned away from me. Consumed by anger, I gave in to the unthinkable.",
        "The twists of time cannot undo it; your people forged an artificial god they lost control of, and I refused my help. I averted my gaze, abandoning them to their grim fate.",
        "What a downfall for a god like me. When I finally found the strength to grant my forgiveness to the Frogs, it was already too late.",
        "The surface of Amphipolis had emptied, giving way to these automatons corrupted by the brilliance of the scarlet god.",
        "Even the Fishes, who had always shown me respect, had succumbed. I did nothing to save them. Oh, my children, grant me your forgiveness.",
        "So, I wept, letting my tears threaten to engulf the universe. I wanted to swallow my own creation, erase my failure... For how many ages had I not gazed upon the stars' light?",
        "And here you are, having undertaken a long journey to reach me. Where do you truly come from?",
        "Planet Triton? Thus, you have named this humble plot of land? This fills me with joy, knowing that finally someone inhabits it.",
        "My roots in you are becoming clearer, my child. However, tell me, what is your quest here?",
        "A comet struck your planet and since then, you've been striving to find your home? Don't tell me that...",
        "What a pitiful divine incarnation I have become. I neglected my own universe, the cosmic symphony is now in chaos. It is my pain that guided you here, but alas, I can no longer assist you.",
        "You crossed the sun, the sacred boundary, and you have been of immeasurable help to me. However, now you are trapped in this dimension, just like me. I beg you to please accept my humble apologies, Frog.",
        "Now, we are the deities of a deserted universe, condemned to contemplate the extent of my failure.",
        "What?!",
        "Life endures in a land called Solimont, as well as on its satellite, Solisède?!",
        "I understand. Some Fishes have survived. What a delight.",
        "Yet, it seems the souls of Solimont are but shadows of themselves, victims of that cursed Red Sun. Should I reduce that star to nothingness?",
        "They will perish without its light, you say? I am weary of this power, arbiter of lives and deaths for the beings I created. I lack the energy for such a decision.",
        "But now that you are here, Frog, could you help me rectify my past mistakes?",
        "Should I extract the Red Sun from existence, freeing the universe from its accursed glow, risking the annihilation of the Lizards in the process?",
        "Or should we let destiny guide events, since it led you here?"
    };


    private string[] engApresChoix1 = {
        "If that is your choice, I will destroy it. Witness the power of a true god against this impostor.",
        "Farewell, Lizards."
    };

    private string[] engApresChoix1ApresAnimation =
    {
        "Thank you immensely, Frog. You seem to be a more than promising god, and I am reassured now.",
        "I would like to ask you one last favor, if you're willing.",
        "Could you take my place, my child? I have caused far too much sorrow in this universe. I would now aspire to regain the peace I once lost.",
        "You will do wonderfully, do not doubt yourself. You have had the courage and strength to come this far, you will be a perfect God.",
        "Cherish this universe that witnessed your birth, and everything will be fine.",
        "Farewell, my friend. Thank you..."
    };

    private string[] engApresChoix2 =
    {
        "You prefer to do nothing? I understand, this people have already suffered a great deal. Let us give them the rest they deserve.",
        "However, I would still like to ask you one last favor, if you're willing.",
        "Could you take my place, my child? I have caused far too much sorrow in this universe. I would now aspire to regain the peace I once lost.",
        "You will do wonderfully, do not doubt yourself. You have had the courage and strength to come this far, you will be a perfect God.",
        "Cherish this universe that witnessed your birth, and everything will be fine.",
        "Farewell, my friend."
    };
}
