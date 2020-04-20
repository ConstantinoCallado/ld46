using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraShake : MonoBehaviour
{
    // Transform of the GameObject you want to shake
    private Transform transform;

    // Desired duration of the shake effect
    public float globalShakeDuration = 20f;

    public float swingDurationHor = 5f;
    public float swingDurationVer = 2f;
    
    private float currentDurationHor = 0f;
    private float currentDurationVer = 0f;
    private bool backhome = false;
    private float signHor = 1;

    private float signVer = 1;

    // A measure of magnitude for the shake. Tweak based on your preference
    public float shakeMagnitudeHor = 0.02f;
    public float shakeMagnitudeVer = 0.01f;


    // A measure of how quickly the shake effect should evaporate
    private float dampingSpeed = 1.0f;

    // The initial position of the GameObject
    Vector3 initialPosition;


    void Awake()
    {
        if (transform == null)
        {
            transform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        initialPosition = transform.localPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (globalShakeDuration > 0)
        {
            if (currentDurationHor <= swingDurationHor)
            {

                currentDurationHor += Time.deltaTime;
                transform.localPosition += Vector3.right * shakeMagnitudeHor * signHor;
            }
            else 
            {
                if (backhome)
                {
                    backhome = false;

                }
                else {
                    signHor *= -1;
                    backhome = true;
                } 
                currentDurationHor = 0f;
            }


            if (currentDurationVer <= swingDurationVer)
            {
                currentDurationVer += Time.deltaTime;
                transform.localPosition += Vector3.up * shakeMagnitudeVer * signVer;
            }
            else
            {
                signVer *= -1;
                currentDurationVer = 0f;
            }


            //transform.localPosition = initialPosition + Vector3.right * shakeMagnitudeHor;
            //transform.localPosition.x = initialPosition.x + Random.insideUnitSphere.x * shakeMagnitudeHor;

            globalShakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            globalShakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }


    public void TriggerShake()
    {
        globalShakeDuration = 2.0f;
    }

}
