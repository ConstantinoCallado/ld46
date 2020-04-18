using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DancerState : MonoBehaviour
{
    public enum DancerStateNames { Inactive = 0, Created = 1, Dancing = 2, Leaving = 3 }

    public DancerStateNames stateName = DancerStateNames.Inactive;

    private Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (stateName == DancerStateNames.Created)
        {
            Vector3 groundPosition = new Vector3(transform.position.x, 0f, transform.position.z);
            float distance = Vector3.Distance(groundPosition, destination);
            print("Ground pos " + groundPosition + " destiny " + destination);
            print("Distance " + distance);
            if (Mathf.Abs(distance) < 0.5f)
            {
                stateName = DancerStateNames.Dancing;
                GetComponent<NavMeshAgent>().isStopped = true;
                GetComponent<DancerMood>().enabled = true;
            }
        }
        else if (stateName == DancerStateNames.Leaving)
        {

        }
    }

    public void SetState(DancerStateNames newState)
    {
        stateName = newState;
    }

    public void MoveToDestination(Vector3 newDestination)
    {
        destination = newDestination;
        GetComponent<NavMeshAgent>().destination = destination;
    }
}
