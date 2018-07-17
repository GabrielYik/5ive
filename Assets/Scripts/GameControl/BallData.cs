﻿/*
 * This class represents a ball and is used to restore
 * a ball to its saved position and velocity in the saved game.
 */

using System;
using UnityEngine;

[Serializable]
public class BallData {

    // Vector2 velocity
    private float vX;
    private float vY;

    // Vector3 position
    private float pX;
    private float pY;
    private float pZ;

    public BallData(Vector2 velocity, Vector3 position) {
        this.vX = velocity.x;
        this.vY = velocity.y;

        this.pX = position.x;
        this.pY = position.y;
        this.pZ = position.z;
    }

    // Reconstruct the velocity
    public Vector2 GetVelocity() {
        return new Vector2(vX, vY);
    }

    // Reconstruct the position
    public Vector3 GetPosition() {
        return new Vector3(pX, pY, pZ);
    }
}