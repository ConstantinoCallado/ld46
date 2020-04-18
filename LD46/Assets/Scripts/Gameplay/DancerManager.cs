using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DancerManager : MonoBehaviour
{
    public Transform entranceNodes;
    public Transform dancingSpots;
    public Transform dancingArea;
    public Transform exitNodes;

    public GameObject dancerPrefab;

    public float DANCERS_DELAY = 10f;

    public int MAX_DANCERS = 10;

    private float lastDancerTime = 0f;
    private List<GameObject> dancers;

    private DancerMood.MusicColor currentMusicColor;

    // Start is called before the first frame update
    void Start()
    {
        dancers = new List<GameObject>();
        lastDancerTime = 0f;
        CreateDancer();
    }

    // Update is called once per frame
    void Update()
    {
        TEST_MusicChange();

        lastDancerTime += Time.deltaTime;
        if (lastDancerTime >= DANCERS_DELAY)
        {
            lastDancerTime -= DANCERS_DELAY;
            if (dancers.Count < MAX_DANCERS)
            {
                CreateDancer();
            }
        }
    }

    void CreateDancer()
    {
        Vector3 dancerEntrance = GetDancerEntrance();
        Vector3 dancerDestination = GetDancingSpot();
        GameObject dancer = Instantiate(dancerPrefab, dancerEntrance, Quaternion.identity);

        DancerState dancerStateComp = dancer.GetComponent<DancerState>();
        dancerStateComp.SetState(DancerState.DancerStateNames.Created);
        dancerStateComp.MoveToDestination(dancerDestination);
        dancer.GetComponent<DancerMood>().manager = this;

        dancers.Add(dancer);
    }

    public void LeaveDancer(GameObject dancer)
    {
        Vector3 dancerExit = GetDancerExit();
        NavMeshAgent agentComp = dancer.GetComponent<NavMeshAgent>();
        agentComp.destination = dancerExit;
        agentComp.isStopped = false;
    }

    Vector3 GetDancerEntrance()
    {
        int randomIndex = Random.Range(0,entranceNodes.childCount);
        return entranceNodes.GetChild(randomIndex).transform.position;
    }

    Vector3 GetDancingSpot()
    {
        Vector3 topRight = dancingArea.Find("TopRight").transform.position;
        Vector3 bottomLeft = dancingArea.Find("BottomLeft").transform.position;
        float randomX = Random.Range(topRight.x, bottomLeft.x);
        float randomZ = Random.Range(topRight.z, bottomLeft.z);

        return new Vector3(randomX, 0.0f, randomZ);
    }

    Vector3 GetDancerExit()
    {
        int randomIndex = Random.Range(0, exitNodes.childCount);
        return exitNodes.GetChild(randomIndex).transform.position;
    }

    void ChangeMusic(DancerMood.MusicColor musicColor)
    {
        foreach (GameObject dancer in dancers)
        {
            DancerMood dancerMoodComp = dancer.GetComponent<DancerMood>();
            if (dancerMoodComp.enabled)
            {
                dancerMoodComp.MusicChanged(musicColor);
            }
        }
    }

    void TooSoon()
    {
        foreach (GameObject dancer in dancers)
        {
            DancerMood dancerMoodComp = dancer.GetComponent<DancerMood>();
            if (dancerMoodComp.enabled)
            {
                dancerMoodComp.TooSoon(currentMusicColor);
            }
        }
    }

    void TooLate()
    {
        foreach (GameObject dancer in dancers)
        {
            DancerMood dancerMoodComp = dancer.GetComponent<DancerMood>();
            if (dancerMoodComp.enabled)
            {
                dancerMoodComp.TooLate();
            }
        }
    }

        void TEST_MusicChange()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentMusicColor = DancerMood.MusicColor.Cyan;
            ChangeMusic(DancerMood.MusicColor.Cyan);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            currentMusicColor = DancerMood.MusicColor.Magenta;
            ChangeMusic(DancerMood.MusicColor.Magenta);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            currentMusicColor = DancerMood.MusicColor.Yellow;
            ChangeMusic(DancerMood.MusicColor.Yellow);
        }
    }


}
