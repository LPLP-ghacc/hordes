using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class CameraStartAnimation : MonoBehaviour
{
    private VolumeProfile profile;

    private LiftGammaGain liftGammaGain;

    void Start()
    {
        profile = GetProfile();

        liftGammaGain = GetGamma(profile);
    }

    float value = -1f;

    private void Update()
    {
        if (value < 0)
        {
            value += Time.deltaTime * 0.1f;
        }
        else
        {
            var standart = new Vector4Parameter(new Vector4(1, 1, 1, 0));

            liftGammaGain.gamma.SetValue(standart);

            liftGammaGain.gain.SetValue(standart);

            Destroy(this.gameObject.GetComponent<CameraStartAnimation>());
        }

        var gammaparameter = new Vector4Parameter(new Vector4(1, 1, 1, value));
        liftGammaGain.gamma.SetValue(gammaparameter);

        var gainparameter = new Vector4Parameter(new Vector4(1, 1, 1, value));
        liftGammaGain.gain.SetValue(gainparameter);
    }

    public Volume GetThisVolume()
    {
        if (!GameObject.Find("Sky and Fog Volume").GetComponent<Volume>())
            return null;

        return GameObject.Find("Sky and Fog Volume").GetComponent<Volume>();
    }

    public VolumeProfile GetProfile()
    {
        if (GetThisVolume() == null)
            return null;

        return GetThisVolume().profile;
    }

    public LiftGammaGain GetGamma(VolumeProfile profile)
    {
        LiftGammaGain _override;

        profile.TryGet<LiftGammaGain>(out _override);

        return _override;
    }
}
