﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class MovementModule
{
    public static void Move(this Rigidbody2D rb2D, Vector2 dir, float speed)
    {
        dir = dir.normalized * speed * Time.fixedDeltaTime;
        rb2D.Move(dir);
    }
    public static void Move(this Rigidbody2D rb2D, Vector2 delta)
    {
        rb2D.MovePosition(rb2D.position + delta);
    }

    public static void Send(this Rigidbody2D rb2D, Vector2 dir, float speed)
    {
        dir = dir.normalized * speed;
        rb2D.Send(dir);
    }
    public static void Send(this Rigidbody2D rb2D, Vector2 velocity)
    {
        rb2D.velocity = velocity;
    }

    public static IEnumerator MoveOverTime(this Rigidbody2D rb2D, Vector2 endingPos, float time)
    {
        // Store the inverse of the time so we can use multiplication, which is more efficient than repeated division
        float inverseTime = 1f / time;

        // Store the starting position of the rigidbody.  We use this to linearly interpolate to the end positiong
        Vector2 startingPos = rb2D.position;

        // This function updates the rigidbody position by linearly interpolating it between the start and ending position
        UnityAction<float> updatePosition = currentTime =>
        {
            rb2D.MovePosition(Vector2.Lerp(startingPos, endingPos, currentTime * inverseTime));
        };

        // Run fixed update for time, where the action updates the rigidbody position
        yield return FixedUpdateForTime(time, updatePosition);
    }

    public static IEnumerator RotateOverTime(this Rigidbody2D rb2D, float amount, float time, RotationDirection direction)
    {
        // Store the inverse time so we can use multiplication, which is more efficient than division
        float inverseTime = 1f / time;

        // Store starting and ending rotation values to interpolate between
        float startingRotation = rb2D.rotation;

        // Initiaize the ending rotation. If rotating clockwise, subtract the rotation amount
        float endingRotation;
        if (direction == RotationDirection.Clockwise)
        {
            endingRotation = startingRotation - amount;
        }
        else
        {
            endingRotation = startingRotation + amount;
        }

        // This function linearly interpolates between starting and ending rotations for the rigidbody
        UnityAction<float> updateRotation = currentTime =>
        {
            rb2D.MoveRotation(Mathf.Lerp(startingRotation, endingRotation, currentTime * inverseTime));
        };

        // Run the fixed update for some time, updating the rigidbody rotation
        yield return FixedUpdateForTime(time, updateRotation);
    }

    public static IEnumerator FixedUpdateForTime(float time, UnityAction<float> update)
    {
        // Current amount of time that the rigidbody has been moving
        float currentTime = 0f;

        // Wait used in the coroutine. Waits for each physics update
        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        while (currentTime < time)
        {
            update.Invoke(currentTime);
            yield return wait;
            currentTime += Time.fixedDeltaTime;
        }
    }
}
