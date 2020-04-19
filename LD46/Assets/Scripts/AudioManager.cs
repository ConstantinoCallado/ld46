using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    private AudioSource _leftTurnTableAudioSource;
    private AudioSource _rightTurnTableAudioSource;

    private AudioSource _currentTurnTableAudioSource = null;

    private int musicSamplesAIndex;
    private int musicSamplesBIndex;
    private int musicSamplesCIndex;

    public static AudioManager audioManagerRef;

    // list of gameplay sounds
    public Sound[] sounds;

    // list of music samples
    public Sound[] musicSamplesA;
    public Sound[] musicSamplesB;
    public Sound[] musicSamplesC;

    void Awake()
    {
        audioManagerRef = this;

        // Initializing the music sample Audio sources
        musicSamplesAIndex = 0;
        musicSamplesBIndex = 0;
        musicSamplesCIndex = 0;

        Component[] turntableAudioSources = GetComponents(typeof(AudioSource));

        if(turntableAudioSources.Length > 0)
            _leftTurnTableAudioSource = turntableAudioSources[0] as AudioSource;
        if (turntableAudioSources.Length > 1)
            _rightTurnTableAudioSource = turntableAudioSources[1] as AudioSource;

        musicSamplesAIndex = musicSamplesBIndex = musicSamplesCIndex = 0;

        // Initializing other sounds
        foreach (Sound s in sounds) 
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void PlaySound(string name) 
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void StopSound(string name)
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name);
        if (s == null) 
        {
            Debug.LogWarning("StopSound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public void PlayRecord(GameEnums.TurnTable turntable, GameEnums.MusicColor recordType) 
    {
        AudioSource turntableAudioSource = null;
        AudioSource otherTurntableAudioSource = null;
        Sound musicSample = null;

        switch (turntable) 
        {
            case GameEnums.TurnTable.Left:
                turntableAudioSource = _leftTurnTableAudioSource;
                otherTurntableAudioSource = _rightTurnTableAudioSource;
                break;
            case GameEnums.TurnTable.Right:
                turntableAudioSource = _rightTurnTableAudioSource;
                otherTurntableAudioSource = _leftTurnTableAudioSource;
                break;
            default:
                Debug.LogWarning("Invalid turntable: " + turntable.ToString());
                break;
        }

        if (turntableAudioSource == null || otherTurntableAudioSource == null) 
        {
            Debug.LogWarning("An AudioSource component for the turntable is missing. Please add both AudioSource components to the AudioManager object.");
            return;
        }

        musicSample = GetMusicSample(recordType);

        if (musicSample == null)
            return;

        otherTurntableAudioSource.Stop(); // TO DO : Should be crossfaded

        turntableAudioSource.clip = musicSample.clip;
        turntableAudioSource.volume = musicSample.volume;
        turntableAudioSource.pitch = musicSample.pitch;
        turntableAudioSource.loop = musicSample.loop;
        turntableAudioSource.Play();

        _currentTurnTableAudioSource = turntableAudioSource;
    }

    public float GetCurrentRecordDuration() 
    {
        if (_currentTurnTableAudioSource == null)
            return 0f;
        else
            return _currentTurnTableAudioSource.clip.length;
    }

    public void StopRecord(GameEnums.TurnTable turntable) 
    {
        AudioSource turntableAudioSource = null;

        switch (turntable)
        {
            case GameEnums.TurnTable.Left:
                turntableAudioSource = _leftTurnTableAudioSource;
                break;
            case GameEnums.TurnTable.Right:
                turntableAudioSource = _rightTurnTableAudioSource;
                break;
            default:
                Debug.LogWarning("Invalid turntable: " + turntable.ToString());
                break;
        }

        if (turntableAudioSource == null)
        {
            Debug.LogWarning("An AudioSource component for the turntable is missing. Please add both AudioSource components to the AudioManager object.");
            return;
        }

        turntableAudioSource.Stop(); // TO DO : Should be crossfaded
    }

    private Sound GetMusicSample(GameEnums.MusicColor recordType) 
    {
        Sound musicSample = null;
        Sound[] musicSampleList = null;
        ref int musicSampleIndex = ref musicSamplesAIndex;

        switch (recordType)
        {
            case GameEnums.MusicColor.Magenta:
                musicSampleList = musicSamplesA;
                break;
            case GameEnums.MusicColor.Cyan:
                musicSampleList = musicSamplesB;
                musicSampleIndex = ref musicSamplesBIndex;
                break;
            case GameEnums.MusicColor.Yellow:
                musicSampleList = musicSamplesC;
                musicSampleIndex = ref musicSamplesCIndex;
                break;
            default:
                break;
        }

        if (musicSampleList == null) 
            Debug.LogWarning("The record type " + recordType.ToString() + "doesn't exists.");

        if (musicSampleList.Length == 0)
            Debug.LogWarning("There are no music samples for the record type " + recordType.ToString() + ". Please, add music samples in the AudioManager.");

        if(musicSampleList != null && musicSampleList.Length > 0) { 
            musicSample = musicSampleList[musicSampleIndex];

            if (musicSampleIndex == musicSampleList.Length - 1)
                musicSampleIndex = 0;
            else
                musicSampleIndex++;
        }

        return musicSample;
    }
}
