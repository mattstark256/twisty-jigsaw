using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// This class handles the main order of execution


[RequireComponent(
    typeof(CameraController),
    typeof(WipeTransition),
    typeof(PuzzleManager))]
[RequireComponent(
    typeof(MenuManager),
    typeof(SaveData),
    typeof(FramerateCounter))]
[RequireComponent(
    typeof(SoundEffectManager), 
    typeof(VolumeController))]
public class GameData : MonoBehaviour
{
    [SerializeField]
    private SequenceSequence sequenceSequence;
    public SequenceSequence GetSequenceSequence() { return sequenceSequence; }

    private CameraController cameraController;
    public CameraController GetCameraController() { return cameraController; }
    private WipeTransition wipeTransition;
    public WipeTransition GetWipeTransition() { return wipeTransition; }
    private PuzzleManager puzzleManager;
    public PuzzleManager GetPuzzleManager() { return puzzleManager; }
    private MenuManager menuManager;
    public MenuManager GetMenuManager() { return menuManager; }
    private SaveData saveData;
    public SaveData GetSaveData() { return saveData; }
    private FramerateCounter framerateCounter;
    public FramerateCounter GetFramerateCounter() { return framerateCounter; }
    private SoundEffectManager soundEffectManager;
    public SoundEffectManager GetSoundEffectManager() { return soundEffectManager; }
    private VolumeController volumeController;
    public VolumeController GetVolumeController() { return volumeController; }


    private void Awake()
    {
        cameraController = GetComponent<CameraController>();
        wipeTransition = GetComponent<WipeTransition>();
        puzzleManager = GetComponent<PuzzleManager>();
        menuManager = GetComponent<MenuManager>();
        saveData = GetComponent<SaveData>();
        framerateCounter = GetComponent<FramerateCounter>();
        soundEffectManager = GetComponent<SoundEffectManager>();
        volumeController = GetComponent<VolumeController>();
    }
}
