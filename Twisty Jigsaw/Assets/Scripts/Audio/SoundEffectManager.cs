using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // needed for Array.Find


[RequireComponent(typeof(VolumeController))]
public class SoundEffectManager : MonoBehaviour
{
    private VolumeController volumeControl;

    [SerializeField]
    private SoundEffect[] effects;


    private void Awake()
    {
        volumeControl = GetComponent<VolumeController>();
    }


#if UNITY_ANDROID && !UNITY_EDITOR

    private void Start()
    {
        AndroidNativeAudio.makePool(); // makes 16 streams by default
        foreach (SoundEffect effect in effects) { effect.fileID = AndroidNativeAudio.load(effect.filePath); }
    }

    public void PlayEffect(string effectName)
    {
        SoundEffect s = Array.Find(effects, effect => effect.name == effectName); // this uses a Lambda Expression
        int soundID = AndroidNativeAudio.play(s.fileID);
        AndroidNativeAudio.setVolume(soundID, s.volume * volumeControl.GetEffectsVolume());
    }

    void OnDestroy()
    {
        foreach (SoundEffect effect in effects) { AndroidNativeAudio.unload(effect.fileID); }
        AndroidNativeAudio.releasePool();
    }

#else

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        foreach (SoundEffect effect in effects)
        {
            // This bit is necessary for accessing audio clips from the StreamingAssets folder in non-Android versions.
            // For more info see the ANA guide.
            var www = new WWW("file:" + Application.streamingAssetsPath + "/" + effect.filePath);
            while (!www.isDone) { } // I think this waits until the clip is loaded
            effect.audioClip = www.GetAudioClip();
        }
    }

    public void PlayEffect(string effectName)
    {
        SoundEffect s = Array.Find(effects, effect => effect.name == effectName); // this uses a Lambda Expression
        audioSource.PlayOneShot(s.audioClip, s.volume * volumeControl.GetEffectsVolume());
    }

#endif
}