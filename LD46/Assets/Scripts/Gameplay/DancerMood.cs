using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DancerMood : MonoBehaviour
{
    public int[,] reactions = new int[,] { { 1, 0, -1 }, { -1, 1, 0 }, { 0, -1, 1 } };

    public GameEnums.MusicColor dancerColor = GameEnums.MusicColor.Magenta;
    public GameEnums.MoodStates currentMood = GameEnums.MoodStates.Neutral;
    public DancerManager manager;

    public Renderer bodyRenderer;
    public Material onFireMaterial;
    private Material defaultMaterial;

    public Animator animator;

    public ParticleSystem likeParticle;
    public ParticleSystem hateParticle;
    public ParticleSystem silenceParticle;

    public DancerState dancerState;

    void Awake()
    {
        currentMood = GameEnums.MoodStates.Neutral;
        PlayCurrentMoodAnimation();
        defaultMaterial = bodyRenderer.materials[1];
        dancerState = GetComponent<DancerState>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Leave()
    {
        manager.LeaveDancer(this.gameObject);
        StartCoroutine(WaitAndPlaySound(Random.value, "sfx_boo"));
    }

    void SetMoodFromInt(int numericMood)
    {
        float waitValue = Random.value;
        string playMoodSound =  "";
        if (numericMood < (int)GameEnums.MoodStates.RageQuit)
        {
            numericMood = (int)GameEnums.MoodStates.RageQuit;
        }
        else if (numericMood > (int)GameEnums.MoodStates.OnFire)
        {
            numericMood = (int)GameEnums.MoodStates.OnFire;
        }
        if ((int)currentMood > numericMood)
        {
            StartCoroutine(WaitAndEmitParticle(waitValue, hateParticle));
            if (numericMood > (int)GameEnums.MoodStates.RageQuit)
            {
                playMoodSound = "sfx_awww";
            }
        }
        else if ((int)currentMood < numericMood)
        {
            StartCoroutine(WaitAndEmitParticle(waitValue, likeParticle));
            // The yeah sound should only play when not oging to OnFire
            if (numericMood < (int)GameEnums.MoodStates.OnFire)
            {
                playMoodSound = "sfx_yeah";
            }
        }
        currentMood = (GameEnums.MoodStates)numericMood;
        if (currentMood == GameEnums.MoodStates.RageQuit)
        {
            if (!DancerManager.dancerManagerRef.tutorialActive)
            {
                Leave();
            }
            else
            {
                currentMood = GameEnums.MoodStates.Bored;
            }
        }

        StartCoroutine(WaitAndPlayAnimation(waitValue, playMoodSound));
    }

    public void SetSilenceMood(int numericMood)
    {
        float waitValue = Random.value;
        if (numericMood < (int)GameEnums.MoodStates.RageQuit)
        {
            numericMood = (int)GameEnums.MoodStates.RageQuit;
        }
        else if (numericMood > (int)GameEnums.MoodStates.OnFire)
        {
            numericMood = (int)GameEnums.MoodStates.OnFire;
        }
        currentMood = (GameEnums.MoodStates)numericMood;
        if (currentMood == GameEnums.MoodStates.RageQuit)
        {
            Leave();
        }
        silenceParticle.Emit(1);
        StartCoroutine(WaitAndPlayAnimation(waitValue, ""));
    }

    public void MusicChanged(GameEnums.MusicColor receivedColor)
    {
        bool affectedByMusic = dancerState.stateName == GameEnums.DancerStateNames.MoodActive ||
            dancerState.stateName == GameEnums.DancerStateNames.Dancing;
        affectedByMusic = affectedByMusic && currentMood != GameEnums.MoodStates.OnFire;
        if (affectedByMusic)
        {
            int reaction = reactions[(int)dancerColor, (int)receivedColor];
            if (reaction != 0)
            {
                int numericMood = (int)currentMood + reaction;
                if (numericMood >= (int)GameEnums.MoodStates.OnFire)
                {
                    Debug.Log("IncreaseDancersOnFire");
                    manager.IncreaseDancersOnFire(1);
                }
                SetMoodFromInt(numericMood);
            }
        }
    }

    public void TooSoonChange(GameEnums.MusicColor currentColor)
    {
        // The too soon doesn't make us lose the OnFire state.
        bool affectedByMusic = dancerState.stateName == GameEnums.DancerStateNames.MoodActive ||
            dancerState.stateName == GameEnums.DancerStateNames.Dancing;
        affectedByMusic = affectedByMusic && currentMood != GameEnums.MoodStates.OnFire;
        if (affectedByMusic)
        {
            if (dancerColor == currentColor)
            {
                int reaction = reactions[(int)dancerColor, (int)currentColor];
                if (reaction < 0)
                {
                    int numericMood = (int)currentMood + reaction;
                    SetMoodFromInt(numericMood);
                }
            }
        }
    }

    public void TooLateChange()
    {
        // The too late makes everybody lose one mood state.
        bool affectedByMusic = dancerState.stateName == GameEnums.DancerStateNames.MoodActive ||
          dancerState.stateName == GameEnums.DancerStateNames.Dancing;
        if (affectedByMusic)
        {
            manager.SetDancersOnFire(0);
            int numericMood = (int)currentMood - 1;
            SetMoodFromInt(numericMood);
        }
    }

    public void SilencePenalty()
    {
        bool affectedByMusic = dancerState.stateName == GameEnums.DancerStateNames.MoodActive ||
          dancerState.stateName == GameEnums.DancerStateNames.Dancing;
        if (affectedByMusic)
        {
            manager.SetDancersOnFire(0);
            int numericMood = (int)currentMood - 1;
            SetSilenceMood(numericMood);
        }
    }

    void PlayCurrentMoodAnimation()
    {
        PlayAnimation(currentMood);
    }

    void PlayAnimation(GameEnums.MoodStates moodState)
    {
        if (moodState == GameEnums.MoodStates.Bored)
        {
            PlayBoredAnimation();
        }
        else if (moodState == GameEnums.MoodStates.Neutral)
        {
            PlayNeutralAnimation();
        }
        else if (moodState == GameEnums.MoodStates.HavingFun)
        {
            PlayHavingFunAnimation();
            ChangeBodyMaterial(defaultMaterial);
        }
        else if (moodState == GameEnums.MoodStates.OnFire)
        {
            PlayOnFireAnimation();
            ChangeBodyMaterial(onFireMaterial);
        }
    }

    void ChangeBodyMaterial(Material newMaterial)
    {
        Material[] matArray = bodyRenderer.materials;
        matArray[1] = newMaterial;
        bodyRenderer.materials = matArray;
    }

    public void PlayWalkAnimation()
    {
        animator.SetBool("isWalking", true);
    }

    public void StopWalkAnimation()
    {
        animator.SetBool("isWalking", false);
    }

    void PlayBoredAnimation()
    {
        animator.SetBool("isBored", true);
        animator.SetBool("isFun", false);
        animator.SetBool("isOnFire", false);
    }

    void PlayNeutralAnimation()
    {
        animator.SetBool("isBored", false);
        animator.SetBool("isFun", false);
        animator.SetBool("isOnFire", false);        
    }

    void PlayHavingFunAnimation()
    {
        animator.SetBool("isBored", false);
        animator.SetBool("isFun", true);
        animator.SetBool("isOnFire", false);
    }

    void PlayOnFireAnimation()
    {
        animator.SetBool("isBored", false);
        animator.SetBool("isFun", true);
        animator.SetBool("isOnFire", true);
        AudioManager.audioManagerRef.PlaySoundWithRandomPitch("sfx_wohoo");
    }

    IEnumerator WaitAndPlayAnimation(float waitTime, string playMoodSound)
    {
        yield return new WaitForSeconds(waitTime);
        PlayCurrentMoodAnimation();
        if (playMoodSound.Length > 0)
        {
            AudioManager.audioManagerRef.PlaySoundWithRandomPitch(playMoodSound);
        }
    }

    IEnumerator WaitAndEmitParticle(float waitTime, ParticleSystem particleSys)
    {
        yield return new WaitForSeconds(waitTime);
        particleSys.Emit(1);
    }

    IEnumerator WaitAndPlaySound(float waitTime, string sound)
    {
        yield return new WaitForSeconds(waitTime);
        AudioManager.audioManagerRef.PlaySoundWithRandomPitch(sound);
    }

}