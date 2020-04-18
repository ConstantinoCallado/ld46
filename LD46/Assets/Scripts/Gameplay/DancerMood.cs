using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DancerMood : MonoBehaviour
{
    public enum MoodStates { RageQuit = -2, Bored = -1, Neutral = 0, HavingFun = 1, OnFire = 2};
    public enum MusicColor { Magenta = 0, Cyan = 1, Yellow = 2};
    public int[,] reactions = new int[,] { { 1, 0, -1 }, {-1, 1, 0 }, {0, -1, 1 } };
    public Material[] moodMaterials;

    public MusicColor dancerColor = MusicColor.Magenta;
    public MoodStates currentMood = MoodStates.Neutral;
    public DancerManager manager;

    void Awake()
    {
        dancerColor = (MusicColor)Random.Range(0, 3);
        GetComponent<Renderer>().material = moodMaterials[(int)dancerColor];

        TEST_ShowMoodHearts();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentMood = MoodStates.Neutral;
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
        if (numericMood < (int)MoodStates.RageQuit)
        {
            numericMood = (int)MoodStates.RageQuit;
        }
        else if (numericMood > (int)MoodStates.OnFire)
        {
            numericMood = (int)MoodStates.OnFire;
        }
        currentMood = (MoodStates)numericMood;
        if (currentMood == MoodStates.RageQuit)
        {
            Leave();
        }

        TEST_ShowMoodHearts();
    }

    public void MusicChanged(MusicColor receivedColor)
    {
        if (currentMood != MoodStates.OnFire)
        {
            int numericMood = (int)currentMood + reactions[(int)dancerColor, (int)receivedColor];
            SetMoodFromInt(numericMood);
        }

    }

    public void TooSoon(MusicColor currentColor)
    {
        if (dancerColor == currentColor)
        {
            int numericMood = (int)currentMood - 1;
            SetMoodFromInt(numericMood);
        }
    }

    public void TooLate()
    {
        int numericMood = (int)currentMood - 1;
        SetMoodFromInt(numericMood);
    }

    void TEST_ShowMoodHearts()
    {
        int i = 0;
        int numericMood = 1 + (int)currentMood;
        foreach (Transform child in transform)
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
        }
    }

}