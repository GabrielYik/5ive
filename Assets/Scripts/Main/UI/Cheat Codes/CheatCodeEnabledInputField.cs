﻿using UnityEngine;
using UnityEngine.UI;

public class CheatCodeEnabledInputField : MonoBehaviour {

	/// <summary>
	/// The input field in which the cheat codes can be entered.
	/// </summary>
	private InputField inputField;

	private void Start() {
		inputField = GetComponent<InputField>();
	}

	public void RunCheatCodeEntered() {
		string input = inputField.text.ToLower();

		switch (input) {
		case UnlockAllLevelsCheatCode.Code:
			new UnlockAllLevelsCheatCode().Run(inputField);
			break;
		default:
			// Ignore input since not a valid cheat code
			return;
		}
	}
}
