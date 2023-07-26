using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public string volumeParameter = "";
    public AudioMixer mixer;

    private Slider slider;
    private const float _multiplier = 20f;
    private float _volumeValue;

    protected void Awake()
    {
        _volumeValue = PlayerPrefs.GetFloat(volumeParameter);

        slider = GetComponent<Slider>();

        slider.onValueChanged.AddListener(HandlerSliderOnValueChanged);
    }

    private void HandlerSliderOnValueChanged(float value)
    {
        _volumeValue = Mathf.Log10(value) * _multiplier;  

        mixer.SetFloat(volumeParameter, _volumeValue);
    }

    public void Start()
    {
        _volumeValue = PlayerPrefs.GetFloat(volumeParameter, Mathf.Log10(slider.value) * _multiplier);

        slider.value = Mathf.Pow(10, _volumeValue / _multiplier);
    }

    protected void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeParameter, _volumeValue);
    }
}
