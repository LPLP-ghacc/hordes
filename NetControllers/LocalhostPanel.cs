using Assets._NETWORK;
using Photon.Pun;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

public class LocalhostPanel : MonoBehaviour
{
    public GameObject[] netModels;
    public bool isFireDigger = false;
    public Vector2 screenResolution;
    public AudioMixer mixer;
    public NetworkingItemController itemController;

    public List<GameObject> netItemPrefabs = new List<GameObject>();

    private void Awake()
    {
        DontDestroyOnLoad(this);

        gameObject.name = PlayersFieds.localhostPanel;

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged; ;

        netItemPrefabs = PopulateNetworkedPrefabs();
    }

    private void SceneManager_activeSceneChanged(Scene current, Scene next)
    {
        //Debug.Log($"Last scene [{current.name}] was replaced by [{next.name}]");
    }

    private void SceneManager_sceneUnloaded(Scene scene)
    {
        //Debug.Log($"scene: {scene} was unloaded.");
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        //Debug.Log($"scene: {scene} was loaded.");
    }

    protected void Start()
    {
        DJ();
    }

    protected void Update()
    {
        screenResolution = new Vector2(Screen.width, Screen.height);
    }

    private void DJ()
    {
        string[] parameters = new string[]
        {
            "Master",
            "Walk",
            "Microphone",
            "CurrentPlayerVoice",
            "Buttons"
        };

        foreach (var parameter in parameters)
        {
            var value = (PlayerPrefs.GetFloat(parameter));

            mixer.SetFloat(parameter, value);
        }
    }

    private static List<GameObject> PopulateNetworkedPrefabs()
    {
        GameObject[] result = Resources.LoadAll<GameObject>("");

        List<GameObject> _networkedPrefabs = new List<GameObject>();

        for (int i = 0; i < result.Length; i++)
        {
            if (result[i].GetComponent<PhotonView>() != null)
            {
                _networkedPrefabs.Add(result[i]);
            }
        }
        return _networkedPrefabs;
    }
}

public static class VolumeSettings
{
    public static Volume GetThisVolume()
    {
        if (GameObject.Find("Sky and Fog Volume").GetComponent<Volume>())
            return GameObject.Find("Sky and Fog Volume").GetComponent<Volume>();
        else
            return null;
    }

    public static VolumeProfile GetProfile()
    {
        if (GetThisVolume())
            return GetThisVolume().profile;
        else
            return null;
    }

    public static Fog GetFog(VolumeProfile profile)
    {
        profile.TryGet(out Fog fog);

        return fog;
    }

    public static DepthOfField GetDepth(VolumeProfile profile)
    {
        profile.TryGet(out DepthOfField comp);

        return comp;
    }

    public static LensDistortion GetLens(VolumeProfile profile)
    {
        profile.TryGet(out LensDistortion comp);

        return comp;
    }

    public static MotionBlur GetBlur(VolumeProfile profile)
    {
        profile.TryGet(out MotionBlur comp);

        return comp;
    }

    public static Vignette GetVignette(VolumeProfile profile)
    {
        profile.TryGet(out Vignette comp);

        return comp;
    }

    public static AmbientOcclusion GetAmbientOcclusion(VolumeProfile profile)
    {
        profile.TryGet(out AmbientOcclusion comp);

        return comp;
    }
}

