using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlanetNameDisplay : MonoBehaviour
{
    public Camera cosmoGuideCamera;
    public GameObject planetNamePrefab;
    public float growFactor;
    public Vector2 nameOffset;
    public Vector2 offset;
    public GameObject system;

    private GameObject[] planets;
    private GameObject[] names;

    private void Start()
    {
        // Recherche tous les objets nommés "Planet" dans la scène
        planets = GameObject.FindGameObjectsWithTag("Planet");
        names = new GameObject[planets.Length]; // Initialise le tableau des noms avec la taille correspondante
        //Debug.Log(planets.Length);

        // Crée un nouvel objet vide pour contenir les noms des planètes
        GameObject namesContainer = new GameObject("Names");
        namesContainer.transform.SetParent(this.transform); // Définit le canvas comme parent
        
        for (int i = 0; i < planets.Length; i++)
        // for (int i = 0; i < 2; i++)
        {
            names[i] = (GameObject)Instantiate(planetNamePrefab, namesContainer.transform);
            names[i].name = "TEXT-"+planets[i].transform.parent.name;
            names[i].GetComponentInChildren<TextMeshProUGUI>().text = planets[i].transform.parent.name;

            // Récupère le MeshRenderer de la planète
            MeshRenderer planetRenderer = planets[i].GetComponent<MeshRenderer>();
            // Copie le matériau du MeshRenderer sur le TextMeshProUGUI
            names[i].GetComponentInChildren<TextMeshProUGUI>().color = planetRenderer.material.color;
        }
        // Debug.Log(names);
    }

    private void Update()
    {
        int i = 0;
        foreach (GameObject planet in planets)
        {
            names[i].SetActive(system.GetComponent<OpenCosmoGuide>().CosmoGuideIsOpen);
            if(system.GetComponent<OpenCosmoGuide>().CosmoGuideIsOpen)
            {
                // Récupère les coordonnées mondiales de chaque planète
                Vector3 worldPosition = planet.transform.position;

                // Convertit les coordonnées mondiales en coordonnées de l'écran de la caméra
                Vector3 screenPosition = cosmoGuideCamera.WorldToScreenPoint(worldPosition);

                // Récupère les dimensions de l'écran
                Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

                // Recentre les coordonnées par rapport au centre de l'écran
                Vector2 planetPosOn2DScreen = growFactor*(new Vector2(screenPosition.x, screenPosition.y))+offset;
                Vector3 worldPositionNormalize = worldPosition.normalized;
                Vector2 direction = new Vector2(worldPositionNormalize.z,worldPositionNormalize.x);
                Vector2 nameOffsetAbs = new Vector2(Mathf.Abs(nameOffset.x),Mathf.Abs(nameOffset.y));
                // Debug.Log(direction);
                // Debug.Log(nameOffsetAbs);
                Vector2 namePosOn2DScreen = planetPosOn2DScreen + direction * nameOffset;
                // Debug.Log(namePosOn2DScreen);

                // Crée un texte à chaque coordonnée
                names[i].GetComponentInChildren<TextMeshProUGUI>().transform.position = new Vector3(namePosOn2DScreen.x, namePosOn2DScreen.y, 0);
            }
            i++;
        }
    }
}
