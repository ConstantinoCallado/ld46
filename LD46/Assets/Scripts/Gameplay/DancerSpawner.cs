using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn
{
    public int seconds;
    public int dancers;

    public Spawn(int _seconds, int _dancers)
    {
        seconds = _seconds;
        dancers = _dancers;
    }
}

public class SpawnerPhase
{
    public int startTime;
    public int endTime;
    public List<Spawn> spawns;

    public SpawnerPhase(int _startTime, int _endTime)
    {
        startTime = _startTime;
        endTime = _endTime;
        spawns = new List<Spawn>();
    }

    public void AddSpawn(Spawn _spawn)
    {
        spawns.Add(_spawn);
    }

    public Spawn GetRandomSpawn()
    {
        return spawns[Random.Range(0, spawns.Count)];
    }
}

public class DancerSpawner : MonoBehaviour
{
    public List<SpawnerPhase> phases;

    public int currentPhase = 0;

    private float currentTime = 0.0f;
    private float nextSpawn = 0.0f;

    private DancerManager manager;

    void Awake()
    {
        currentPhase = 0;
        phases = new List<SpawnerPhase>();
        phases.Add(new SpawnerPhase(0, 60));
        Spawn s1 = new Spawn(10, 1);
        phases[0].AddSpawn(s1);

        phases.Add(new SpawnerPhase(60, 99999));
        Spawn s2 = new Spawn(10, 1);
        phases[1].AddSpawn(s2);
        Spawn s3 = new Spawn(20, 2);
        phases[1].AddSpawn(s3);
    }

    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<DancerManager>();
        currentTime = 0.0f;
        nextSpawn = phases[currentPhase].GetRandomSpawn().seconds;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= nextSpawn)
        {
            Spawn rSpawn = phases[currentPhase].GetRandomSpawn();
            SpawnDancer(rSpawn.dancers);
            CalculateNextSpawn(rSpawn.seconds);
        }
    }

    void SpawnDancer(int dancers)
    {
        for (int i = 0; i < dancers; i++)
        {
            manager.SpawnDancer();
        }
    }

    void CalculateNextSpawn(int seconds)
    {
        nextSpawn += seconds;
        if (currentPhase < phases.Count -1)
        {
            if (nextSpawn >= phases[currentPhase].endTime)
            {
                currentPhase++;
            }
        }
    }
}
