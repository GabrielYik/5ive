﻿using UnityEngine;
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

	private SaveMenu saveMenu;

	private static Logger logger = Logger.GetLogger();

	private void Start() {
		saveMenu = GetComponentInParent<SaveMenu>();
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
		logger.LogLow("User entered " + fileName);

		bool didUserInputSomething = !fileName.Equals(string.Empty);
		if (!didUserInputSomething) {
			return;
		}

		if (saveMenu.DoesFileWithSameNameExist(fileName)) {
			NotificationManager.Send(new FileAlreadyExistsMessage());
			return;
		}

		Game.instance.SaveLevel(fileName);
		logger.LogHigh("Level saved");

		saveMenu.AddButton();
		logger.LogHigh("Buttons updated");
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