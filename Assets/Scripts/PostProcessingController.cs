using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{
    #region Singleton

    private static PostProcessingController _postprocessingControllerInstance;

    public static PostProcessingController Instance
    {
        get
        {
            if (_postprocessingControllerInstance == null) _postprocessingControllerInstance = FindFirstObjectByType<PostProcessingController>();
            return _postprocessingControllerInstance;
        }
    }

    #endregion

    private Volume _volume;
    private VolumeProfile _volumeProfile;
    private DepthOfField _depthOfField;
    private ChromaticAberration _chromaticAberration;
    private Vignette _vignette;

    public const float DefaultVignetteIntensity = 0.4f;
    private const float DefaultChromaticAberrationIntensity = 0.2f;

    #region Unity Event

    private void Awake()
    {
        _volume = GetComponent<Volume>();
        // _volumeProfile = GetComponent<Volume>().profile;
        _volume.profile.TryGet(out _depthOfField);
        _volume.profile.TryGet(out _chromaticAberration);
        _volume.profile.TryGet(out _vignette);
    }

    private void Start()
    {
        SetDepthOfField(false);
        SetChromaticAberration(false);
        SetChromaticAberrationIntensity(DefaultChromaticAberrationIntensity);
        SetVignetteIntensity(DefaultVignetteIntensity);
    }

    #endregion

    public void SetDepthOfField(bool value)
    {
        _depthOfField.active = value;
    }

    public void SetChromaticAberration(bool value)
    {
        _chromaticAberration.active = value;
    }

    private void SetChromaticAberrationIntensity(float value)
    {
        _chromaticAberration.intensity.value = value;
    }

    public void SetVignetteIntensity(float value)
    {
        _vignette.intensity.value = value;
    }

    public void SetVignetteCenter(Vector2 center)
    {
        _vignette.center.value = center;
    }
}