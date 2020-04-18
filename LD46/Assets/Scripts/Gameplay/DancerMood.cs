using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DancerMood : MonoBehaviour
{
    public int[,] reactions = new int[,] { { 1, 0, -1 }, {-1, 1, 0 }, {0, -1, 1 } };
    public Material[] moodMaterials;

    public GameEnums.MusicColor dancerColor = GameEnums.MusicColor.Magenta;
    public GameEnums.MoodStates currentMood = GameEnums.MoodStates.Neutral;
    public DancerManager manager;

    void Awake()
    {
        dancerColor = (GameEnums.MusicColor)Random.Range(0, 3);
        GetComponent<Renderer>().material = moodMaterials[(int)dancerColor];

        TEST_ShowMoodHearts();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentMood = GameEnums.MoodStates.Neutral;
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

        TEST_ShowMoodHearts();
    }

    public void MusicChanged(GameEnums.MusicColor receivedColor)
    {
        if (currentMood != GameEnums.MoodStates.OnFire)
        {
            int numericMood = (int)currentMood + reactions[(int)dancerColor, (int)receivedColor];
            SetMoodFromInt(numericMood);
        }

    }

    public void TooSoon(GameEnums.MusicColor currentColor)
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