using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;


public class pixel_effect : MonoBehaviour
{
    public int environment_layer_id = 3;
    public int fusee_layer_id = 8;
    public int player_layer_id = 6;
    private List<Renderer> all_scene_renderers;
    private List<Renderer> list_environment_renderers;

    private List<Material> list_environment_materials;

    private List<Renderer> list_player_renderers;



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

    public Shader overide_shader;

    void Start()
    {
        RenderPipelineManager.beginCameraRendering += OnPreRenderCallback;
        RenderPipelineManager.endCameraRendering += OnPostRenderCallback;

        all_scene_renderers = new List<Renderer>();
        list_environment_materials = new List<Material>();
        list_environment_renderers = new List<Renderer>();
        list_player_renderers = new List<Renderer>();
        list_original_mat_player = new List<Material>();
        list_original_mat_env = new List<Material>();

        white = new Material(overide_shader);
        white.SetFloat("_is_white", 1f);
        black = new Material(overide_shader);
        black.SetFloat("_is_white", 0f);

        setup_cams();

        compute_list_environment_and_player();
        compute_original_mat();
    }

    void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= OnPreRenderCallback;
        RenderPipelineManager.endCameraRendering -= OnPostRenderCallback;
        list_environment_renderers.Clear();
        list_player_renderers.Clear();
        list_original_mat_player.Clear();
        list_original_mat_env.Clear();
    }


    // Update is called once per frame
    void Update()
    {
        compute_list_environment_and_player();
        compute_original_mat();
    }

    void compute_list_environment_and_player(){
        all_scene_renderers.Clear();
        all_scene_renderers.AddRange(FindObjectsByType<Renderer>(FindObjectsSortMode.None));

        list_environment_renderers.Clear();

        list_player_renderers.Clear();

        foreach (Renderer r in all_scene_renderers)
        {
            if(r.gameObject.layer == environment_layer_id || r.gameObject.layer == fusee_layer_id)
            {
                list_environment_renderers.Add(r);
            }
            else if(r.gameObject.layer == player_layer_id)
            {
                list_player_renderers.Add(r);
            }
        }
    }

    void compute_original_mat(){
        list_original_mat_player.Clear();
        list_original_mat_env.Clear();


        foreach (Renderer r in list_player_renderers){
            list_original_mat_player.Add(r.material);
        }


        foreach(Renderer r in list_environment_renderers){
            list_original_mat_env.Add(r.material);
        }
    }

    void reset_materials(){
        for(int i = 0; i<list_original_mat_player.Count; i++){

            list_player_renderers[i].material = list_original_mat_player[i];
        }

        for(int i = 0; i<list_environment_renderers.Count; i++){
            list_environment_renderers[i].material = list_original_mat_env[i];
        }
    }


    void change_materials(){
        //Debug.Log("changement");
        // for the player => set to full white
        foreach(Renderer mr_player in list_player_renderers){
            mr_player.material = white;
            if(mr_player.gameObject.TryGetComponent(out SpriteRenderer spr))
            {
                mr_player.material.SetTexture("_optionnal_texture",spr.sprite.texture);
                mr_player.material.SetFloat("_has_texture",1f);
            }
        }
        Resources.UnloadUnusedAssets();
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

        // set the depth
        players_camera.depthTextureMode = DepthTextureMode.Depth;
        environment_camera.depthTextureMode = DepthTextureMode.Depth;


        // Set the target textures
        mask_camera.targetTexture = texture_mask;
        players_camera.targetTexture = texture_players;
        environment_camera.targetTexture = texture_environment;
        all_camera.targetTexture = texture_all;




        // set the player masks for the player and the environment
        players_camera.cullingMask = (1<<player_layer_id);
        environment_camera.cullingMask = (1<<environment_layer_id) + (1<<fusee_layer_id);
        mask_camera.cullingMask = (1<<player_layer_id) + (1<<environment_layer_id);

        // black background for the mask and the player only
        mask_camera.clearFlags = CameraClearFlags.Depth;
        // mask_camera.backgroundColor = Color.white;    

        players_camera.clearFlags = CameraClearFlags.SolidColor;
        players_camera.backgroundColor = new Color (0,0,0,0);
        players_camera.depthTextureMode = DepthTextureMode.Depth;
        


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
            //Debug.Log("avant rendu de la mask camera");
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
