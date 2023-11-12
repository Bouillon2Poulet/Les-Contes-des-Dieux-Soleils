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
        
        /// Monologue de d�but
        /// Joueur libre
        DialogManager.instance.OpenMonologue(Monologue, "Omnio", "Fin");
        FindObjectOfType<ThirdPersonMovement>().unblockPlayerMoveInputs();
        yield return new WaitUntil(() => !DialogManager.instance.isItActive());

        /// Pr�sentation du choix
        /// GUI
        ChoixCanvas.enabled = true;
        PlayerStatus.instance.GameMenuCursor(true);
        yield return new WaitUntil(() => ChoixCanvas.enabled == false);
        PlayerStatus.instance.GameMenuCursor(false);

        /// Choix 1 - Destruction Soleil Rouge
        if (choice == 1)
        {
            /// R�action d'Omnio
            /// Joueur libre
            DialogManager.instance.OpenMonologue(ApresChoix1, "Omnio", "Fin");
            FindObjectOfType<ThirdPersonMovement>().unblockPlayerMoveInputs();
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

            /// Derni�re tirade d'Omnio
            /// Joueur libre
            DialogManager.instance.OpenMonologue(ApresChoix1ApresAnimation, "Omnio", "Fin");
            FindObjectOfType<ThirdPersonMovement>().unblockPlayerMoveInputs();
            yield return new WaitUntil(() => !DialogManager.instance.isItActive());

            /// Fin de la fin
            HasEnded = true;
        }
        else
        {
            /// R�action d'Omnio
            /// Joueur libre
            DialogManager.instance.OpenMonologue(ApresChoix2, "Omnio", "Fin");
            FindObjectOfType<ThirdPersonMovement>().unblockPlayerMoveInputs();
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
        "Devrais-je extirper le Soleil Rouge de l'existence, d�livrant l'univers de sa maudite lueur, au risque d'annihiler les l�zards en m�me temps ?",
        "Ou bien devrions-nous laisser le destin guider les �v�nements, puisque c'est lui qui t'a conduit jusqu'ici ?"
    };

    private string[] Monologue = {
        "Quelle d�termination�",
        "Ainsi, c'est toi qui m'as arrach� � l'ab�me du d�sespoir. Je te remercie, mon enfant.",
        "Il s'est �coul� une �ternit� depuis que j'ai contempl� mon univers. J'avais oubli� � quel point la vie que j'ai tiss�e peut �tre �blouissante.",
        "Ainsi, te voil� une grenouille, peut-�tre l'ultime h�riti�re de ton peuple� Malgr� quelques diff�rences qui te distinguent de tes anc�tres.",
        "Sache-le, je les ai jadis reni�. �gar�s par leur qu�te de progr�s, les membres de ta race se sont d�tourn�s de moi. Consum� par la col�re, j'ai c�d� � l'impensable.",
        "Les m�andres du temps ne peuvent se d�faire, ton peuple a forg� un dieu artificiel dont il a perdu le contr�le, et j'ai refus� mon aide. J'ai d�tourn� mon regard, les abandonnant � leur funeste destin.",
        "Quelle d�ch�ance pour un dieu tel que moi. Quand j'ai enfin trouv� la force d'accorder mon pardon aux Grenouilles, il �tait d�j� trop tard.", 
        "La surface d'Amphipolis s'�tait vid�e, laissant place � ces automates corrompus par l'�clat du dieu �carlate.",
        "M�me les Poissons, qui m'avaient toujours t�moign� respect, avaient succomb�. Je n'ai rien fait pour les sauver. �, mes enfants, accordez-moi votre pardon.",
        "Alors, j'ai pleur�, laissant mes larmes menacer de submerger l'univers. J'ai voulu engloutir ma propre cr�ation, effacer mon �chec� Depuis combien d'�ges n'avais-je point contempl� la lueur des �toiles ?",
        "Et te voici, ayant entrepris un long p�riple pour me rejoindre. D'o� viens-tu, en v�rit� ?",
        "La plan�te Triton ? Ainsi, tu as baptis� ce modeste lopin de terre ? Cette nouvelle m'emplit d'all�gresse, qu'enfin quelqu'un l'habite.",
        "Mes racines en toi se dessinent plus clairement, mon enfant. Cependant, dis-moi, quelle est ta qu�te ici ?",
        "Une com�te a heurt� ta plan�te et, depuis, tu t'efforces de retrouver ton foyer ? Ne me dis pas que�",
        "Quelle pitoyable incarnation divine suis-je devenue. J'ai n�glig� mon propre cosmos, la symphonie cosmique est maintenant en chaos. C'est ma douleur qui t'a guid�e ici, mais h�las, je ne peux plus t'assister. ",
        "Tu as franchi le soleil, la fronti�re sacr�e, et tu m'as �t� d'un secours incommensurable. Toutefois, d�sormais, tu es prisonni�re de cette dimension, tout comme moi. Je te prie de bien vouloir accepter mes humbles excuses, Grenouille.",
        "D�sormais, nous sommes les divinit�s d'un univers d�sert, condamn�es � contempler l'�tendue de mon fiasco.",
        "Comment ?! ",
        "La vie perdure en une contr�e nomm�e Solimont, ainsi que sur son satellite Solis�de ?!",
        "Je comprends. Quelques Poissons ont surv�cu. Quelle ravissement.", 
        "Pourtant, il semble que les �mes de Solimont ne soient plus que des ombres d'elles-m�mes, victimes de ce maudit Soleil Rouge. Devrais-je r�duire cette �toile en n�ant ?",
        "Ils p�riront sans sa lumi�re, dis-tu ? Je suis las de ce pouvoir, arbitre de vies et de tr�pas pour les �tres que j'ai cr��s. L'�nergie me manque pour une telle d�cision.", 
        "Mais maintenant que tu es l�, Grenouille, pourrais-tu m'aider � rectifier mes errements pass�s ?",
        "Devrais-je extirper le Soleil Rouge de l'existence, d�livrant l'univers de sa maudite lueur, au risque d'annihiler les l�zards en m�me temps ?",
        "Ou bien devrions-nous laisser le destin guider les �v�nements, puisque c'est lui qui t'a conduit jusqu'ici ?"
    };

    private string[] ApresChoix1 = 
    {
        "Si tel est ton choix, je vais le d�truire. Contemple le pouvoir d�un vrai dieu face � cet imposture.",
        "Adieu l�zards."
    };

    private string[] ApresChoix1ApresAnimation =
    {
        "Merci infiniment, Grenouille. Tu sembles �tre un dieu plus que prometteur, me voil� rassur�.", 
        "J�aimerais te demander un dernier service, si tu le veux bien.",
        "Pourrais-tu prendre ma place, mon enfant ? J'ai cr�� beaucoup trop de malheur dans cet univers. J'aimerais maintenant aspirer � retrouver la paix que j'ai perdue jadis.",
        "Tu t'en sortiras parfaitement, ne doute pas de toi. Tu as eu le courage et la force de venir jusqu'ici, tu seras un Dieu parfait.",
        "Ch�ris cet univers qui t'a vu na�tre, et tout se passera bien.",
        "Adieu, mon ami. Merci�"
    };

    private string[] ApresChoix2 =
    {
        "Tu pr�f�res ne rien faire ? Je comprend, ce peuple a d�ja beaucoup souffert. Laissons lui le repos qu�il m�rite.",
        "J�aimerais n�anmoins te demander un dernier service, si tu le veux bien.",
        "Pourrais-tu prendre ma place, mon enfant ? J'ai cr�� beaucoup trop de malheur dans cet univers. J'aimerais maintenant aspirer � retrouver la paix que j'ai perdue jadis.",
        "Tu t'en sortiras parfaitement, ne doute pas de toi. Tu as eu le courage et la force de venir jusqu'ici, tu seras un Dieu parfait.",
        "Ch�ris cet univers qui t'a vu na�tre, et tout se passera bien.",
        "Adieu, mon ami."
    };
}
