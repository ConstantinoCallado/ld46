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
    private int armIdleAngle = -8;
    private int armStartingAngle = 0;
    private int armEndAngle = 28;

    public bool isSpinning = false;
    Coroutine spinCoroutine;

    // Trail
    public Transform needleSocket;
    public Transform needleTrail;

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
            /*Vector3 localDisplacement = needleSocket.position - needleTrail.transform.position;

            Debug.Log(localDisplacement);

            Vector3[] vertexPositions = new Vector3[needleTrail.positionCount];
            needleTrail.GetPositions(vertexPositions);
            for(int i=0; i<vertexPositions.Length; i++)
            {
                vertexPositions[i] = vertexPositions[i] + localDisplacement;
            }
            needleTrail.SetPositions(vertexPositions);*/

            //needleTrail.transform.position = needleSocket.position;
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

        needleTrail.transform.parent = disk.transform;
    }

    public void DestroyDisk()
    {
        if(disk)
        {
            needleTrail.transform.parent = needleSocket;

            disk.transform.Translate(disk.transform.up * 0.02f);
            disk.transform.parent = null;
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
        armPivot.localRotation = Quaternion.Euler(0, armStartingAngle, 0);
        if (spinCoroutine != null) StopCoroutine(SpinCoroutine());
        spinCoroutine = StartCoroutine(SpinCoroutine());
    }

    public void StopSpinning()
    {
        isSpinning = false;
        needleTrail.gameObject.SetActive(false);
        if (spinCoroutine != null) StopCoroutine(spinCoroutine);
        RestoreArm();
    }

    public IEnumerator SpinCoroutine()
    {
        yield return new WaitForEndOfFrame();
        needleTrail.gameObject.SetActive(true);

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
