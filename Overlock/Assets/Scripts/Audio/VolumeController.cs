using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    [SerializeField]
    private float defaultEffectsVolume = 0.7f;


    private float effectsVolume;
    public float GetEffectsVolume() { return effectsVolume; }
    public void SetEffectsVolume(float newVolume)
    {
        effectsVolume = newVolume;
        PlayerPrefs.SetFloat("Effects Volume", effectsVolume);
    }


    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Effects Volume")) { PlayerPrefs.SetFloat("Effects Volume", defaultEffectsVolume); }

        effectsVolume = PlayerPrefs.GetFloat("Effects Volume");
    }
}
