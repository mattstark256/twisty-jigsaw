using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundEffect
{
    public string name;

    [Tooltip("This should be in the StreamingAssets folder. Don't forget the file extension!")]
    public string filePath;

    [Range(0f, 1f)]
    public float volume = 0.8f;

    [HideInInspector]
    public AudioClip audioClip; // This is set up on start using WWW. See SoundEffectManager.
    [HideInInspector]
    public int fileID = 0; // This is used to keep track of the file once it has been loaded at the start.
}
