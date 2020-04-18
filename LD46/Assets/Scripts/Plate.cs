using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public Transform anchor;

    public bool hasDisk = true;
    private float diskRotationSpeed = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hasDisk)
        {
            anchor.Rotate(Vector3.up * diskRotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}
