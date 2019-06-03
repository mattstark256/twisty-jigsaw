using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(GameData))]
public class MenuManager : MonoBehaviour
{
    private GameData gameData;

    [SerializeField]
    private RectTransform menuUIParent;
    [SerializeField]
    private MainMenu mainMenuPrefab;
    [SerializeField]
    private SelectSequenceMenu selectSequenceMenuPrefab;
    [SerializeField]
    private SelectPuzzleMenu selectPuzzleMenuPrefab;
    [SerializeField]
    private ConfirmMenu confirmMenuPrefab;
    [SerializeField]
    private SettingsMenu settingsMenuPrefab;

    private MenuScreen currentMenuScreen;


    private void Awake()
    {
        gameData = GetComponent<GameData>();
    }


    private void Start()
    {
        OpenMainMenu();
    }


    public void CloseMenu()
    {
        if (currentMenuScreen != null) { Destroy(currentMenuScreen.gameObject); }
        currentMenuScreen = null;
    }


    public void OpenMainMenu()
    {
        SwitchToScreen(mainMenuPrefab);
    }


    public void OpenSequenceSelect()
    {
        SwitchToScreen(selectSequenceMenuPrefab);
    }


    public void OpenPuzzleSelect(int sequenceIndex)
    {
        SwitchToScreen(selectPuzzleMenuPrefab);
        ((SelectPuzzleMenu)currentMenuScreen).InitializeForSequence(sequenceIndex);
    }


    public void OpenConfirmMenu()
    {
        SwitchToScreen(confirmMenuPrefab);
    }


    public void OpenSettingsMenu()
    {
        SwitchToScreen(settingsMenuPrefab);
    }


    private void SwitchToScreen(MenuScreen menuScreenPrefab)
    {
        if (currentMenuScreen != null) { Destroy(currentMenuScreen.gameObject); }
        currentMenuScreen = Instantiate(menuScreenPrefab, menuUIParent);
        currentMenuScreen.Initialize(gameData);
    }
}

