using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DancerState : MonoBehaviour
{
    public GameEnums.DancerStateNames stateName = GameEnums.DancerStateNames.Inactive;

    private Vector3 destination;

    public DancerManager manager;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (stateName == GameEnums.DancerStateNames.Created)
        {
            CheckEnterDanceArea();
            CheckDistanceToDanceSpot();
        }
        else if (stateName == GameEnums.DancerStateNames.MoodActive)
        {
            CheckDistanceToDanceSpot();
        }
        else if (stateName == GameEnums.DancerStateNames.Dancing)
        {
            RotateToDJ();
        }
        else if (stateName == GameEnums.DancerStateNames.Leaving)
        {
            CheckDestroyDancer();
        }
    }

    void CheckEnterDanceArea()
    {
        if (manager.IsDancerInsideDancingArea(transform))
        {
            stateName = GameEnums.DancerStateNames.MoodActive;
            DancerMood dancerMoodComponent = GetComponent<DancerMood>();
            dancerMoodComponent.enabled = true;
        }
    }

    void CheckDistanceToDanceSpot()
    {
        Vector3 groundPosition = new Vector3(transform.position.x, 0f, transform.position.z);
        float distance = Vector3.Distance(groundPosition, destination);
        if (Mathf.Abs(distance) < 0.5f)
        {
            stateName = GameEnums.DancerStateNames.Dancing;
            GetComponent<NavMeshAgent>().isStopped = true;
            DancerMood dancerMoodComp = GetComponent<DancerMood>();
            dancerMoodComp.enabled = true;
            dancerMoodComp.StopWalkAnimation();
            RotateToDJ();
        }
    }

    void RotateToDJ()
    {
        Vector3 target = new Vector3(0.0f, 0.0f, -7.0f);

        Vector3 targetDirection = target - transform.position;

        float singleStep = 5.0f * Time.deltaTime; 
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    void CheckDestroyDancer()
    {
        Vector3 groundPosition = new Vector3(transform.position.x, 0f, transform.position.z);
        float distance = Vector3.Distance(groundPosition, destination);
        if (Mathf.Abs(distance) < 0.5f)
        {
            manager.RemoveDancer(gameObject);
            Destroy(gameObject);
        }
    }

    public void SetState(GameEnums.DancerStateNames newState)
    {
        stateName = newState;
    }

    public void MoveToDestination(Vector3 newDestination)
    {
        destination = newDestination;
        NavMeshAgent agentComp = GetComponent<NavMeshAgent>();
        agentComp.destination = destination;
        agentComp.isStopped = false;
    }
}
