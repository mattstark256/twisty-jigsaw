using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// This class handles the main order of execution


[RequireComponent(typeof(CameraController), typeof(PointerInput), typeof(WipeTransition))]
public class GameController : MonoBehaviour
{
    [SerializeField]
    private PuzzleSequence puzzleSequence;
    [SerializeField]
    private GameObject continueButton;
    [SerializeField]
    private string mainMenuScene;

    //[SerializeField]
    //private PulseCircle pulsePrefab;

    private CameraController cameraController;
    private PointerInput pointerInput;
    private WipeTransition wipeTransition;

    Puzzle currentPuzzle;

    [SerializeField]
    int puzzleIndex = 0;


    private void Awake()
    {
        cameraController = GetComponent<CameraController>();
        pointerInput = GetComponent<PointerInput>();
        wipeTransition = GetComponent<WipeTransition>();
    }


    void Start()
    {
        LoadPuzzle();
    }


    private void Update()
    {
        // Handle pointer inputs
        pointerInput.HandleInput();

        // Check if the puzzle is solved
        if (currentPuzzle.IsSolved() && !continueButton.activeSelf)
        {
            continueButton.SetActive(true);
            continueButton.GetComponentInChildren<Text>().color = currentPuzzle.GetColorPalette().foregroundColor;
        }
    }


    public void StartInteraction(Vector3 position)
    {
        if (!currentPuzzle.IsSolved())
        {
            Vector3 worldPosition = cameraController.GetCamera().ScreenToWorldPoint(position);
            worldPosition.z = 0;
            currentPuzzle.StartInteraction(worldPosition);

            //PulseCircle pulse = Instantiate(pulsePrefab);
            //pulse.transform.position = worldPosition;
            //pulse.SetColor(currentPuzzle.GetColorPalette().foregroundColor);
        }
        else
        {
            wipeTransition.DoWipeTransition(position);
            LoadNextPuzzle();
        }
    }


    public void ContinueInteraction(Vector3 position)
    {
    }


    public void EndInteraction()
    {
    }


    public void LoadNextPuzzle()
    {
        if (puzzleSequence.IsLastPuzzle(puzzleIndex))
        {
            SceneManager.LoadScene(mainMenuScene);
        }
        else
        {
            Destroy(currentPuzzle.gameObject);
            puzzleIndex++;
            LoadPuzzle();
        }
    }


    private void LoadPuzzle()
    {
        currentPuzzle = Instantiate(puzzleSequence.GetPuzzle(puzzleIndex).prefab);
        currentPuzzle.Initialize();
        continueButton.SetActive(false);
        cameraController.SetBackgroundColor(currentPuzzle.GetColorPalette().backgroundColor);
    }
}
