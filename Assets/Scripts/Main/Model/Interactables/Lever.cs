﻿/*  * This script encapsulates the functionality of a lever.  * It requires the gameObject of interest to be dragged to interactable.  */  using System.Collections; using UnityEngine;  public class Lever : MonoBehaviour, IPersistent<LeverData> {      public GameObject interactable;     public float speed;      private Quaternion startRotation;     public Quaternion EndRotation { get; private set; }     public Quaternion OriginalStartRotation { get; private set; }     public Quaternion OriginalEndRotation { get; private set; }      private Coroutine currentCoroutine;     public Direction MovementDirection { get; private set; }      public bool HasSwitchedRotation { get; private set; }     private bool interactableState;     private bool canPullLever;     private bool toResumeRotation;      public bool IsRotating { get; private set; }     private bool hasFinishedRotating;     private bool hasInitialised;      private readonly float angleOfRotation = 90f;     private float startTime;     private float journeyLength;       void Start() {         Vector3 angleDifference = Vector3.back * angleOfRotation;         Vector3 currentAngle = transform.eulerAngles;         Vector3 targetAngle = transform.eulerAngles + angleDifference;          /*          * The variables here are stored in the lever data          * so no need to initialise them again          * Initialising them again also overwrites the previous lever state          */         if (!hasInitialised) {             startRotation = transform.rotation;             Vector3 eulerAngles = transform.eulerAngles;             EndRotation = Quaternion.Euler(eulerAngles + angleDifference);              OriginalStartRotation = startRotation;             OriginalEndRotation = EndRotation;              // Default movement direction initially since all levers start tilting to the left             MovementDirection = Direction.Right;         }          speed = 300f;         journeyLength = Vector3.Distance(targetAngle, currentAngle);          /*          * It is possible that a number of levers control          * a gameObject in unison. In that case, no gameObject          * will be attached as an interactable.          * The InteractionManager will manage their collective          * interaction.          */         if (interactable) {             interactableState = interactable.activeSelf;         }     }      /*       * If the player is within the collider boundaries of the lever      * and the R key is pressed, the lever will rotate and      * and the interactable gameObject will disappear.     */     void Update() {         if (LeverIsPulled()) {             // Start time for the rotation coroutine             startTime = Time.time;              if (IsRotating) {                 InterruptRotation();                  // Set the start rotation as the current rotation                 startRotation = transform.rotation;                 SetEndRotation();                 StartRotation();             } else {
                IsRotating = true;                 StartRotation();             }         }          // Resuming rotation from a saved game         if (toResumeRotation) {             ResumeRotation();             StartRotation();         }     }      private void StartRotation() {         currentCoroutine = StartCoroutine(Rotate());     }      private bool LeverIsPulled() {         return canPullLever && Input.GetKeyUp(KeyCode.R) && !PauseLevel.IsPaused;     }      private void InterruptRotation() {         StopCoroutine(currentCoroutine);         // Set the lever to rotate in the opposite direction         ChangeMovementDirection();     }      private void SetEndRotation() {         // Set the end rotation, depending on the movement direction         EndRotation = MovementDirection == Direction.Left ? OriginalStartRotation : OriginalEndRotation;     }      // When the player enters the lever     private void OnTriggerEnter2D(Collider2D collision) {         if (collision.gameObject.CompareTag("Player")) {             canPullLever = true;         }     }      // When the player leaves the lever     private void OnTriggerExit2D(Collider2D collision) {         if (collision.gameObject.CompareTag("Player")) {             canPullLever = false;         }     }      private IEnumerator Rotate() {         float fracJourney = 0;         // The lever cannot be both rotating and have its rotation switched simultaneously         HasSwitchedRotation = false;          while (fracJourney < 1) {             if (PauseLevel.IsPaused) {                 // Cache the current startRotation so that the rotation can be resumed when unpaused                 startRotation = transform.rotation;                 // Refresh the start time to get accurate distance covered                 startTime = Time.time;                 yield return null;             }              float distCovered = (Time.time - startTime) * speed;             fracJourney = distCovered / journeyLength;             transform.rotation = Quaternion.Slerp(startRotation, EndRotation, fracJourney);             yield return null;         }          IsRotating = false;         ChangeInteractableState();         ChangeHasSwitchedRotation();          ChangeMovementDirection();         RefreshStartEndRotations();     }      private void RefreshStartEndRotations() {         if (MovementDirection == Direction.Left) {             startRotation = OriginalEndRotation;             EndRotation = OriginalStartRotation;         } else {             startRotation = OriginalStartRotation;             EndRotation = OriginalEndRotation;         }     }      // Controls the interactable assigned to the lever     private void ChangeInteractableState() {         if (interactable) {             interactableState = !interactableState;             interactable.SetActive(interactableState);         }     }      private void ChangeMovementDirection() {         MovementDirection = MovementDirection == Direction.Right ? Direction.Left : Direction.Right;     }      private void ChangeHasSwitchedRotation() {         HasSwitchedRotation = MovementDirection == Direction.Right ? true : false;     }      public LeverData Save() {         return new LeverData(this);     }      public void Restore(LeverData leverData) {         startRotation = leverData.PrevStartRotation;         EndRotation = leverData.PrevEndRotation;         OriginalStartRotation = leverData.OriginalStartRotation;         OriginalEndRotation = leverData.OriginalEndRotation;         MovementDirection = leverData.MovementDirection;         HasSwitchedRotation = leverData.HasSwitchedRotation;         IsRotating = leverData.IsRotating;         hasInitialised = true;          if (IsRotating) {             toResumeRotation = true;         }          if (HasSwitchedRotation) {             SwitchRotation();         }     }      private void ResumeRotation() {         IsRotating = true;         // Refresh the start time for the rotation coroutine         startTime = Time.time;         toResumeRotation = false;     }      private void SwitchRotation() {         transform.rotation = startRotation;         ChangeInteractableState();     } }  