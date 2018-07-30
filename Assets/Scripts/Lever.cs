﻿/*
 * This script encapsulates the functionality of a lever.
 * It requires the gameObject of interest to be dragged to interactable.
 */

using System.Collections;
using UnityEngine;

public class Lever : MonoBehaviour {

    public GameObject interactable;

    private PauseScene pauseScene;

    private Quaternion startRotation;
    private Quaternion endRotation;
    private Quaternion prevRotation;
    private Quaternion prevEndRotation;

    private Vector3 currentAngle;
    private Vector3 targetAngle;
    private float angleOfRotation;

    private bool interactableState;
    private bool hasEnteredTrigger;
    private bool toResume;
    private bool hasSwitchedRotation;
    private bool isRotating;
    private bool hasFinishedRotating;

    private float startTime;
    private float journeyLength;
    private float speed;

    /*
     * Awake() is used instead of Start() to allow the lever
     * to be rotated by Restore() in InteractablesData
     * to its previous state when a new game is loaded
     * */
    void Awake() {
        /*
         * It is possible that a number of levers control
         * a gameObject in unison. In that case, no gameObject
         * will be attached as an interactable.
         * The InteractionManager will manage their collective
         * interaction.
         */
        if (interactable != null) {
            interactableState = interactable.activeSelf;
        }
        angleOfRotation = 90f;

        pauseScene = GameObject.FindGameObjectWithTag("Pause").GetComponent<PauseScene>();

        Vector3 angleDifference = Vector3.back * angleOfRotation;
        currentAngle = transform.eulerAngles;
        targetAngle = transform.eulerAngles + angleDifference;

        startRotation = transform.rotation;
        Vector3 eulerAngles = transform.eulerAngles;
        endRotation = Quaternion.Euler(eulerAngles + angleDifference);
    }

    void Start() {
        speed = 300f;
        journeyLength = Vector3.Distance(targetAngle, currentAngle);
    }

    /* 
     * If the player is within the collider boundaries of the lever
     * and the R key is pressed, the lever will rotate and
     * and the interactable gameObject will disappear.
    */
    void Update() {
        if (hasEnteredTrigger && Input.GetKeyUp(KeyCode.R)) {
            startTime = Time.time;
            isRotating = true;
            StartCoroutine(Rotate());
        }

        if (toResume) {
            isRotating = true;
            StartCoroutine(Rotate());
        }
    }

    // When the player enters the lever
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            hasEnteredTrigger = true;
        }
    }

    // When the player leaves the lever
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            hasEnteredTrigger = false;
        }
    }

    private IEnumerator Rotate() {
        float distCovered;
        float fracJourney = 0;
        Quaternion start;
        Quaternion end;

        if (toResume) {
            start = prevRotation;
            end = prevEndRotation;
        } else {
            start = startRotation;
            end = endRotation;
        }

        while (fracJourney < 1) {
            if (!pauseScene.IsScenePaused()) {
                // Distance moved = time * speed.
                distCovered = (Time.time - startTime) * speed;

                // Fraction of journey completed = current distance divided by total distance.
                fracJourney = distCovered / journeyLength;

                // Set our position as a fraction of the distance between the markers.
                transform.rotation = Quaternion.Slerp(start, end, fracJourney);
            } else {
                startTime = Time.time;
            }
            yield return null;
        }

        if (interactable) {
            ChangeInteractableState();
        }

        if (toResume) {
            toResume = false;
        }

        Quaternion temp = startRotation;
        startRotation = endRotation;
        endRotation = temp;

        hasSwitchedRotation = !hasSwitchedRotation;
        isRotating = false;
    }

    // Allow the rotation to resume
    public void ResumeRotation(Quaternion prevRotation, Quaternion prevEndRotation) {
        toResume = true;
        startTime = Time.time;
        this.prevRotation = prevRotation;
        this.prevEndRotation = prevEndRotation;
    }

    public void SwitchRotation() {
        transform.rotation = endRotation;
        ChangeInteractableState();
        hasSwitchedRotation = true;
    }

    // Controls the interactable assigned to the lever
    private void ChangeInteractableState() {
        interactable.SetActive(!interactableState);
        interactableState = !interactableState;
    }

    public bool HasSwitchedRotation() {
        return hasSwitchedRotation;
    }

    public LeverData CacheData() {
        return new LeverData(transform.rotation, endRotation, hasSwitchedRotation, isRotating);
    }

}

