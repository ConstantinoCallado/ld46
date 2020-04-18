using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public Plate plateLeft;
    public Plate plateRight;

    public CrossFader crossFader;
    public DancerManager dancerManager;


    public void DoCrossFade()
    {
        if(crossFader.isLeft)
        {
            if (plateLeft.disk)
            {
                Debug.Log("Table doing CrossFade!");
                dancerManager.PerfectChange(plateLeft.disk.musicColor);
            }
            plateRight.DestroyDisk();
        }
        else
        {
            if (plateRight.disk)
            {
                Debug.Log("Table doing CrossFade!");
                dancerManager.PerfectChange(plateRight.disk.musicColor);
            }
            plateLeft.DestroyDisk();
        }
    }
}
