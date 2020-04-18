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

    public Transform armPivot;
    private int armIdleAngle = -15;
    private int armEndAngle = 30;

    // Start is called before the first frame update
    void Start()
    {
        RestoreArm();
    }

    void RestoreArm()
    {
        armPivot.transform.localEulerAngles = new Vector3(0, armIdleAngle, 0);
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
            DestroyDisk();
        }

        disk = theDisk;

        disk.transform.parent = anchor;
        disk.transform.localPosition = Vector3.zero;
        disk.transform.localRotation = Quaternion.identity;
    }

    public void DestroyDisk()
    {
        if(disk)
        {
            disk.transform.parent = null;
            disk.gameObject.GetComponent<Collider>().enabled = false;
            Rigidbody diskRigidBody = disk.gameObject.AddComponent<Rigidbody>();
            diskRigidBody.AddTorque(new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50)), ForceMode.VelocityChange);
            diskRigidBody.AddForce(((disk.transform.position - Hero.heroRef.mainCamera.transform.position).normalized + (Vector3.up * 1.5f)) * Random.Range(4, 7), ForceMode.VelocityChange);
            disk.Throwed();
            disk = null;
        }
    }
}
