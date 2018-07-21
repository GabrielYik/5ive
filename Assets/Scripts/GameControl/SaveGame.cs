﻿/*
 * This script is used to save the game.
 * It is called when the user presses enter on the attached InputField gameObject.
 */

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class SaveGame : MonoBehaviour {

    private InputField inputField;
    private String saveFilePath;
    private GameObject player;
    private GameObject ball;
    private FileButtonManager buttonManager;

    private void Start() {
        inputField = GetComponent<InputField>();
        player = GameObject.FindWithTag("Player");
        ball = GameObject.FindWithTag("TeleportationBall");
        // Fragile code since it breaks when the hierarchy changes
        buttonManager =
            transform.parent.parent.GetComponentInChildren<FileButtonManager>();
    }

    // Serialise the game data and save it into a file as named by the user
    public void Save() {
        if (!inputField.text.Equals(System.String.Empty)) {
            // Get the current scene
            Scene scene = SceneManager.GetActiveScene();
            // Cache the player data
            PlayerData playerData = CachePlayerData();
            // Cache the ball data
            BallData ballData = CacheBallData();
            // Cache the state of interactables
            InteractablesData interactablesData = CacheInteractablesData();
            // Package the game data into a LevelData instance
            LevelData levelData = new LevelData(scene, playerData, ballData, interactablesData);

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            // Directory to save file into
            Directory.CreateDirectory(GameFile.GetSaveDirectoryPath());
            // Path of the file to be used for saving
            saveFilePath = GameFile.ConvertToPath(GameFile.AddTag(inputField.text));
            FileStream fileStream = File.Create(saveFilePath);
            // Clear the input field
            inputField.text = System.String.Empty;

            // Serialise levelData
            binaryFormatter.Serialize(fileStream, levelData);
            fileStream.Close();

            buttonManager.UpdateButtons();
        }
    }

    private PlayerData CachePlayerData() {
        Vector2 velocity = player.GetComponent<Rigidbody2D>().velocity;
        Vector3 position = player.transform.position;
        return new PlayerData(velocity, position);
    }

    private BallData CacheBallData() {
        Vector2 velocity = ball.GetComponent<Rigidbody2D>().velocity;
        Vector3 position = ball.transform.position;
        return new BallData(velocity, position);
    }

    private InteractablesData CacheInteractablesData() {
        InteractablesData interactablesData = new InteractablesData();
        interactablesData.ScreenShot();
        return interactablesData;
    }
}
