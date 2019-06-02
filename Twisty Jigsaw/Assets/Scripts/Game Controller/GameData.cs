using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// This class handles the main order of execution


[RequireComponent(typeof(CameraController), typeof(PuzzleInput), typeof(WipeTransition))]
[RequireComponent(typeof(PuzzleManager), typeof(MenuManager), typeof(SaveData))]
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


    private void Awake()
    {
        cameraController = GetComponent<CameraController>();
        wipeTransition = GetComponent<WipeTransition>();
        puzzleManager = GetComponent<PuzzleManager>();
        menuManager = GetComponent<MenuManager>();
        saveData = GetComponent<SaveData>();
    }
}
