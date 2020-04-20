using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingSprite : MonoBehaviour
{

    private Transform transform;


    public float bounceDuration = 2f;
    private float currentDuration = 0f;
    private float sign = 1;

    public float shakeMagnitude = 0.01f;


    void Awake()
    {
        if (transform == null)
        {
            transform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (currentDuration <= bounceDuration)
        {
            currentDuration += Time.deltaTime;
            transform.position += transform.up * shakeMagnitude * sign * Time.deltaTime;
        }
        else
        {
            sign *= -1;
            currentDuration = 0f;
        }

    }



    public void TriggerShake()
    {
        
    }

}
