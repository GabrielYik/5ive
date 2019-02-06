﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This script represents an <see cref="InputField"/> in the
/// in game save menu that takes in file name and saves the
/// current level progress into a file named as such.
/// </summary>
/// <remarks>
/// The method that implements this function, <see cref="SaveLevel()"/>,
/// is caled when the user presses the return key in the input field.
/// </remarks>
public class SavingEnabledInputField : MonoBehaviour {

	private InputField inputField;

	private Menu parentMenu;

	private Level level;

	private void Start() {
		//TODO
		parentMenu = GetComponent<Menu>();
		level = GetComponent<Level>();
	}

	/// <summary>
	/// Saves the level.
	/// </summary>
	/// <remarks>
	/// This is a wrapper function over the overloaded core <code>SaveLevel#Save(...)</code>.
	/// If nothing is entered in the input field, the function does nothing.
	/// </remarks>
	public void SaveLevel() {
		string fileName = inputField.text;

		bool didUserInputSomething = !fileName.Equals(string.Empty);
		if (!didUserInputSomething) {
			return;
		}

		if (parentMenu.DoesFileWithSameNameExist(fileName)) {
			NotificationManager.Send(new FileAlreadyExistsMessage());
		} else {
			level.Save();
			parentMenu.UpdateButtons();
		}
		ClearInputField();
	}

	private void ClearInputField() {
		inputField.text = string.Empty;
	}

	private void OnEnable() {
		inputField = GetComponent<InputField>();
		if (inputField != null) {
			inputField.ActivateInputField();
		}
	}
}