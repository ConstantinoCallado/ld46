using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public static LaserManager laserManagerRef;
    public Laser laserRight;
    public Laser laserLeft;

    // laser aperture
    [Header("Laser aperture")]
    public float minApertureValue = 1;
    public float maxApertureValue = 10;
    public float minApertureTime = 0.5f;
    public float maxApertureTime = 3;
    public float minDelayBetweenApertures = 1;
    public float maxDelayBetweenApertures = 6;

    // laser rotation
    [Header("Laser rotation")]
    public float minRotationSpeed = 50;
    public float maxRotationSpeed = 80;
    public float minRotationTime = 3;
    public float maxRotationTime = 7;
    public float minDelayBetweenRotations = 4;
    public float maxDelayBetweenRotations = 9;

    // laser aim
    [Header("Laser aiming")]
    public Vector3 leftBottomCorner;
    public Vector3 rightUpCorner;
    public float minAimTime = 1;
    public float maxAimTime = 2;
    public float minDelayBetweenAims = 3;
    public float maxDelayBetweenAims = 8;

    public Material laserMaterial;

    void Awake()
    { 
        laserManagerRef = this; 
    }

    void Start()
    {
        laserRight.EnableLasers(false);
        laserLeft.EnableLasers(false);

        StartCoroutine(RandomizeAperture());
        StartCoroutine(RandomizeRotation());
        StartCoroutine(RandomizeAimTo());
    }

    IEnumerator RandomizeAperture()
    {
        while(true)
        {
            float apertureValue = Random.Range(minApertureValue, maxApertureValue);
            float apertureTime = Random.Range(minApertureTime, maxApertureTime);

            laserRight.ChangeAperture(apertureValue, apertureTime);
            laserLeft.ChangeAperture(apertureValue, apertureTime);

            yield return new WaitForSeconds(Random.Range(minDelayBetweenApertures, maxDelayBetweenApertures));
        }
    }

    IEnumerator RandomizeRotation()
    {
        while(true)
        {
            while (true)
            {
                float rotationTime = Random.Range(minRotationTime, maxRotationTime);
                float rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
                if (Random.Range(0, 100) > 50) rotationSpeed = -rotationSpeed;

                laserRight.RotateHead(rotationSpeed, rotationTime);
                laserLeft.RotateHead(-rotationSpeed, rotationTime);

                yield return new WaitForSeconds(Random.Range(minDelayBetweenRotations, maxDelayBetweenRotations));
            }
        }
    }

    IEnumerator RandomizeAimTo()
    {
        while (true)
        {
            while (true)
            {
                Vector3 randomTargetR = new Vector3(Random.Range(leftBottomCorner.x, rightUpCorner.x), 
                                                      Random.Range(leftBottomCorner.y, rightUpCorner.y), 
                                                      Random.Range(leftBottomCorner.z, rightUpCorner.z));

                Vector3 randomTargetL = new Vector3(-randomTargetR.x, randomTargetR.y, randomTargetR.z);

                float aimTime = Random.Range(minAimTime, maxAimTime);
                
                laserRight.Aim(randomTargetR, aimTime);
                laserLeft.Aim(randomTargetL, aimTime);

                yield return new WaitForSeconds(Random.Range(minDelayBetweenAims, maxDelayBetweenAims));
            }
        }
    }

    public void MusicStopped()
    {
        laserRight.EnableLasers(false);
        laserLeft.EnableLasers(false);
    }

    public void MusicStarted(GameEnums.MusicColor musicColor)
    {
        Debug.Log("Music changed to " + musicColor);
        Color cyan = new Vector4(0f, 2f, 1.5f, 1.0f);
        Color magenta = new Vector4(2f, 0f, 1.6f, 1f);
        Color yellow = new Vector4(2f, 1.5f, 0f, 1.0f);

        laserRight.EnableLasers(true);
        laserLeft.EnableLasers(true);

        switch(musicColor)
        {
            case GameEnums.MusicColor.Magenta:
                laserMaterial.SetColor("_EmissionColor", magenta);
                break;
            case GameEnums.MusicColor.Cyan:
                laserMaterial.SetColor("_EmissionColor", cyan);
                break;
            case GameEnums.MusicColor.Yellow:
                laserMaterial.SetColor("_EmissionColor", yellow);
                break;
            default:
                break;
        }
    }
}
