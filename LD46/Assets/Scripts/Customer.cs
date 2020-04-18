using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        StartCoroutine(JumpRandom());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator JumpRandom()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 4f));
            rigidBody.AddForce(new Vector3(0, 2, 0), ForceMode.VelocityChange);
        }
    }
}
