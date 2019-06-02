using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(GameData))]
public class PuzzleManager : MonoBehaviour
{
    private GameData gameData;
    private PuzzleInput puzzleInput;

    [SerializeField]
    private RectTransform puzzleUIParent;
    [SerializeField]
    private Text continueTextPrefab;
    [SerializeField]
    private OpenMenuButton openMenuButtonPrefab;
    [SerializeField]
    private PulseCircle pulsePrefab;

    private int sequenceIndex = 0;
    private int puzzleIndex = 0;
    private Puzzle currentPuzzle = null;
    private bool currentPuzzleSolved = false;

    // This is true when the puzzle can be interacted with. This includes the interaction of continuing to the next puzzle. 
    private bool interactionsAvailable = false;
    public bool InteractionsAvailable() { return interactionsAvailable; }

    private Text continueText;
    private OpenMenuButton openMenuButton;


    private void Awake()
    {
        gameData = GetComponent<GameData>();
        puzzleInput = GetComponent<PuzzleInput>();

        openMenuButton = Instantiate(openMenuButtonPrefab, puzzleUIParent);
        openMenuButton.Initialize(gameData);
    }


    private void Update()
    {
        if (currentPuzzle == null) return;
        
        puzzleInput.HandleInput();

        // If the puzzle is complete
        if (currentPuzzle.IsSolved())
        {
            // If it has just been completed
            if (!currentPuzzleSolved)
            {
                currentPuzzleSolved = true;

                // Show "Continue" text
                continueText = Instantiate(continueTextPrefab, puzzleUIParent);
                continueText.color = gameData.GetSequenceSequence().GetSequence(sequenceIndex).GetColor(0, puzzleIndex);

                // Save any progress
                if (gameData.GetSequenceSequence().GetSequence(sequenceIndex).IsLastPuzzle(puzzleIndex))
                { gameData.GetSaveData().SequenceCompleted(sequenceIndex); }
                else
                { gameData.GetSaveData().PuzzleCompleted(puzzleIndex); }
            }

            if (puzzleInput.GetPointerPressState() == PressState.pressed)
            {
                gameData.GetWipeTransition().DoCircleWipe(puzzleInput.GetPointerScreenPosition());
                if (gameData.GetSequenceSequence().GetSequence(sequenceIndex).IsLastPuzzle(puzzleIndex))
                {
                    if (gameData.GetSequenceSequence().IsLastSequence(sequenceIndex))
                    {
                        // Go to "Game complete" screen
                        Debug.Log("Game complete!");
                        interactionsAvailable = false;
                    }
                    else
                    {
                        // Go to "Sequence complete" screen
                        Debug.Log("Sequence complete!");

                        LoadPuzzle(sequenceIndex + 1, 0);
                    }
                }
                else
                {
                    LoadPuzzle(sequenceIndex, puzzleIndex + 1);
                }
            }
        }

        // If the puzzle is incomplete
        else
        {
            switch (puzzleInput.GetPointerPressState())
            {
                case PressState.pressed:
                    CreatePulse(puzzleInput.GetPointerWorldPosition());
                    currentPuzzle.StartInteraction(puzzleInput.GetPointerWorldPosition());
                    break;
                case PressState.held:
                    currentPuzzle.ContinueInteraction(puzzleInput.GetPointerWorldPosition());
                    break;
                case PressState.released:
                    currentPuzzle.EndInteraction();
                    break;
            }
        }
    }


    public void LoadPuzzle(int newSequenceIndex, int newPuzzleIndex)
    {
        if (currentPuzzle != null) { Destroy(currentPuzzle.gameObject); }
        if (continueText != null) { Destroy(continueText); }

        sequenceIndex = newSequenceIndex;
        puzzleIndex = newPuzzleIndex;
        PuzzleSequence puzzleSequence = gameData.GetSequenceSequence().GetSequence(sequenceIndex);
        
        currentPuzzle = Instantiate(puzzleSequence.GetPuzzle(puzzleIndex).prefab);
        currentPuzzle.Initialize(puzzleSequence.GetColor(0, puzzleIndex));
        gameData.GetCameraController().SetBackgroundColor(puzzleSequence.GetColor(1, puzzleIndex));
        openMenuButton.SetColors(puzzleSequence.GetColor(0, puzzleIndex), puzzleSequence.GetColor(1, puzzleIndex));

        currentPuzzleSolved = false;
        interactionsAvailable = true;
    }


    private void CreatePulse(Vector3 position)
    {
        PulseCircle pulse = Instantiate(pulsePrefab);
        pulse.transform.position = position;
        if (currentPuzzle != null) { pulse.SetColor(gameData.GetSequenceSequence().GetSequence(sequenceIndex).GetColor(0, puzzleIndex)); }
    }
}