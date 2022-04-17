//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
// ---------------------------------- serialized for debug
// ---data members---
// ---getters---
// ---setters---
// ---constructors---
// ---unity methods---
// ---primary methods---

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// this script helps manage startup of a scene.
//      it's primary purpose is to allow loading the game from scenes other than the start up scene.
//      therefore, all instances of this script should be removed from scenes other than the startup
//      scene in consumer facing builds

public class SceneStarter : MonoBehaviour
{
    // ---data members---
    [SerializeField] private GameObject gameControllerPrefab;
    [SerializeField] private GameObject gameDataPrefab;

    private GameController gameController;
    private GameData gameData;

    // ---getters---
    private GameController GetGameController() { return gameController; }
    private GameData GetGameData() { return gameData; }

    // ---setters---
    private void SetGameController(GameController newGameController) { gameController = newGameController; }
    private void SetGameData(GameData newGameData) { gameData = newGameData; }

    // ---unity methods---

    private void Awake()
    {
        // subscribe delegate to scene loader for detecting scene changes
        SceneManager.sceneLoaded += OnLevelLoaded;

        // find references for or instantiate game controller and game data objects and scripts
        GameObject gameControllObj = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllObj == null)
        {
            gameControllObj = Instantiate(gameControllerPrefab);
        }
        SetGameController(gameControllObj.GetComponent<GameController>());

        GameObject gameDataObj = GameObject.FindGameObjectWithTag("GameData");
        if (gameDataObj == null)
        {
            gameDataObj = Instantiate(gameDataPrefab);
        }
        SetGameData(gameDataObj.GetComponent<GameData>());

        // Initialize game controller and data
        gameData.Initialize(GetGameController());
        gameController.Initialize(GetGameData());

        // start scene initialization
        InitializeScene();
    }

    private void OnApplicationQuit()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

    // ---primary methods---

    // start initialization process for the appropriate scene.
    //      default means it is a stage being loaded
    private void InitializeScene()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "StartUp":
                break;
            case "MainMenu":
                GameObject mainMenuContObj = GameObject.Find("Main Menu Controller");
                mainMenuContObj.GetComponent<MainMenuController>().Initialize(GetGameController());
                break;
            default:
                GameObject stageContObj = GameObject.FindGameObjectWithTag("Stage Controller");
                stageContObj.GetComponent<StageController>().Initialize(GetGameController());
                break;
        }
    }

    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeScene();
    }
}

