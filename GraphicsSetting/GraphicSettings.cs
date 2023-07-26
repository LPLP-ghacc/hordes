using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicSettings : MonoBehaviour
{
    [Header("Specify originally")]
    public GameObject menu;

    [Space]
    public Toggle fogToggle;
    public Toggle depthOfFieldToggle;
    public Toggle lensDistortionToggle;
    public Toggle motionBlurToggle;
    public Toggle vignetteToggle;
    public Toggle ambientOcclusionToggle;

    public TMP_Dropdown windowDropdown;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown graphicsDropdown;
    public TMP_Dropdown vSyncDropdown;

    private int resolutionMode;
    private int windowMode;
    private int graphicsMode;
    private int vsyncMode;
    private int anisotropicFilteringMode;

    //private void Start()
    //{
    //    OnStartLoadPlayerPrefs();
    //
    //    PostProcessingControl(true, null);
    //}

    //private void OnStartLoadPlayerPrefs()
    //{
    //    if (!PlayerPrefs.HasKey("HORDES_GRAPHICS_SAVE"))
    //        return;
    //
    //    var save = CurrentSave.GetSave();
    //
    //    resolutionMode = save.savedResolution;
    //    windowMode = save.savedFullscreen;
    //    graphicsMode = save.savedGraphics;
    //    vsyncMode = save.savedVSync;
    //    anisotropicFilteringMode = save.savedAnisotropicFiltering;
    //}

    //private void PostProcessingControl(bool value, Save save)
    //{
    //    if (value)
    //    {
    //        //from start load prefs
    //        var localsave = CurrentSave.GetSave();
    //
    //        fogToggle.isOn = localsave.savedFog;
    //        depthOfFieldToggle.isOn = localsave.savedDepthOfField;
    //        ambientOcclusionToggle.isOn = localsave.savedAmbientOcclusion;
    //        lensDistortionToggle.isOn = localsave.savedLensDistortion;
    //        motionBlurToggle.isOn = localsave.savedMotionBlur;
    //        vignetteToggle.isOn = localsave.savedVignette;
    //
    //        PostProcActive();
    //    }
    //    else
    //    {
    //        PostProcActive();
    //
    //        //to save
    //        save.savedFog = fogToggle.isOn;
    //        save.savedDepthOfField = depthOfFieldToggle.isOn;
    //        save.savedAmbientOcclusion = ambientOcclusionToggle.isOn;
    //        save.savedLensDistortion = lensDistortionToggle.isOn;
    //        save.savedMotionBlur = motionBlurToggle.isOn;
    //        save.savedVignette = vignetteToggle.isOn;
    //    }
    //}

    private void PostProcActive()
    {
        var profile = VolumeSettings.GetProfile();

        try
        {
            VolumeSettings.GetFog(profile).active = fogToggle.isOn;
            VolumeSettings.GetDepth(profile).active = depthOfFieldToggle.isOn;
            VolumeSettings.GetAmbientOcclusion(profile).active = ambientOcclusionToggle.isOn;
            VolumeSettings.GetLens(profile).active = lensDistortionToggle.isOn;
            VolumeSettings.GetBlur(profile).active = motionBlurToggle.isOn;
            VolumeSettings.GetVignette(profile).active = vignetteToggle.isOn;
        }
        catch
        {

        }

    }

    //public void Save()
    //{
    //    Save save = CurrentSave.GetSave();
    //
    //    save.savedResolution = resolutionMode;
    //    save.savedVSync = vsyncMode;
    //    save.savedGraphics = graphicsMode;
    //    save.savedFullscreen = windowMode;
    //    save.savedAnisotropicFiltering = anisotropicFilteringMode;
    //
    //    PostProcessingControl(false, save);
    //
    //    GraphicsSettingsSaving.SaveData(save);
    //
    //    var currentResolution = Screen.resolutions[resolutionMode];
    //    Screen.SetResolution(currentResolution.width, currentResolution.height, (windowMode == 0) ? false : true, currentResolution.refreshRate);
    //    QualitySettings.SetQualityLevel(graphicsMode);
    //    QualitySettings.vSyncCount = vsyncMode;
    //    QualitySettings.anisotropicFiltering = (anisotropicFilteringMode == 0) ? AnisotropicFiltering.Disable : AnisotropicFiltering.ForceEnable;
    //
    //    menu.SetActive(true);
    //}

    #region OnDropdown

    public void OnResolutionChangeValue(GameObject dropdown)
    {
        resolutionMode = dropdown.GetComponent<TMP_Dropdown>().value;
    }

    public void OnGraphicsChangeValue(GameObject dropdown)
    {
        graphicsMode = dropdown.GetComponent<TMP_Dropdown>().value;
    }

    public void OnScreenChangeValue(GameObject dropdown)
    {
        windowMode = dropdown.GetComponent<TMP_Dropdown>().value;
    }

    public void OnVsyncChangeValue(GameObject dropdown)
    {
        vsyncMode = dropdown.GetComponent<TMP_Dropdown>().value;
    }

    public void OnAnisotropicFilteringcChangeValue(GameObject dropdown)
    {
        anisotropicFilteringMode = dropdown.GetComponent<TMP_Dropdown>().value;
    }

    #endregion
}

//public static class GraphicsSettingsSaving
//{
//    private static string saveField = "HORDES_SAVE";
//
//    public static Save LoadData() => JsonUtility.FromJson<Save>(PlayerPrefs.GetString(saveField));
//
//    public static void SaveData(Save save) => PlayerPrefs.SetString(saveField, JsonUtility.ToJson(save));
//}
//
//public static class CurrentSave
//{
//    public static Save GetSave()
//    {
//        if (PlayerPrefs.HasKey("HORDES_GRAPHICS_SAVE"))
//            return GraphicsSettingsSaving.LoadData();
//        else
//            return new Save();
//    }
//}

[SerializeField]
public class Save
{
    public int savedResolution;
    public int savedGraphics;
    public int savedVSync;
    public int savedFullscreen;
    public int savedAnisotropicFiltering;

    public bool savedFog;
    public bool savedDepthOfField;
    public bool savedLensDistortion;
    public bool savedMotionBlur;
    public bool savedVignette;
    public bool savedAmbientOcclusion;
}
