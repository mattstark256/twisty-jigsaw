using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This class handles the main order of execution


[RequireComponent(typeof(CameraController), typeof(PointerInput))]
public class GameController : MonoBehaviour
{
    [SerializeField]
    private Puzzle puzzle;
    [SerializeField]
    private GameObject continueButton;

    private CameraController cameraController;
    private PointerInput pointerInput;


    private void Awake()
    {
        cameraController = GetComponent<CameraController>();
        pointerInput = GetComponent<PointerInput>();
    }


    void Start()
    {
        // Initialize puzzle
        puzzle.Initialize();

        // Set up camera position
        cameraController.Initialize(puzzle);

        // Make sure the continue button is disabled
        continueButton.SetActive(false);
    }


    private void Update()
    {
        // Handle pointer inputs
        pointerInput.HandleInput(cameraController.GetCamera(), puzzle);

        // Check if the puzzle is solved
        if (puzzle.IsSolved() && !continueButton.activeSelf) { continueButton.SetActive(true); }
    }
}
