﻿using Main._5ive.Commons;
using UnityEngine;

namespace Main._5ive.Model.Story.Clues {

    public class T : MonoBehaviour {

        private SpriteRenderer spriteRenderer;

        void Start() {
            spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag(Tags.Ball)) {
                spriteRenderer.enabled = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if (collision.CompareTag(Tags.Ball)) {
                spriteRenderer.enabled = false;
            }
        }
    }

}