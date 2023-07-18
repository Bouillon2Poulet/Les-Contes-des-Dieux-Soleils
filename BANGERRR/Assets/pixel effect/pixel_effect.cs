using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


// [ExecuteAlways]
public class pixel_effect : MonoBehaviour
{
    // Start is called before the first frame update
    public int environment_layer_id = 7;
    public int player_layer_id = 6;
    private object[] all_scene_objects;
    private List<GameObject> list_environment;
    private List<MeshRenderer> list_environment_renderers;


    private List<GameObject> list_player;
    private List<SpriteRenderer> list_player_renderers;



    private List<Material> list_original_mat_env;
    private List<Material> list_original_mat_player;

    private Material white;
    private Material black;

    private Camera mask_camera = new Camera();
    private Camera players_camera = new Camera();
    private Camera environment_camera = new Camera();
    private Camera all_camera = new Camera();

    public RenderTexture texture_mask;
    public RenderTexture texture_players;
    public RenderTexture texture_environment;

    public RenderTexture texture_all;


    // public Material pixel_effect_material;

    // private quad_drawer drawer;


    void Start()
    {
        compute_list_environment_and_player();
        compute_original_mat();

        white = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        white.SetColor("_BaseColor",Color.white);
        black = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        black.SetColor("_BaseColor",Color.black);

        setup_cams();


        RenderPipelineManager.beginCameraRendering += OnPreRenderCallback;
        RenderPipelineManager.endCameraRendering += OnPostRenderCallback;


    }

    void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= OnPreRenderCallback;
        RenderPipelineManager.endCameraRendering -= OnPostRenderCallback;
    }


    // Update is called once per frame
    void Update()
    {
        compute_list_environment_and_player();
        compute_original_mat();
        Debug.Log(list_original_mat_player.Count + "nb de mat");
        Debug.Log(list_player.Count + "nb de players");
    }

    void compute_list_environment_and_player(){
        all_scene_objects =  GameObject.FindObjectsOfType<GameObject>();
        list_environment = new List<GameObject>();
        list_environment_renderers = new List<MeshRenderer>();

        list_player = new List<GameObject>();
        list_player_renderers = new List<SpriteRenderer>();
        foreach(object o in all_scene_objects){
            GameObject go = (GameObject) o;
            if(go.layer == environment_layer_id) {
                list_environment.Add(go);
                list_environment_renderers.Add(go.GetComponent<MeshRenderer>());
            }
            if(go.layer == player_layer_id) {
                list_player.Add(go);
                list_player_renderers.Add(go.GetComponent<SpriteRenderer>());
            }
        }
    }


    void compute_original_mat(){
        list_original_mat_player = new List<Material>();
        list_original_mat_env = new List<Material>();


        foreach(GameObject go_player in list_player){
            list_original_mat_player.Add(go_player.GetComponent<SpriteRenderer>().material);
        }


        foreach(GameObject go_env in list_environment){
            list_original_mat_env.Add(go_env.GetComponent<MeshRenderer>().material);
        }
    }

    void reset_materials(){
        for(int i = 0; i<list_original_mat_player.Count; i++){
            list_player_renderers[i].material = list_original_mat_player[i];
        }

        for(int i = 0; i<list_environment.Count; i++){
            list_environment_renderers[i].material = list_original_mat_env[i];
        }
    }


    void change_materials(){
        // for the player => set to full white
        foreach(SpriteRenderer mr_player in list_player_renderers){
            mr_player.material = white;
        }

        // for the environment => set to full black
        foreach(MeshRenderer mr_env in list_environment_renderers){
            mr_env.material= black;
        }
    }


    void setup_cams()
    {
        // Create new camera objects
        mask_camera = new GameObject("MaskCamera").AddComponent<Camera>();
        players_camera = new GameObject("PlayersCamera").AddComponent<Camera>();
        environment_camera = new GameObject("EnvironmentCamera").AddComponent<Camera>();
        all_camera = new GameObject("AllCamera").AddComponent<Camera>();

        // Copy settings from the main camera
        mask_camera.CopyFrom(Camera.main);
        players_camera.CopyFrom(Camera.main);
        environment_camera.CopyFrom(Camera.main);
        all_camera.CopyFrom(Camera.main);

        // Set the target textures
        mask_camera.targetTexture = texture_mask;
        players_camera.targetTexture = texture_players;
        environment_camera.targetTexture = texture_environment;
        all_camera.targetTexture = texture_all;




        // set the player masks for the player and the environment
        players_camera.cullingMask = (1<<player_layer_id);
        environment_camera.cullingMask = (1<<environment_layer_id);


        // black background for the mask and the player only
        mask_camera.clearFlags = CameraClearFlags.SolidColor;
        mask_camera.backgroundColor = Color.black;

        players_camera.clearFlags = CameraClearFlags.SolidColor;
        players_camera.backgroundColor = Color.black;


        // set them as children
        mask_camera.gameObject.transform.SetParent(Camera.main.gameObject.transform);
        environment_camera.gameObject.transform.SetParent(Camera.main.gameObject.transform);
        players_camera.gameObject.transform.SetParent(Camera.main.gameObject.transform);
        all_camera.gameObject.transform.SetParent(Camera.main.gameObject.transform);
    }


    public void OnPreRenderCallback(ScriptableRenderContext context, Camera camera)
    {
        if (camera == mask_camera)
        {
            // Actions to perform before rendering mask_camera
            change_materials();
        }
    }

    public void OnPostRenderCallback(ScriptableRenderContext context, Camera camera)
    {
        if (camera == mask_camera)
        {
            // Actions to perform after rendering mask_camera
            reset_materials();
        }
    }
}
