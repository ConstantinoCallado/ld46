﻿using System.Collections;
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
    private int armIdleAngle = -8;
    private int armStartingAngle = 0;
    private int armEndAngle = 28;

    public bool isSpinning = false;

    Coroutine spinCoroutine;

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
        if(isSpinning)
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
            disk.transform.Translate(disk.transform.up * 0.02f);
            disk.transform.parent = null;
            disk.gameObject.GetComponent<Collider>().enabled = false;
            Rigidbody diskRigidBody = disk.gameObject.AddComponent<Rigidbody>();
            diskRigidBody.AddTorque(new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50)), ForceMode.VelocityChange);
            diskRigidBody.AddForce(((disk.transform.position - Hero.heroRef.mainCamera.transform.position).normalized + (Vector3.up * 1.5f)) * Random.Range(4, 7), ForceMode.VelocityChange);
            disk.Throwed();
            disk = null;
        }
    }

    public void StartSpinning()
    {
        isSpinning = true;
        if (spinCoroutine != null) StopCoroutine(SpinCoroutine());
        spinCoroutine = StartCoroutine(SpinCoroutine());
    }

    public void StopSpinning()
    {
        isSpinning = false;
        if (spinCoroutine != null) StopCoroutine(spinCoroutine);
        RestoreArm();
    }

    public IEnumerator SpinCoroutine()
    {
        float startSongTime = Time.time;
        float songDuration = 0;

        if (disk)
        {
            songDuration = disk.duration;
        }

        Quaternion initialRotation = Quaternion.Euler(0, armStartingAngle, 0);
        Quaternion finalRotation = Quaternion.Euler(0, armEndAngle, 0);

        while (Time.time <= startSongTime + songDuration)
        {
            armPivot.localRotation = Quaternion.Lerp(initialRotation, finalRotation, (Time.time - startSongTime) / songDuration);
            yield return new WaitForEndOfFrame();
        }
    }
}
