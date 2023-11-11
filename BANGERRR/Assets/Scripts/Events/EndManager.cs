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
        "Quelle d�termination�",
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
}
