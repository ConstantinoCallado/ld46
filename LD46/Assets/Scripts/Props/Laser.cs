using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject beamPrefab;
    public Transform laserHead;
    private float beamLength = 15f;
    public int beamAmount = 6;
    public float beamsAperture = 10;

    private List<LineRenderer> beams = new List<LineRenderer>();

    private Coroutine openCoroutine;
    private Coroutine rotateCoroutine;
    private Coroutine aimCoroutine;

    void Awake()
    {
        ResetLasers();
    }

    void ResetLasers()
    {
        for (int i = 0; i < beams.Count; i++)
        {
            Destroy(beams[i].gameObject);
        }
        beams.Clear();

        for (int i = 0; i < beamAmount; i++)
        {
            Transform beamInstance = GameObject.Instantiate(beamPrefab, transform.position, Quaternion.identity).transform;
            beamInstance.parent = laserHead;
            beams.Add(beamInstance.GetComponent<LineRenderer>());
        }

        SetAperture(beamsAperture);
    }

    public void EnableLasers(bool param)
    {
        laserHead.gameObject.SetActive(param);
    }

    void SetAperture(float aperture)
    {
        aperture = Mathf.Clamp(aperture, 1, 10) * Mathf.PI / 180 / 2;

        for (int i = 0; i < beamAmount; i++)
        {
            float angle = i * 2 * Mathf.PI / beamAmount;
            //beams[i].SetPosition(1, new Vector3(beamLength, beamsAperture * Mathf.Sin(angle), beamsAperture * Mathf.Cos(angle)));
            beams[i].SetPosition(1, new Vector3(beamsAperture * Mathf.Sin(angle), beamsAperture * Mathf.Cos(angle), beamLength));
        }
    }

    public void ChangeAperture(float newAperture, float apertureTime)
    {
        if (openCoroutine != null) StopCoroutine(openCoroutine);
        openCoroutine = StartCoroutine(ChangeApertureCoroutine(newAperture, apertureTime));
    }

    IEnumerator ChangeApertureCoroutine(float newAperture, float apertureTime)
    {
        float initialAperture = beamsAperture;
        float initialTime = Time.time;

        while (Time.time < initialTime + apertureTime)
        {
            beamsAperture = Mathf.Lerp(initialAperture, newAperture, (Time.time - initialTime) / apertureTime);
            SetAperture(beamsAperture);
            yield return new WaitForEndOfFrame();
        }
    }

    public void RotateHead(float rotationSpeed, float rotationTime)
    {
        if (rotateCoroutine != null) StopCoroutine(rotateCoroutine);
        rotateCoroutine = StartCoroutine(RotateHeadCoroutine(rotationSpeed, rotationTime));
    }

    IEnumerator RotateHeadCoroutine(float rotationSpeed, float rotationTime)
    {
        float initialTime = Time.time;

        while (Time.time < initialTime + rotationTime)
        {
            laserHead.localRotation = laserHead.localRotation * Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public void Aim(Vector3 targetPoint, float aimTime)
    {
        if (aimCoroutine != null) StopCoroutine(aimCoroutine);
        aimCoroutine = StartCoroutine(AimToCoroutine(targetPoint, aimTime));
    }

    IEnumerator AimToCoroutine(Vector3 targetPoint, float aimTime)
    {
        float initialTime = Time.time;

        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position, transform.right);

        //transform.Rotate(0, -90, 0);

        while (Time.time < initialTime + aimTime)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, (Time.time - initialTime) / aimTime);
            //transform.Rotate(0, -90, 0);

            yield return new WaitForEndOfFrame();
        }
    }
}
