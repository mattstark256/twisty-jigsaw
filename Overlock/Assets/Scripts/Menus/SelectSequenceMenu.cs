using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectSequenceMenu : MenuScreen
{
    [SerializeField]
    private SelectSequenceButton buttonPrefab;
    [SerializeField]
    private RectTransform buttonParent;
    [SerializeField]
    private ScrollRect scrollRect;


    public override void Initialize(GameData _gameData)
    {
        base.Initialize(_gameData);

        for (int i = 0; i < gameData.GetSequenceSequence().GetSequenceCount(); i++)
        {
            SelectSequenceButton newButton = Instantiate(buttonPrefab, buttonParent);
            newButton.Initialize(gameData, i);
        }

        scrollRect.verticalNormalizedPosition = 1;
    }


    public void GoBack()
    {
        gameData.GetMenuManager().OpenMainMenu();
    }
}