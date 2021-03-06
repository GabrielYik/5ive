﻿using System.Collections;
using Main._5ive.Model.Story.Messages;
using UnityEngine;
using UnityEngine.UI;

namespace Main._5ive.Model.Story.StoryLines {

    /// <summary>
    /// This script manages the sending of notifications.
    /// </summary>
    public class StoryLineManager : MessageManager {
        private static Image messageImage;

        private static Text messageText;

        private static bool hasNewMessage;

        private static float startTime;

        public override float VisibleDuration {
            get { return 5f; }
        }

        private void Start() {
            messageImage = GetComponentInChildren<Image>();

            // Make the message image invisible
            messageImage.enabled = false;
            messageText = GetComponentInChildren<Text>();
        }

        public static void Send(IMessage message) {
            SetUp();
            messageText.text = message.Text;
            hasNewMessage = true;
        }

        // Makes the notification disappear after a certain duration
        protected IEnumerator Disappear() {
            hasVisibleMessage = true;
            float difference = 0;

            while (difference < VisibleDuration) {
                difference = Time.time - startTime;
                yield return null;
            }

            CleanUp();
        }

        private void Update() {
            if (hasNewMessage) {
                if (hasVisibleMessage) {
                    InterruptDisappearance();
                }

                StartTimerToDisappearance();
            }
        }

        private void StartTimerToDisappearance() {
            currentCoroutine = StartCoroutine(Disappear());
            hasNewMessage = false;
        }

        private void InterruptDisappearance() {
            StopCoroutine(currentCoroutine);
            hasNewMessage = false;
        }

        private static void SetUp() {
            // Make the notification box visible
            messageImage.enabled = true;
            startTime = Time.time;
        }

        private void CleanUp() {
            // Make the notification box invisible
            messageImage.enabled = false;

            // Clear the notification box of any messageTexts
            messageText.text = System.String.Empty;
            hasVisibleMessage = false;
            hasNewMessage = false;
            currentCoroutine = null;
        }
    }

}