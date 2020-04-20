using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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

[System.Serializable]
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
    public int currentPhase = 0;
    public int startDancers = 2;
    public List<SpawnerPhase> phases;

    private float currentTime = 0.0f;
    private float nextSpawn = 0.0f;

    private DancerManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<DancerManager>();
        currentTime = 0.0f;
        nextSpawn = 0;

        /*for (int i = 0; i < startDancers; i++)
        {
            manager.SpawnDancerAtDestiny();
        }*/
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
