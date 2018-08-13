﻿/*
 * This script looks a specified scene upon
 * interacting with the attached gameObject.
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickLoadScene : MonoBehaviour {

    public void LoadByIndex(int sceneBuildIndex) {
        //int currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
<<<<<<< HEAD
        if (GameDataManager.HasAdvancedInGame() && gameObject.CompareTag("NewButton")) {
=======
        if (GameDataManager.hasAdvancedInGame && gameObject.CompareTag("NewButton")) {
>>>>>>> 3ae47c30b94e45541d67e8b8ee46f01a173c2acb
            if (GameDataManager.HasUnlockedNewLevelWithoutSaving()) { // A new level has been unlocked but not saved
                SceneManager.LoadScene(GameDataManager.lastUnlockedLevel);

            } else { // No new levels have been unlocked, and we resume at the current level
                string saveFilePath = LevelFile.ConvertToPath(GameDataManager.lastSavedFileName, true);
                LevelData levelData = LevelFile.Deserialise<LevelData>(saveFilePath);
                // Load the scene of the saved game
                SceneManager.LoadScene(levelData.GetSceneBuildIndex());
                // Cache the levelData reference in restoreGame
                RestoreLevel.restoreLevel.Cache(levelData);
            }

        } else {
            SceneManager.LoadScene(sceneBuildIndex);
        }
    }
}
