using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum PuzzleState { notLoaded, unsolved, solved, sequenceComplete, gameComplete };


[RequireComponent(typeof(GameData))]
public class PuzzleManager : MonoBehaviour
{
    private GameData gameData;
    private PuzzleInput puzzleInput;

    [SerializeField]
    private RectTransform puzzleUIParent;

    [SerializeField]
    private Text puzzleNumberTextPrefab;
    [SerializeField]
    private Text solvedTextPrefab;
    [SerializeField]
    private Text continueTextPrefab;
    [SerializeField]
    private Text tutorialTextPrefab;
    [SerializeField]
    private Text sequenceCompleteTextPrefab;
    [SerializeField]
    private Text gameCompleteTextPrefab;
    [SerializeField]
    private OpenMenuButton openMenuButtonPrefab;
    [SerializeField]
    private PulseCircle pulsePrefab;

    private int sequenceIndex = 0;
    private int puzzleIndex = 0;
    private Puzzle currentPuzzle = null;

    private Text puzzleNumberText;
    private Text solvedText;
    private Text continueText;
    private Text tutorialText;
    private Text sequenceCompleteText;
    private Text gameCompleteText;
    private OpenMenuButton openMenuButton;

    private PuzzleState puzzleState = PuzzleState.notLoaded;
    public PuzzleState GetPuzzleState() { return puzzleState; }


    private void Awake()
    {
        gameData = GetComponent<GameData>();
        puzzleInput = GetComponent<PuzzleInput>();

        openMenuButton = Instantiate(openMenuButtonPrefab, puzzleUIParent);
        openMenuButton.Initialize(gameData);
    }


    private void Update()
    {
        puzzleInput.HandleInput();

        switch (puzzleState)
        {
            case PuzzleState.unsolved:
                // Pass input to puzzle
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

                // Check if it's complete
                if (currentPuzzle.IsSolved())
                {
                    // Show "Solved" text
                    solvedText = Instantiate(solvedTextPrefab, puzzleUIParent);
                    continueText = Instantiate(continueTextPrefab, puzzleUIParent);
                    UpdateColors(sequenceIndex, puzzleIndex);

                    // Save any progress
                    if (gameData.GetSequenceSequence().GetSequence(sequenceIndex).IsLastPuzzle(puzzleIndex))
                    { gameData.GetSaveData().SequenceCompleted(sequenceIndex); }
                    else
                    { gameData.GetSaveData().PuzzleCompleted(puzzleIndex); }

                    puzzleState = PuzzleState.solved;
                }
                break;


            case PuzzleState.solved:
                if (puzzleInput.GetPointerPressState() == PressState.pressed)
                {
                    gameData.GetWipeTransition().DoIrisWipe(puzzleInput.GetPointerScreenPosition());
                    if (gameData.GetSequenceSequence().GetSequence(sequenceIndex).IsLastPuzzle(puzzleIndex))
                    {
                        if (gameData.GetSequenceSequence().IsLastSequence(sequenceIndex))
                        { GameComplete(); }
                        else
                        { SequenceComplete(); }
                    }
                    else
                    {
                        LoadPuzzle(sequenceIndex, puzzleIndex + 1);
                    }
                }
                break;


            case PuzzleState.sequenceComplete:
                if (puzzleInput.GetPointerPressState() == PressState.pressed)
                {
                    gameData.GetWipeTransition().DoIrisWipe(puzzleInput.GetPointerScreenPosition());
                    LoadPuzzle(sequenceIndex + 1, 0);
                }
                break;


            case PuzzleState.gameComplete:
                if (puzzleInput.GetPointerPressState() == PressState.pressed)
                {
                    DestroyObjects();
                    puzzleState = PuzzleState.notLoaded;
                    gameData.GetMenuManager().OpenMainMenu();
                }
                break;
        }
    }


