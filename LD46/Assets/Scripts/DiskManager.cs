using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskManager : MonoBehaviour
{
    public static DiskManager diskManagerRef;
    public List<Disk> playList;
    private List<Disk> diskPool = new List<Disk>();
    public int refreshDiskDelay = 2;
    public int amountDisksInDeck = 3;
    public List<Disk> disksInDeck = new List<Disk>();
    public List<Transform> diskAnchors = new List<Transform>();

    void Awake()
    {
        diskManagerRef = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        diskPool = new List<Disk>(playList);
        disksInDeck = new List<Disk>(amountDisksInDeck);

        for (int i = 0; i < amountDisksInDeck; i++)
        {
            SpawnDiskAtDeck(i);
        }
    }

    public void SpawnDiskAtDeck(int position)
    {
        Disk instantiatedDisk = InstantiateDiskFromList();
        if (position > disksInDeck.Count - 1)
        {
            disksInDeck.Insert(position, instantiatedDisk);
        }
        else
        {
            disksInDeck[position] = instantiatedDisk;
        }

        instantiatedDisk.transform.parent = diskAnchors[position].transform;
        instantiatedDisk.transform.localPosition = Vector3.zero;
        instantiatedDisk.transform.localRotation = Quaternion.identity;
        diskAnchors[position].gameObject.SetActive(false);
    }

    private Disk InstantiateDiskFromList()
    {
        if(diskPool.Count == 0)
        {
            diskPool = new List<Disk>(playList);
        }

        Disk result;
        int randomIndex = Random.Range(0, diskPool.Count);
        result = diskPool[randomIndex];
        diskPool.RemoveAt(randomIndex);

        return GameObject.Instantiate(result, transform.position, Quaternion.identity);
    }

    public void UseDisk(Disk disk)
    {
        int position = disksInDeck.IndexOf(disk);
        disksInDeck[position] = null;
        StartCoroutine(RefillDiskAtPosition(position));
    }

    private IEnumerator RefillDiskAtPosition(int position)
    {
        yield return new WaitForSeconds(refreshDiskDelay);
        SpawnDiskAtDeck(position);
    }

    public void ShowDisksInDeck(bool show = true)
    {
        for (int i = 0; i < amountDisksInDeck; i++)
        {
            if (disksInDeck[i] != null)
            {
                diskAnchors[i].gameObject.SetActive(show);
            }
        }
    }
}
