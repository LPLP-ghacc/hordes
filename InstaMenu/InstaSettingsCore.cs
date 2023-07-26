using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using Toggle = UnityEngine.UI.Toggle;

public class InstaSettingsCore : MonoBehaviour
{
    public Save savedSave; //what?

    public List<TMP_Dropdown> dropdowns = new List<TMP_Dropdown>();
    public List<Toggle> toggles = new List<Toggle>();

    [Space]
    public TextMeshProUGUI messageBox, logBox;

    private int currentResolutionIndex;
    private string saveField = "HORDES_SAVE";

    private protected void Awake()
    {
        savedSave = GetSave();
    }

    private protected void Start()
    {
        LoadSettings();

        Message("Welcome to the Settings Menu!");

        DropdownSettingsOnStart();
        TogglesSettingsOnStart();

        SelectSavedFieds();
    }

    private void SelectSavedFieds()
    {
        var save = savedSave;

        dropdowns[0].value = save._mode;
        dropdowns[1].value = save._resolution;
        dropdowns[2].value = save._qualityLevel;
        dropdowns[3].value = save._vsync;
        dropdowns[4].value = save._anisotropicFiltering;

        toggles[0].isOn = save._fog;
        toggles[1].isOn = save._depthOfField;
        toggles[2].isOn = save._lensDistortion;
        toggles[3].isOn = save._motionBlur;
        toggles[4].isOn = save._vignette;
        toggles[5].isOn = save._ambientOcclusion;
    }

    #region MainSettings
    private protected void DropdownSettingsOnStart()
    {
        dropdowns.ForEach(x => {
            x.ClearOptions();

            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

            GetOptionsFromObjectData(x.gameObject.GetComponent<ObjectData>()).ToList().ForEach(y =>
            {
                var data = new TMP_Dropdown.OptionData();
                data.text = y;
                options.Add(data);
            });

            x.AddOptions(options);

            x.onValueChanged.AddListener(delegate {
                DropdownValueChanged(x);
            });
        });
    }

