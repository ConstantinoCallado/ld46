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
            Vector3 groundPosition = new Vector3(transform.position.x, 0f, transform.position.z);
            float distance = Vector3.Distance(groundPosition, destination);
            if (Mathf.Abs(distance) < 0.5f)
            {
                stateName = GameEnums.DancerStateNames.Dancing;
                GetComponent<NavMeshAgent>().isStopped = true;
                GetComponent<DancerMood>().enabled = true;
            }
        }
        else if (stateName == GameEnums.DancerStateNames.Leaving)
        {
            Vector3 groundPosition = new Vector3(transform.position.x, 0f, transform.position.z);
            float distance = Vector3.Distance(groundPosition, destination);
            if (Mathf.Abs(distance) < 0.5f)
            {
                manager.RemoveDancer(gameObject);
                Destroy(gameObject);
            }
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
