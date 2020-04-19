using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DancerMood : MonoBehaviour
{
    public int[,] reactions = new int[,] { { 1, 0, -1 }, { -1, 1, 0 }, { 0, -1, 1 } };
    public Material[] moodMaterials;

    public GameEnums.MusicColor dancerColor = GameEnums.MusicColor.Magenta;
    public GameEnums.MoodStates currentMood = GameEnums.MoodStates.Neutral;
    public DancerManager manager;

    public Renderer bodyRenderer;
    public Material onFireMaterial;
    private Material defaultMaterial;

    public Animator animator;

    void Awake()
    {
        //dancerColor = (GameEnums.MusicColor)Random.Range(0, 3);
        currentMood = GameEnums.MoodStates.Neutral;
        PlayCurrentMoodAnimation();
        defaultMaterial = bodyRenderer.materials[1];
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
        enabled = false;
    }

    void SetMoodFromInt(int numericMood)
    {
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

        PlayCurrentMoodAnimation();

        ShowMoodHearts_DEBUG();
    }

    public void MusicChanged(GameEnums.MusicColor receivedColor)
    {
        if (currentMood != GameEnums.MoodStates.OnFire)
        {
            int reaction = reactions[(int)dancerColor, (int)receivedColor];
            if (reaction != 0)
            {
                int numericMood = (int)currentMood + reaction;
                if (numericMood >= (int)GameEnums.MoodStates.OnFire)
                {
                    DancersOnFireCounter.dancersOnFire += 1;
                }
                SetMoodFromInt(numericMood);
            }
        }
    }

    public void TooSoonChange(GameEnums.MusicColor currentColor)
    {
        // The too soon doesn't make us lose the OnFire state.
        if (currentMood != GameEnums.MoodStates.OnFire)
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
        int numericMood = (int)currentMood - 1;
        SetMoodFromInt(numericMood);
    }

    public void ShowMoodHearts_DEBUG()
    {
        if (!GameEnums.DEBUGGING)
        {
            return;
        }

        int i = 0;
        int numericMood = 1 + (int)currentMood;
        /*foreach (Transform child in transform)
        {
            Renderer rendererComp = child.GetComponent<Renderer>();
            if (i <= numericMood)
            {
                rendererComp.enabled = true;
            }
            else
            {
                rendererComp.enabled = false;
            }
            i++;
        }*/
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
            print("Set default materials");
            Material[] matArray = bodyRenderer.materials;
            matArray[1] = defaultMaterial;
            bodyRenderer.materials = matArray;
        }
        else if (moodState == GameEnums.MoodStates.OnFire)
        {
            PlayOnFireAnimation();
            // Change material to onFire
            print("Set on fire materials");
            bodyRenderer.materials[1] = onFireMaterial;

            Material[] matArray = bodyRenderer.materials;
            matArray[1] = onFireMaterial;
            bodyRenderer.materials = matArray;

        }
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
    }



}