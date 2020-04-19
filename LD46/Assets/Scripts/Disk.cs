using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Disk : MonoBehaviour
{
    private bool _broken = false;
    public GameEnums.MusicColor musicColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Throwed()
    {
        AudioManager.audioManagerRef.PlaySound("sfx_throw");
        StartCoroutine(CoroutineThrow());
    }

    public IEnumerator CoroutineThrow()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_broken && collision.gameObject.tag == "layout") 
        {
            AudioManager.audioManagerRef.PlaySound("sfx_broken_record");
            _broken = true;
        }

    }
}
