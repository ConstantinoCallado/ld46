using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Disk : MonoBehaviour
{
    public GameEnums.MusicColor musicColor;
    public float duration = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Throwed()
    {
        StartCoroutine(CoroutineThrow());
    }

    public IEnumerator CoroutineThrow()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
