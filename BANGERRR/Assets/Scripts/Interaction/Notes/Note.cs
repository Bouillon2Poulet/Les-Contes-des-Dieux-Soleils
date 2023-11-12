using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Note : MonoBehaviour
{
    private GameObject noteSprite;

    [Header("Rotation")]
    public float rotationSpeed = 100f;
    private float angle = 0f;

    [Header("Up and Down")]
    public float amplitude = .1f;
    public float frequency = .5f;
    private Vector3 startPos;

    private void FixedUpdate()
    {
        IdleRotateNote();
        IdleMoveNote();
    }

    private void IdleRotateNote()
    {
        angle += Time.fixedDeltaTime * rotationSpeed;
        noteSprite.transform.localRotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void IdleMoveNote()
    {
        float verticalOffset = amplitude * Mathf.Sin(Time.time * 2 * Mathf.PI * frequency);
        noteSprite.transform.localPosition = startPos + new Vector3(0f, verticalOffset, 0f);
    }

    private void Start()
    {
        noteSprite = GetComponentInChildren<SpriteRenderer>().gameObject;

        startPos = noteSprite.transform.localPosition;
    }
}
