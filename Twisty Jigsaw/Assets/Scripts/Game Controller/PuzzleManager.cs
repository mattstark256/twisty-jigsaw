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
                    // Show "Continue" text
                    continueText = Instantiate(continueTextPrefab, puzzleUIParent);
                    continueText.color = gameData.GetSequenceSequence().GetSequence(sequenceIndex).GetColor(0, puzzleIndex);

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
        }
    }


    public void LoadPuzzle(int newSequenceIndex, int newPuzzleIndex)
    {
        DestroyObjects();

        sequenceIndex = newSequenceIndex;
        puzzleIndex = newPuzzleIndex;

        puzzleNumberText = Instantiate(puzzleNumberTextPrefab, puzzleUIParent);
        puzzleNumberText.text = (sequenceIndex + 1) + " - " + (puzzleIndex + 1);

        PuzzleSequence puzzleSequence = gameData.GetSequenceSequence().GetSequence(sequenceIndex);
        currentPuzzle = Instantiate(puzzleSequence.GetPuzzle(puzzleIndex));
        currentPuzzle.Initialize(puzzleSequence.GetColor(0, puzzleIndex));

        if (currentPuzzle.GetTutorialText() != "")
        {
            tutorialText = Instantiate(tutorialTextPrefab, puzzleUIParent);
            tutorialText.text = currentPuzzle.GetTutorialText();
            tutorialText.color = puzzleSequence.GetColor(0, puzzleIndex);
        }

        puzzleNumberText.color = puzzleSequence.GetColor(0, puzzleIndex);
        gameData.GetCameraController().SetBackgroundColor(puzzleSequence.GetColor(1, puzzleIndex));
        openMenuButton.SetColor(puzzleSequence.GetColor(0, puzzleIndex));

        puzzleState = PuzzleState.unsolved;
    }


    private void SequenceComplete()
    {
        DestroyObjects();

        sequenceCompleteText = Instantiate(sequenceCompleteTextPrefab, puzzleUIParent);
        sequenceCompleteText.text = "Sequence " + (sequenceIndex + 1) + " complete!\nWell done!\nTap to continue";

        PuzzleSequence puzzleSequence = gameData.GetSequenceSequence().GetSequence(sequenceIndex);
        sequenceCompleteText.color = puzzleSequence.GetColor(0, puzzleIndex + 1);
        gameData.GetCameraController().SetBackgroundColor(puzzleSequence.GetColor(1, puzzleIndex + 1));
        openMenuButton.SetColor(puzzleSequence.GetColor(0, puzzleIndex + 1));

        puzzleState = PuzzleState.sequenceComplete;
    }


    private void GameComplete()
    {
        DestroyObjects();

        gameCompleteText = Instantiate(gameCompleteTextPrefab, puzzleUIParent);

        PuzzleSequence puzzleSequence = gameData.GetSequenceSequence().GetSequence(sequenceIndex);
        gameCompleteText.color = puzzleSequence.GetColor(0, puzzleIndex + 1);
        gameData.GetCameraController().SetBackgroundColor(puzzleSequence.GetColor(1, puzzleIndex + 1));
        openMenuButton.SetColor(puzzleSequence.GetColor(0, puzzleIndex + 1));

        puzzleState = PuzzleState.gameComplete;
    }


    private void CreatePulse(Vector3 position)
    {
        PulseCircle pulse = Instantiate(pulsePrefab);
        pulse.transform.position = position;
        if (currentPuzzle != null) { pulse.SetColor(gameData.GetSequenceSequence().GetSequence(sequenceIndex).GetColor(0, puzzleIndex)); }
    }


    private void DestroyObjects()
    {
        if (currentPuzzle != null) { Destroy(currentPuzzle.gameObject); }
        if (puzzleNumberText != null) { Destroy(puzzleNumberText); }
        if (continueText != null) { Destroy(continueText); }
        if (tutorialText != null) { Destroy(tutorialText); }
        if (sequenceCompleteText != null) { Destroy(sequenceCompleteText); }
        if (gameCompleteText != null) { Destroy(gameCompleteText); }
    }
}