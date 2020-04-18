using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    private float startRadius = 30f;
    private float endRadius = 5f;

    public Transform anchor;

    public Disk disk;
    private float diskRotationSpeed = 150f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(disk != null)
        {
            anchor.Rotate(Vector3.up * diskRotationSpeed * Time.deltaTime, Space.Self);
        }
    }

    public void EquipDisk(Disk theDisk)
    {
        if(disk)
        {
            Destroy(disk.gameObject);
        }

        disk = theDisk;

        disk.transform.parent = anchor;
        disk.transform.localPosition = Vector3.zero;
        disk.transform.localRotation = Quaternion.identity;
    }
}