    private void DropdownValueChanged(TMP_Dropdown change)
    {
        var name = change.GetComponent<ObjectData>().item1;
        var value = change.value;

        switch (name)
        {
            case "ScreenMode":
                Screen.fullScreenMode = (FullScreenMode)value; 
                break;

            case "Resolution":
                var resolution = Screen.resolutions[value];
                currentResolutionIndex = value;
                Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, resolution.refreshRate);
                Message($"Resolution: {Screen.currentResolution.width} X {Screen.currentResolution.height} & {Screen.currentResolution.refreshRate} FPS");
                break;

            case "Graphics":
                QualitySettings.SetQualityLevel(value);
                Message($"Graphics Set: {QualitySettings.names[QualitySettings.GetQualityLevel()]}");
                break;

            case "VSync":
                if (value == 0)
                {
                    QualitySettings.vSyncCount = 0;
                    Message($"VSync: OFF ({QualitySettings.vSyncCount})");
                }
                else
                {
                    QualitySettings.vSyncCount = 1;
                    Message($"VSync: ON ({QualitySettings.vSyncCount})");
                }
                break;

            case "Anisotropic Filtering":
                QualitySettings.anisotropicFiltering = (AnisotropicFiltering)value;
                break;
        }
    }

    private string[] GetOptionsFromObjectData(ObjectData data)
    {
        List<string> options = new List<string>();

        switch (data.item1)
        {
            case "ScreenMode":
                options.Add("Maximized Window");
                options.Add("Windowed");
                options.Add("FullScreen Window");
                options.Add("Exclusive Window");
                break;

            case "Resolution":
                Screen.resolutions.ToList().ForEach(x =>
                {
                    options.Add($"{x.width} X {x.height} & {x.refreshRate}");
                });
                break;

            case "Graphics":
                QualitySettings.names.ToList().ForEach(options.Add);
                break;

            case "VSync":
                options.Add("OFF");
                options.Add("ON");
                break;

            case "Anisotropic Filtering":
                options.Add("Disable");
                options.Add("Enable");
                options.Add("Force Enable");
                break;
        }

        return options.ToArray();
    }
    #endregion

    #region Toggles

    private protected void TogglesSettingsOnStart()
    {
        toggles.ForEach(x =>
        {
            x.onValueChanged.AddListener(delegate
            {
                ToggleValueChanged(x);
            });
        });
    }

    private void ToggleValueChanged(Toggle toggle)
    {
        var name = toggle.GetComponent<ObjectData>().item1;

        var value = toggle.isOn;

        var profile = VolumeSettings.GetProfile();

        switch (name)
        {
            case "Fog":
                VolumeSettings.GetFog(profile).active = value;
                break;
            case "DepthOfField":
                VolumeSettings.GetDepth(profile).active = value;
                break;
            case "LensDistortion":
                VolumeSettings.GetLens(profile).active = value;
                break;
            case "Motion Blur":
                VolumeSettings.GetBlur(profile).active = value;
                break;
            case "Vignette":
                VolumeSettings.GetVignette(profile).active = value;
                break;
            case "Ambient Occlusion":
                VolumeSettings.GetAmbientOcclusion(profile).active = value;
                break;
        }
    }

    #endregion

    private void Message(string message, bool log = false)
    {
        if (!log)
        {
            messageBox.text = " >  " + message + "\n";
        }else
        {
            logBox.text = $"> {message}";
        }
    }

    public void SaveSettings()
    {
        int mode = (int)Screen.fullScreenMode;
        int resolution = currentResolutionIndex;
        int qualityLevel = QualitySettings.GetQualityLevel();
        int vsync = QualitySettings.vSyncCount;
        Debug.Log($"saved: {vsync}");
        int anisotropicFiltering = (int)QualitySettings.anisotropicFiltering;

        var profile = VolumeSettings.GetProfile();
        bool fog = VolumeSettings.GetFog(profile).active;
        bool depthOfField = VolumeSettings.GetDepth(profile).active;
        bool lensDistortion = VolumeSettings.GetLens(profile).active;
        bool motionBlur = VolumeSettings.GetBlur(profile).active;
        bool vignette = VolumeSettings.GetVignette(profile).active;
        bool ambientOcclusion = VolumeSettings.GetAmbientOcclusion(profile).active;

        var save = new Save()
        {
            _mode = mode,
            _resolution = resolution,
            _qualityLevel = qualityLevel,
            _vsync = vsync,
            _anisotropicFiltering = anisotropicFiltering,

            _fog = fog,
            _depthOfField = depthOfField,
            _lensDistortion = lensDistortion,
            _motionBlur = motionBlur,
            _vignette = vignette,
            _ambientOcclusion = ambientOcclusion,
        };

        if (SetSave(save))
        {
            Message("Successfully saved", true);
        }
        else
        {
            Message("Saving error, call to developer", true);
        }
    }

    private void LoadSettings()
    {
        var save = savedSave;

        Screen.fullScreenMode = (FullScreenMode)save._mode;
        var resolution = Screen.resolutions[save._resolution];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, resolution.refreshRate);
        QualitySettings.SetQualityLevel(save._qualityLevel);
        QualitySettings.vSyncCount = save._vsync;
        QualitySettings.anisotropicFiltering = (AnisotropicFiltering)save._anisotropicFiltering;

        var profile = VolumeSettings.GetProfile();

        VolumeSettings.GetFog(profile).active = save._fog;
        VolumeSettings.GetDepth(profile).active = save._depthOfField;
        VolumeSettings.GetLens(profile).active = save._lensDistortion;
        VolumeSettings.GetBlur(profile).active = save._motionBlur;
        VolumeSettings.GetVignette(profile).active = save._vignette;
        VolumeSettings.GetAmbientOcclusion(profile).active = save._ambientOcclusion;
    }

    public Save GetSave()
    {
        if (!PlayerPrefs.HasKey(saveField)) return null;
        else return JsonUtility.FromJson<Save>(PlayerPrefs.GetString(saveField));
    }

    public bool SetSave(Save save)
    {
        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetString(saveField, JsonUtility.ToJson(save));

        PlayerPrefs.Save();

        if (PlayerPrefs.HasKey(saveField))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [SerializeField]
    public class Save
    {
        public int _mode;
        public int _resolution;
        public int _qualityLevel;
        public int _vsync;
        public int _anisotropicFiltering;

        public bool _fog;
        public bool _depthOfField;
        public bool _lensDistortion;
        public bool _motionBlur;
        public bool _vignette;
        public bool _ambientOcclusion;
    }
}
