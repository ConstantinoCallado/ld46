﻿using System.Collections;
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
                plateLeft.StartSpinning();
                dancerManager.PerfectChange(plateLeft.disk.musicColor);
                AudioManager.audioManagerRef.PlayRecord(GameEnums.TurnTable.Left, plateLeft.disk.musicColor); // Plays the music for the left turntable
            }
            AudioManager.audioManagerRef.StopRecord(GameEnums.TurnTable.Right); // Stops the right turntable record
            plateRight.DestroyDisk();
            plateRight.StopSpinning();
        }
        else
        {
            if (plateRight.disk)
            {
                Debug.Log("Table doing CrossFade!");
                plateRight.StartSpinning();
                dancerManager.PerfectChange(plateRight.disk.musicColor);
                AudioManager.audioManagerRef.PlayRecord(GameEnums.TurnTable.Right, plateRight.disk.musicColor);  // Plays the music for the right turntable
            }
            AudioManager.audioManagerRef.StopRecord(GameEnums.TurnTable.Left); // Stops the right turntable record
            plateLeft.DestroyDisk();
            plateLeft.StopSpinning();
        }
    }
}
