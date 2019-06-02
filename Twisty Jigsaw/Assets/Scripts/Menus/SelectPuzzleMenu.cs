using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectPuzzleMenu : MenuScreen
{
    [SerializeField]
    private SelectPuzzleButton buttonPrefab;
    [SerializeField]
    private RectTransform buttonParent;
    [SerializeField]
    private ScrollRect scrollRect;


    public override void Initialize(GameData _gameData)
    {
        base.Initialize(_gameData);
    }


    public void InitializeForSequence(int sequenceIndex)
    {
        for (int i = 0; i < gameData.GetSequenceSequence().GetSequence(sequenceIndex).GetPuzzleCount(); i++)
        {
            SelectPuzzleButton newButton = Instantiate(buttonPrefab, buttonParent);
            newButton.Initialize(gameData, sequenceIndex, i);
        }

        scrollRect.verticalNormalizedPosition = 1;
    }


    public void GoBack()
    {
        gameData.GetMenuManager().OpenSequenceSelect();
    }
}
