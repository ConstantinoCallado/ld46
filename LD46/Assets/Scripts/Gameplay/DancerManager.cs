using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DancerManager : MonoBehaviour
{
    public GameObject dancerPrefab;

    public float DANCERS_DELAY = 10f;

    public int MAX_DANCERS = 30;

    public Transform entranceList;
    public Transform dancingArea;
    public Transform exitList;

    private float lastDancerTime = 0f;
    private List<GameObject> dancers;

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
        dancerStateComp.SetState(GameEnums.DancerStateNames.Created);
        dancerStateComp.MoveToDestination(dancerDestination);
        dancerStateComp.manager = this;
        dancer.GetComponent<DancerMood>().manager = this;

        dancers.Add(dancer);
    }

    public void LeaveDancer(GameObject dancer)
    {
        Vector3 dancerExit = GetDancerExit();
        DancerState dancerStateComp = dancer.GetComponent<DancerState>();
        dancerStateComp.SetState(GameEnums.DancerStateNames.Leaving);
        dancerStateComp.MoveToDestination(dancerExit);
    }

    public void RemoveDancer(GameObject dancer)
    {
        dancers.Remove(dancer);
    }

    Vector3 GetDancerEntrance()
    {
        int randomIndex = Random.Range(0, entranceList.childCount);
        return entranceList.GetChild(randomIndex).transform.position;
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
        int randomIndex = Random.Range(0, exitList.childCount);
        return exitList.GetChild(randomIndex).transform.position;
    }

    public void TooSoonChange(GameEnums.MusicColor musicColor)
    {
        foreach (GameObject dancer in dancers)
        {
            DancerMood dancerMoodComp = dancer.GetComponent<DancerMood>();
            if (dancerMoodComp.enabled)
            {
                dancerMoodComp.TooSoonChange(musicColor);
            }
        }
    }

    public void PerfectChange(GameEnums.MusicColor musicColor)
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

    public void TooLateChange(GameEnums.MusicColor musicColor)
    {
        foreach (GameObject dancer in dancers)
        {
            DancerMood dancerMoodComp = dancer.GetComponent<DancerMood>();
            if (dancerMoodComp.enabled)
            {
                dancerMoodComp.TooLateChange();
            }
        }
    }

    void TEST_MusicChange()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TooSoonChange(GameEnums.MusicColor.Cyan);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            TooSoonChange(GameEnums.MusicColor.Magenta);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            TooSoonChange(GameEnums.MusicColor.Yellow);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            PerfectChange(GameEnums.MusicColor.Cyan);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            PerfectChange(GameEnums.MusicColor.Magenta);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            PerfectChange(GameEnums.MusicColor.Yellow);
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            TooLateChange(GameEnums.MusicColor.Cyan);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            TooLateChange(GameEnums.MusicColor.Magenta);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            TooLateChange(GameEnums.MusicColor.Yellow);
        }

    }


}
