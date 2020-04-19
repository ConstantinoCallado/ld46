using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FXManager : MonoBehaviour
{
    public static FXManager fxManagerRef;

    public static GameEnums.PartyStatus status;

    private PostProcessVolume _volume;
    private Bloom _bloom = null;
    private ChromaticAberration _chromaticAberration = null;

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

        _volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, _bloom, _chromaticAberration);
    }

    public void SetPartyStatus(GameEnums.PartyStatus s) 
    {
        switch (s) 
        {
            case GameEnums.PartyStatus.Dead:
                _bloom.intensity.value = 0;
                _chromaticAberration.intensity.value = 0;
                break;
            case GameEnums.PartyStatus.WarmingUp:
                _bloom.intensity.value = 0.15f;
                _chromaticAberration.intensity.value = 0.15f;
                break;
            case GameEnums.PartyStatus.Super:
                _bloom.intensity.value = 0.7f;
                _chromaticAberration.intensity.value = 0.3f;
                break;
            case GameEnums.PartyStatus.PartyHard:  
                _bloom.intensity.value = 1f;
                _chromaticAberration.intensity.value = 0.5f;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        //_lensDistorsion.intensity.value = ((Mathf.Sin(Time.realtimeSinceStartup)));

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
}
