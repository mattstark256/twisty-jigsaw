using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour
{
    //[SerializeField]
    //private SceneVariable mainMenuScene;

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevel()
    {
        Debug.Log("loading next level");

        //int currentLevelIndex = PersistentManagers.instance.GetLevelSequence().GetLevelIndex(SceneManager.GetActiveScene().name);
        //if (PersistentManagers.instance.GetLevelSequence().IsLastLevel(currentLevelIndex))
        //{
        //    SceneManager.LoadScene(mainMenuScene.name);
        //}
        //else
        //{
        //    SceneManager.LoadScene(PersistentManagers.instance.GetLevelSequence().GetLevel(currentLevelIndex + 1).name);
        //}
    }
}
