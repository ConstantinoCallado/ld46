using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public Plate plateLeft;
    public Plate plateRight;
    public CrossFader crossFader;

    public float blockedTime = 0.2f;
    public float tooSoonTime = 0.5f;
    public float perfectTime = 0.3f;

    public ParticleSystem particleTooSoon;
    public ParticleSystem particleSmooth;
    public ParticleSystem particleTooLate;

    private int numRecordsPlayed = 0;

    public void DoCrossFade()
    {
        if (crossFader.isLeft)
        {
            if (plateRight.disk)
            {
                switch (plateRight.musicStatus)
                {
                    case GameEnums.MusicStatus.Blocked:
                        break;
                    case GameEnums.MusicStatus.TooSoon:
                        particleTooSoon.Emit(1);
                        break;
                    case GameEnums.MusicStatus.Perfect:
                        particleSmooth.Emit(1);
                        break;
                    case GameEnums.MusicStatus.TooLate:
                        particleTooLate.Emit(1);
                        break;
                }
            }

            if (plateLeft.disk)
            {
                plateLeft.StartSpinning();
                //dancerManager.PerfectChange(plateLeft.disk.musicColor);

                if(plateRight.disk)
                {
                    switch (plateRight.musicStatus)
                    {
                        case GameEnums.MusicStatus.Blocked:
                            DancerManager.dancerManagerRef.TooSoonChange(plateLeft.disk.musicColor);
                            break;
                        case GameEnums.MusicStatus.TooSoon:
                            DancerManager.dancerManagerRef.TooSoonChange(plateLeft.disk.musicColor);
                            break;
                        case GameEnums.MusicStatus.Perfect:
                            DancerManager.dancerManagerRef.PerfectChange(plateLeft.disk.musicColor);
                            break;
                        case GameEnums.MusicStatus.TooLate:
                            DancerManager.dancerManagerRef.TooLateChange();
                            break;
                    }
                }
                else
                {
                    DancerManager.dancerManagerRef.PerfectChange(plateLeft.disk.musicColor);
                }
                
                AudioManager.audioManagerRef.PlayRecord(GameEnums.TurnTable.Left, plateLeft.disk.musicColor); // Plays the music for the left turntable
                numRecordsPlayed++;

                FXManager.fxManagerRef.MusicStarted(plateLeft.disk.musicColor);
            }
            else
            {
                FXManager.fxManagerRef.MusicStopped();
            }
            AudioManager.audioManagerRef.StopRecord(GameEnums.TurnTable.Right); // Stops the right turntable record
            plateRight.DestroyDisk();
            plateRight.StopSpinning();
        }
        else
        {
            if (plateLeft.disk)
            {
                switch (plateLeft.musicStatus)
                {
                    case GameEnums.MusicStatus.Blocked:
                        break;
                    case GameEnums.MusicStatus.TooSoon:
                        particleTooSoon.Emit(1);
                        break;
                    case GameEnums.MusicStatus.Perfect:
                        particleSmooth.Emit(1);
                        break;
                    case GameEnums.MusicStatus.TooLate:
                        particleTooLate.Emit(1);
                        break;
                }
            }

            if (plateRight.disk)
            {
                plateRight.StartSpinning();

                if (plateLeft.disk)
                {
                    switch (plateLeft.musicStatus)
                    {
                        case GameEnums.MusicStatus.Blocked:
                            DancerManager.dancerManagerRef.TooSoonChange(plateRight.disk.musicColor);
                            break;
                        case GameEnums.MusicStatus.TooSoon:
                            DancerManager.dancerManagerRef.TooSoonChange(plateRight.disk.musicColor);
                            break;
                        case GameEnums.MusicStatus.Perfect:
                            DancerManager.dancerManagerRef.PerfectChange(plateRight.disk.musicColor);
                            break;
                        case GameEnums.MusicStatus.TooLate:
                            DancerManager.dancerManagerRef.TooLateChange();
                            break;
                    }
                }
                else
                {
                    DancerManager.dancerManagerRef.PerfectChange(plateRight.disk.musicColor);
                }

                if(numRecordsPlayed > 0)
                {
                    AudioManager.audioManagerRef.PlayRecord(GameEnums.TurnTable.Right, plateRight.disk.musicColor);  // Plays the music for the right turntable
                }
                else
                {
                    AudioManager.audioManagerRef.PlayDefaultRecord(GameEnums.TurnTable.Right);  // Plays the music for the right turntable
                }
                numRecordsPlayed++;

                FXManager.fxManagerRef.MusicStarted(plateRight.disk.musicColor);
            }
            else
            {
                FXManager.fxManagerRef.MusicStopped();
            }
            AudioManager.audioManagerRef.StopRecord(GameEnums.TurnTable.Left); // Stops the right turntable record
            plateLeft.DestroyDisk();
            plateLeft.StopSpinning();
        }
    }
}
