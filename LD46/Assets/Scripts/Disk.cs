using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SongColor { White, Magenta, Cyan, Yellow };

public class Disk : MonoBehaviour
{
    public SongColor songColor;
    public float duration = 10f;
    public AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
