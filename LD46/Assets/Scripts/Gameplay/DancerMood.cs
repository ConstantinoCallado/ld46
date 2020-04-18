using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DancerMood : MonoBehaviour
{
    public enum MoodStates { RageQuit = -2, Bored = -1, Neutral = 0, HavingFun = 1, OnFire = 2};
    public enum MusicColor { Magenta = 0, Cyan = 1, Yellow = 2};
    public int[,] reactions = new int[,] { { 1, 0, -1 }, {-1, 1, 0 }, {0, -1, 1 } };

    public MusicColor dancerColor = MusicColor.Magenta;
    public MoodStates currentState = MoodStates.Neutral;
    public DancerManager manager;

    public float TEST_time;
    public MusicColor TEST_Color;

    // Start is called before the first frame update
    void Start()
    {
        currentState = MoodStates.Neutral;
        TEST_time = 0.0f;
        TEST_Color = MusicColor.Yellow;
        //TEST_Color = (MusicColor)Random.Range(0, 3);
    }

    // Update is called once per frame
    void Update()
    {
        TEST_time += Time.deltaTime;
        if (TEST_time > 5.0f)
        {
            TEST_time -= 5.0f;
            ChangeMood(TEST_Color);
        }
    }

    void Leave()
    {
        manager.LeaveDancer(this.gameObject);
        enabled = false;
    }

    void ChangeMood(MusicColor receivedColor)
    {
        if (currentState == MoodStates.OnFire)
        {
            // If I am on fire, the changes in the music don't affect me
        }
        else
        {
            int numericState = (int)currentState + reactions[(int)dancerColor, (int)receivedColor];
            if (numericState < (int)MoodStates.RageQuit)
            {
                numericState = (int)MoodStates.RageQuit;
            }
            else if (numericState > (int)MoodStates.OnFire)
            {
                numericState = (int)MoodStates.OnFire;
            }
            currentState = (MoodStates)numericState;
            if (currentState == MoodStates.RageQuit)
            {
                Leave();
            }
        }
    }

}