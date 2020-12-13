﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private Transform target = null;

    [SerializeField]
    private Vector3 offset = new Vector3(0, 10, -10);

    [SerializeField]
    private Vector3 lookatOffset = new Vector3(0, 1, 0);

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private bool lookat = false;

    [SerializeField]
    private bool instantPosition = false;

    void FixedUpdate()
    {
        if (!target)
        {
            Debug.LogWarning("No Target");
            return;
        }

        if (instantPosition)
            transform.position = target.position + offset;
        else
            transform.position = Vector3.Lerp(transform.position, target.position + offset, speed * Time.deltaTime);

        if (lookat)
            transform.LookAt(target.transform.position + lookatOffset);
    }

    public void TakeTarget(Transform newTarget)
    {
        target = newTarget;
    }
    public void NewOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }
}
