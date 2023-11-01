using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneLoadManager : MonoBehaviour
{
    public Rigidbody player;
    public GameObject Larme;
    public GameObject FakeLarme;

    [Header("Start Positions")]
    public Transform TritonStart;
    public Transform EdStart;
    public Transform SolisedeStart;
    public Transform SolimontStart;
    public Transform AmpStart;
    public Transform OeilStart;

    [Header("Planet Tags")]
    public PlanetTag TritonTag;
    public PlanetTag EdTag;
    public PlanetTag SolisedeTag;
    public PlanetTag SolimontTag;
    public PlanetTag AmpTag;
    public PlanetTag OeilTag;

    Transform[] Starts;
    PlanetTag[] Tags;

    void Awake()
    {
        Starts = new Transform[] { TritonStart, EdStart , SolisedeStart, SolimontStart, AmpStart, OeilStart };
        Tags = new PlanetTag[] { TritonTag, EdTag , SolisedeTag, SolimontTag, AmpTag, OeilTag };

        //int index = GlobalVariables.Get<int>("planetIndex");
        int index = 0;
        Debug.Log("Forcefully starting with index " + index);

        // Solis�de et apr�s
        // [X] Donner le Cosmoguide
        // [ ] D�sactiver les fleurbulles (performance)
        if (index >= 2)
        {
            PlayerStatus.instance.giveCosmoguide();
        }
        // Amphipolis
        // [X] Faire appara�tre la fausse larme
        // [ ] D�sactiver les poissons (performance)
        // [ ] D�sactiver les l�zards (performance)
        if (index == 4)
        {
            Larme.SetActive(false);
            FakeLarme.SetActive(true);
        }
        // Oeil
        // [ ] D�sactiver toutes les autres plan�tes (performance)

        HandlePlanetTags(index);

        player.rotation = Starts[index].rotation;
        player.position = Starts[index].position;

        // rien � voir
        GlobalVariables.Set("interactRange", 2f);
    }

    void HandlePlanetTags(int index)
    {
        while (index >= 0)
        {
            Tags[index].ActivateObjects();
            Tags[index].DeactivateObjects();
            Tags[index].DiscoverPlanet();
            index--;
        }
    }
}
