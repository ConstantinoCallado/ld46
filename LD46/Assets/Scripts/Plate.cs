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

    private Material onOffMaterial;

    // Trail
    public Transform needleSocket;
    public Transform needleTrail;
    public Transform trailTarget;

    // Music status
    public Table table;
    public GameEnums.MusicStatus musicStatus = GameEnums.MusicStatus.Blocked;
    public GameObject turntable;

    // Start is called before the first frame update
    void Start()
    {
        if(turntable != null)
        {
            Material[] turntableMaterials = turntable.GetComponent<Renderer>().materials;

            if (turntableMaterials.Length > 1)
                onOffMaterial = turntableMaterials[1];
        }

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
            needleTrail.Rotate(Vector3.up * diskRotationSpeed * Time.deltaTime, Space.Self);
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
            trailTarget.transform.position = needleSocket.position;
        }
    }

    public void SetDisk(Disk theDisk)
    {
        disk = theDisk;
        disk.transform.parent = anchor;
        disk.transform.localPosition = Vector3.zero;
        disk.transform.localRotation = Quaternion.identity;

        //DiskManager.diskManagerRef.UseDisk(disk);
    }

    public void EquipDisk(Disk theDisk)
    {
        if(disk)
        {
            DestroyDisk();
        }

        disk = theDisk;

        StartCoroutine(CorotuineEquipDisk());
    }

    IEnumerator CorotuineEquipDisk()
    {
        float launchStart = Time.time;
        float launchingDuration = 0.25f;

        disk.transform.parent = null;

        DiskManager.diskManagerRef.UseDisk(disk);

        Vector3 initialPosition = disk.transform.position;
        Quaternion initialRotation = disk.transform.rotation;

        while (Time.time <= launchStart + launchingDuration)
        {
            disk.transform.position = Vector3.Lerp(initialPosition, anchor.transform.position, (Time.time - launchStart) / launchingDuration);
            disk.transform.rotation = Quaternion.Lerp(initialRotation, anchor.rotation, (Time.time - launchStart) / launchingDuration);

            yield return new WaitForEndOfFrame();
        }

        disk.transform.parent = anchor;
        disk.transform.localPosition = Vector3.zero;
        disk.transform.localRotation = Quaternion.identity;

        AudioManager.audioManagerRef.PlaySound("sfx_put_record");

        //DiskManager.diskManagerRef.UseDisk(disk);
    }

    public void DestroyDisk()
    {
        if(disk)
        {
            //needleTrail.transform.parent = needleSocket;

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

        // set emission of the turntable dark material to true
        if(onOffMaterial != null)
            onOffMaterial.EnableKeyword("_EMISSION");
    }

    public void StopSpinning()
    {
        isSpinning = false;
        needleTrail.gameObject.SetActive(false);
        if (spinCoroutine != null) StopCoroutine(spinCoroutine);
        RestoreArm();

        // set emission of the turntable dark material to false
        if (onOffMaterial != null)
            onOffMaterial.DisableKeyword("_EMISSION");
    }

    public IEnumerator SpinCoroutine()
    {
        Color colorWhiteNeutral = new Vector4(1f, 1f, 1f, 1.0f);
        Color colorGreenPerfect = new Vector4(0f, 1f, 0f, 1.0f);
        Color colorRedBad = new Vector4(1f, 0f, 0f, 1.0f);

        needleTrail.GetComponent<Renderer>().material.SetColor("_Color", colorWhiteNeutral);
        needleTrail.GetComponent<Renderer>().material.SetColor("_EmissionColor", colorWhiteNeutral);

        yield return new WaitForEndOfFrame();
        needleTrail.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        needleTrail.GetComponent<TrailRenderer_Local>().Reset();

        float startSongTime = Time.time;
        float songDuration = AudioManager.audioManagerRef.GetCurrentRecordDuration();

        Quaternion initialRotation = Quaternion.Euler(0, armStartingAngle, 0);
        Quaternion finalRotation = Quaternion.Euler(0, armEndAngle, 0);

        musicStatus = GameEnums.MusicStatus.Blocked;

        while (Time.time <= startSongTime + songDuration * table.blockedTime)
        {
            armPivot.localRotation = Quaternion.Lerp(initialRotation, finalRotation, (Time.time - startSongTime) / songDuration);
            yield return new WaitForEndOfFrame();
        }

        musicStatus = GameEnums.MusicStatus.TooSoon;

        while (Time.time <= startSongTime + songDuration * (table.tooSoonTime+ table.blockedTime))
        {
            armPivot.localRotation = Quaternion.Lerp(initialRotation, finalRotation, (Time.time - startSongTime) / songDuration);
            yield return new WaitForEndOfFrame();
        }

        musicStatus = GameEnums.MusicStatus.Perfect;
        needleTrail.GetComponent<Renderer>().material.SetColor("_Color", colorGreenPerfect);
        needleTrail.GetComponent<Renderer>().material.SetColor("_EmissionColor", colorGreenPerfect);

        while (Time.time <= startSongTime + songDuration * (table.tooSoonTime + table.blockedTime + table.perfectTime))
        {
            armPivot.localRotation = Quaternion.Lerp(initialRotation, finalRotation, (Time.time - startSongTime) / songDuration);
            yield return new WaitForEndOfFrame();
        }

        musicStatus = GameEnums.MusicStatus.TooLate;

        needleTrail.GetComponent<Renderer>().material.SetColor("_Color", colorRedBad);
        needleTrail.GetComponent<Renderer>().material.SetColor("_EmissionColor", colorRedBad);
        //colorRedBad
        AudioManager.audioManagerRef.PlaySound("sfx_needle_skip");
        
        FXManager.fxManagerRef.MusicStopped();
    }
}
