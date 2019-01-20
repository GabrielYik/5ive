﻿using UnityEngine;

/// <summary>
/// This script is intended to be attached to a parent game object
/// so that it can easily get scripts from its children.
/// </summary>
public class ComponentManager : MonoBehaviour {

	public T GetScript<T>() {
		return GetComponentInChildren<T>();
	}

	public T[] GetScripts<T>() {
		return GetComponentsInChildren<T>();
	}
}
