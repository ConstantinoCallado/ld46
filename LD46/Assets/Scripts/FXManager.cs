using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FXManager : MonoBehaviour
{
    public static FXManager fxManagerRef;
    public static GameEnums.PartyStatus status;

    private float _bloomIntensity = 0f;
    private float _chromaticAberrationIntensity = 0f;
    private float _saturationIntensity = 0f;
    private float _brightnessIntensity = 0f;

    private PostProcessVolume _volume;
    private Bloom _bloom = null;
    private ChromaticAberration _chromaticAberration = null;
    private ColorGrading _colorGrading = null;

    private void Awake()
    {
        fxManagerRef = this;
        status = GameEnums.PartyStatus.Dead;

        _bloom = ScriptableObject.CreateInstance<Bloom>();
        _bloom.enabled.Override(true);
        _bloom.intensity.Override(0f);

        _chromaticAberration = ScriptableObject.CreateInstance<ChromaticAberration>();
        _chromaticAberration.enabled.Override(true);
        _chromaticAberration.intensity.Override(0f);

        _colorGrading = ScriptableObject.CreateInstance<ColorGrading>();
        _colorGrading.enabled.Override(true);
        _colorGrading.saturation.Override(0f);
        _colorGrading.brightness.Override(0f);

        _volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, _bloom, _chromaticAberration, _colorGrading);
    }

    public void SetPartyStatus(GameEnums.PartyStatus s) 
    {
        switch (s) 
        {
            case GameEnums.PartyStatus.Dead:
                _bloomIntensity = 0.0f;
                _chromaticAberrationIntensity = 0f;
                _saturationIntensity = -35f;
                _brightnessIntensity = -10f;
                break;
            case GameEnums.PartyStatus.WarmingUp:
                _bloomIntensity = 0.15f;
                _chromaticAberrationIntensity = 0.1f;
                _saturationIntensity = 0f;
                _brightnessIntensity = 0f;
                break;
            case GameEnums.PartyStatus.Super:
                _bloomIntensity = 0.6f;
                _chromaticAberrationIntensity = 0.3f;
                _saturationIntensity = 5f;
                _brightnessIntensity = 5f;
                break;
            case GameEnums.PartyStatus.PartyHard:
                _bloomIntensity = 1.1f;
                _chromaticAberrationIntensity = 0.6f;
                _saturationIntensity = 10f;
                _brightnessIntensity = 10f;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //_lensDistorsion.intensity.value = ((Mathf.Sin(Time.realtimeSinceStartup)));

        if (Mathf.Abs(_bloom.intensity.value - _bloomIntensity) > 0.01f)
            _bloom.intensity.value = Mathf.Lerp(_bloom.intensity.value, _bloomIntensity, 0.75f * Time.deltaTime);

        if (Mathf.Abs(_chromaticAberration.intensity.value - _chromaticAberrationIntensity) > 0.01f)
            _chromaticAberration.intensity.value = Mathf.Lerp(_chromaticAberration.intensity.value, _chromaticAberrationIntensity, 0.75f * Time.deltaTime);

        if (Mathf.Abs(_colorGrading.saturation.value - _saturationIntensity) > 0.01f)
            _colorGrading.saturation.value = Mathf.Lerp(_colorGrading.saturation.value, _saturationIntensity, 0.75f * Time.deltaTime);

        if (Mathf.Abs(_colorGrading.brightness.value - _brightnessIntensity) > 0.01f)
            _colorGrading.brightness.value = Mathf.Lerp(_colorGrading.brightness.value, _brightnessIntensity, 0.75f * Time.deltaTime);

        // Testing
        if (Input.GetKeyDown(KeyCode.U))
            SetPartyStatus(GameEnums.PartyStatus.Dead);
        if (Input.GetKeyDown(KeyCode.I))
            SetPartyStatus(GameEnums.PartyStatus.WarmingUp);
        if (Input.GetKeyDown(KeyCode.O))
            SetPartyStatus(GameEnums.PartyStatus.Super);
        if (Input.GetKeyDown(KeyCode.P))
            SetPartyStatus(GameEnums.PartyStatus.PartyHard);
    }

    private void OnDestroy()
    {
        RuntimeUtilities.DestroyVolume(_volume, true, true);
    }

    public void MusicStopped()
    {
        if(LaserManager.laserManagerRef)
        {
            LaserManager.laserManagerRef.MusicStopped();
        }
    }

    public void MusicStarted(GameEnums.MusicColor musicColor)
    {
        if (LaserManager.laserManagerRef)
        {
            LaserManager.laserManagerRef.MusicStarted(musicColor);
        }
    }
}
