using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mood : MonoBehaviour
{
    public enum MoodStates { Hate = 0, Neutral = 1, Love = 2, PartyHard = 3};

    public MoodStates currentState = MoodStates.Neutral;

    public float MIN_MOOD = 0f;
    public float MAX_MOOD = 100f;
    public float START_MOOD = 50f;
    public float currentMood = 100f;

    public float loveMoodFactor = 5f;
    public float neutralMoodFactor = 1f;
    public float hateMoodFactor = -5f;

    public Transform exitTransform;

    private bool leaving = false;

    // Start is called before the first frame update
    void Start()
    {
        currentState = MoodStates.Hate;
        currentMood = START_MOOD;
        leaving = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMood();
        MoodActions();
    }

    void UpdateMood()
    {
        if (currentState == MoodStates.Love)
        {
            UpdateMoodLove();
        }
        else if (currentState == MoodStates.Neutral)
        {
            UpdateMoodNeutral();
        }
        else if (currentState == MoodStates.Hate)
        {
            UpdateMoodHate();
        }
    }

    void UpdateMoodLove()
    {
        currentMood += loveMoodFactor * Time.deltaTime;
    }

    void UpdateMoodNeutral()
    {
        currentMood += neutralMoodFactor * Time.deltaTime;
    }

    void UpdateMoodHate()
    {
        currentMood += hateMoodFactor * Time.deltaTime;
    }

    void MoodActions()
    {
        if (currentMood <= MIN_MOOD && !leaving)
        {
            Leave();
        }
        else if (currentMood >= MAX_MOOD && currentState != MoodStates.PartyHard)
        {
            currentMood = MAX_MOOD;
            PartyHard();
        }
    }

    void Leave()
    {
        currentMood = MIN_MOOD;
        leaving = true;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = exitTransform.position;
        //Destroy(gameObject);
    }

    void PartyHard()
    {

    }

}