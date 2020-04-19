using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DancerManager : MonoBehaviour
{
    public List<GameObject> dancerPrefabs;

    public float DANCERS_DELAY = 10f;

    public int MAX_DANCERS = 30;

    public Transform entranceList;
    public Transform dancingArea;
    public Transform exitList;

    private List<GameObject> dancers;

    public DancerSpawner spawner;

    private Rect dancingRect;

    public int dancersOnFire;

    void Awake()
    {
        dancers = new List<GameObject>();
        spawner = GetComponent<DancerSpawner>();
        dancersOnFire = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 topRight = dancingArea.Find("TopRight").transform.position;
        Vector3 bottomLeft = dancingArea.Find("BottomLeft").transform.position;

        int width = (int)Mathf.Abs(topRight.x - bottomLeft.x);
        int height = (int)Mathf.Abs(topRight.z - bottomLeft.z);
        dancingRect = new Rect(bottomLeft.x, bottomLeft.z, width, height);

        UpdatePartyStatus();
    }

    // Update is called once per frame
    void Update()
    {
        TEST_MusicChange();
    }

    public void SpawnDancer()
    {
        if (dancers.Count < MAX_DANCERS)
        {
            Vector3 dancerEntrance = GetDancerEntrance();
            Vector3 dancerDestination = GetDancingSpot();
            GameObject randomDancer = GetRandomDancer();
            GameObject dancer = Instantiate(randomDancer, dancerEntrance, Quaternion.identity);

            DancerState dancerStateComp = dancer.GetComponent<DancerState>();
            dancerStateComp.SetState(GameEnums.DancerStateNames.Created);
            dancerStateComp.MoveToDestination(dancerDestination);
            dancerStateComp.manager = this;
            DancerMood dancerMoodComp = dancer.GetComponent<DancerMood>();
            dancerMoodComp.manager = this;
            dancerMoodComp.PlayWalkAnimation();

            dancers.Add(dancer);
        }
    }

    public void SpawnDancerAtDestiny()
    {
        if (dancers.Count < MAX_DANCERS)
        {
            Vector3 dancerDestination = GetDancingSpot();
            GameObject randomDancer = GetRandomDancer();
            GameObject dancer = Instantiate(randomDancer, dancerDestination, Quaternion.identity);

            DancerState dancerStateComp = dancer.GetComponent<DancerState>();
            dancerStateComp.SetState(GameEnums.DancerStateNames.Dancing);
            dancerStateComp.manager = this;
            DancerMood dancerMoodComp = dancer.GetComponent<DancerMood>();
            dancerMoodComp.manager = this;
            dancerMoodComp.enabled = true;

            dancers.Add(dancer);
        }
    }

    public void LeaveDancer(GameObject dancer)
    {
        Vector3 dancerExit = GetDancerExit();
        DancerState dancerStateComp = dancer.GetComponent<DancerState>();
        dancerStateComp.SetState(GameEnums.DancerStateNames.Leaving);
        dancerStateComp.MoveToDestination(dancerExit);
        DancerMood dancerMoodComp = dancer.GetComponent<DancerMood>();
        dancerMoodComp.enabled = true;
        dancerMoodComp.PlayWalkAnimation();
    }

    public bool IsDancerInsideDancingArea(Transform dancer)
    {
        Vector2 point = new Vector2(dancer.position.x, dancer.position.y);
        return dancingRect.Contains(point);
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

    GameObject GetRandomDancer()
    {
        int randomInt = Random.Range(0, dancerPrefabs.Count);
        return dancerPrefabs[randomInt];
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
        // TODO: ScreenShake Here!
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
        DancersOnFireCounter.dancersOnFire = 0;
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

    public void IncreaseDancersOnFire(int quantity)
    {
        dancersOnFire += quantity;
        UpdatePartyStatus();
    }

    public void SetDancersOnFire(int quantity)
    {
        dancersOnFire = quantity;
        UpdatePartyStatus();
    }

    void UpdatePartyStatus()
    {
        int percentage = (100 * dancersOnFire) / MAX_DANCERS;
        if (percentage == 0)
        {
            FXManager.fxManagerRef.SetPartyStatus(GameEnums.PartyStatus.Dead);
        }
        else if (0 < percentage && percentage <= 25)
        {
            FXManager.fxManagerRef.SetPartyStatus(GameEnums.PartyStatus.WarmingUp);
        }
        else if (25 < percentage && percentage <= 60)
        {
            FXManager.fxManagerRef.SetPartyStatus(GameEnums.PartyStatus.Super);
        }
        else
        {
            FXManager.fxManagerRef.SetPartyStatus(GameEnums.PartyStatus.PartyHard);
        }
    }
}
