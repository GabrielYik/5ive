﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour {
    public virtual float visibleDuration { get; }

    protected static Image messageImage;
    protected static Text messageText;
    protected Coroutine currentCoroutine;

    protected static float startTime;

    protected bool hasVisibleMessage;
    protected static bool hasNewMessage;
    protected bool doesFileAlreadyExist;

    protected void Start() {
        messageImage = GetComponentInChildren<Image>();
        // Make the message image invisible
        messageImage.enabled = false;
        messageText = GetComponentInChildren<Text>();
    }

    public static void Send(IMessage message) {
        SetUp();
        messageText.text = message.text;
        hasNewMessage = true;
    }

    // Makes the notification disappear after a certain duration
    protected virtual IEnumerator Disappear() {
        hasVisibleMessage = true;

        float difference = Time.time - startTime;
        while (difference < visibleDuration) {
            yield return null;
        }

        CleanUp();
    }

    protected static void SetUp() {
        // Make the notification box visible
        messageImage.enabled = true;
        startTime = Time.time;
    }

    protected void CleanUp() {
        // Make the notification box invisible
        messageImage.enabled = false;
        // Clear the notification box of any messageTexts
        messageText.text = System.String.Empty;
        hasVisibleMessage = false;
        hasNewMessage = false;
        currentCoroutine = null;
    }
}
