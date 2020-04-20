using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public Plate plateLeft;
    public Plate plateRight;
    public CrossFader crossFader;

    public GameObject crossFaderGauge;
    private Material gaugeMaterial = null;

    public float blockedTime = 0.2f;
    public float tooSoonTime = 0.5f;
    public float perfectTime = 0.3f;

    public DancerManager dancerManager;

    public ParticleSystem particleTooSoon;
    public ParticleSystem particleSmooth;
    public ParticleSystem particleTooLate;

    void Start()
    {
        if (crossFaderGauge != null)
            gaugeMaterial = crossFaderGauge.GetComponent<Renderer>().material;
    }

    public void DoCrossFade()
    {
        if(gaugeMaterial != null)
            gaugeMaterial.DisableKeyword("_EMISSION");

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
                dancerManager.PerfectChange(plateLeft.disk.musicColor);

                if(plateRight.disk)
                {
                    switch (plateRight.musicStatus)
                    {
                        case GameEnums.MusicStatus.Blocked:
                            dancerManager.TooSoonChange(plateLeft.disk.musicColor);
                            break;
                        case GameEnums.MusicStatus.TooSoon:
                            dancerManager.TooSoonChange(plateLeft.disk.musicColor);
                            break;
                        case GameEnums.MusicStatus.Perfect:
                            dancerManager.PerfectChange(plateLeft.disk.musicColor);
                            break;
                        case GameEnums.MusicStatus.TooLate:
                            dancerManager.TooLateChange(plateLeft.disk.musicColor);
                            break;
                    }
                }
                
                AudioManager.audioManagerRef.PlayRecord(GameEnums.TurnTable.Left, plateLeft.disk.musicColor); // Plays the music for the left turntable

                if (gaugeMaterial != null)
                    gaugeMaterial.EnableKeyword("_EMISSION");
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
                            dancerManager.TooSoonChange(plateRight.disk.musicColor);
                            break;
                        case GameEnums.MusicStatus.TooSoon:
                            dancerManager.TooSoonChange(plateRight.disk.musicColor);
                            break;
                        case GameEnums.MusicStatus.Perfect:
                            dancerManager.PerfectChange(plateRight.disk.musicColor);
                            break;
                        case GameEnums.MusicStatus.TooLate:
                            dancerManager.TooLateChange(plateRight.disk.musicColor);
                            break;
                    }
                }
                AudioManager.audioManagerRef.PlayRecord(GameEnums.TurnTable.Right, plateRight.disk.musicColor);  // Plays the music for the right turntable

                if (gaugeMaterial != null)
                    gaugeMaterial.EnableKeyword("_EMISSION");

            }
            AudioManager.audioManagerRef.StopRecord(GameEnums.TurnTable.Left); // Stops the right turntable record
            plateLeft.DestroyDisk();
            plateLeft.StopSpinning();
        }
    }
}
