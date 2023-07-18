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
    public float amplitude = .005f;
    public float frequency = .05f;
    private Vector3 startPos;

    private void FixedUpdate()
    {
        IldeRotateNote();
        IldeMoveNote();
    }

    private void IldeRotateNote()
    {
        angle += Time.fixedDeltaTime * rotationSpeed;
        noteSprite.transform.localRotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void IldeMoveNote()
    {
        float verticalOffset = amplitude * Mathf.Sin(Time.time * 2 * Mathf.PI * frequency);
        transform.localPosition = startPos + new Vector3(0f, verticalOffset, 0f);
    }

    private void Awake()
    {
        noteSprite = GetComponentInChildren<Transform>().gameObject;
        startPos = transform.localPosition;
    }
}