    public void LoadPuzzle(int newSequenceIndex, int newPuzzleIndex)
    {
        DestroyObjects();

        sequenceIndex = newSequenceIndex;
        puzzleIndex = newPuzzleIndex;

        puzzleNumberText = Instantiate(puzzleNumberTextPrefab, puzzleUIParent);
        puzzleNumberText.text = (sequenceIndex + 1) + " - " + (puzzleIndex + 1);
        
        currentPuzzle = Instantiate(gameData.GetSequenceSequence().GetSequence(sequenceIndex).GetPuzzle(puzzleIndex));
        currentPuzzle.Initialize(gameData.GetSequenceSequence().GetSequence(sequenceIndex).GetColor(0, puzzleIndex));

        if (currentPuzzle.GetTutorialText() != "")
        {
            tutorialText = Instantiate(tutorialTextPrefab, puzzleUIParent);
            tutorialText.text = currentPuzzle.GetTutorialText();
        }

        UpdateColors(sequenceIndex, puzzleIndex);

        puzzleState = PuzzleState.unsolved;
    }


    private void SequenceComplete()
    {
        DestroyObjects();

        sequenceCompleteText = Instantiate(sequenceCompleteTextPrefab, puzzleUIParent);
        sequenceCompleteText.text = "Sequence " + (sequenceIndex + 1) + " complete!\nWell done!";
        continueText = Instantiate(continueTextPrefab, puzzleUIParent);

        UpdateColors(sequenceIndex, puzzleIndex + 1);

        puzzleState = PuzzleState.sequenceComplete;
    }


    private void GameComplete()
    {
        DestroyObjects();

        gameCompleteText = Instantiate(gameCompleteTextPrefab, puzzleUIParent);
        continueText = Instantiate(continueTextPrefab, puzzleUIParent);

        UpdateColors(sequenceIndex, puzzleIndex + 1);

        puzzleState = PuzzleState.gameComplete;
    }


    private void CreatePulse(Vector3 position)
    {
        PulseCircle pulse = Instantiate(pulsePrefab);
        pulse.transform.position = position;
        if (currentPuzzle != null) { pulse.SetColor(gameData.GetSequenceSequence().GetSequence(sequenceIndex).GetColor(0, puzzleIndex)); }
    }


    // Destroy spawned objects
    private void DestroyObjects()
    {
        if (currentPuzzle != null) { Destroy(currentPuzzle.gameObject); }
        if (puzzleNumberText != null) { Destroy(puzzleNumberText.gameObject); }
        if (solvedText != null) { Destroy(solvedText.gameObject); }
        if (continueText != null) { Destroy(continueText.gameObject); }
        if (tutorialText != null) { Destroy(tutorialText.gameObject); }
        if (sequenceCompleteText != null) { Destroy(sequenceCompleteText.gameObject); }
        if (gameCompleteText != null) { Destroy(gameCompleteText.gameObject); }
    }


    // Get the colors for the specified sequence and puzzle and apply them to any game UI elements
    private void UpdateColors(int colorSequenceIndex, int colorPuzzleIndex)
    {
        Color foregroundColor = gameData.GetSequenceSequence().GetSequence(colorSequenceIndex).GetColor(0, colorPuzzleIndex);
        Color backgroundColor = gameData.GetSequenceSequence().GetSequence(colorSequenceIndex).GetColor(1, colorPuzzleIndex);

        openMenuButton.SetColor(foregroundColor);
        if (puzzleNumberText != null) { puzzleNumberText.color = foregroundColor; }
        if (solvedText != null) { solvedText.color = foregroundColor; }
        if (continueText != null) { continueText.color = foregroundColor; }
        if (tutorialText != null) { tutorialText.color = foregroundColor; }
        if (sequenceCompleteText != null) { sequenceCompleteText.color = foregroundColor; }
        if (gameCompleteText != null) { gameCompleteText.color = foregroundColor; }

        gameData.GetCameraController().SetBackgroundColor(backgroundColor);
    }
}