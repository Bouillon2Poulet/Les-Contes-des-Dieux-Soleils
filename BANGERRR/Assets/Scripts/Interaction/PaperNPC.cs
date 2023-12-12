using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperNPC : MonoBehaviour, IInteractable
{
    private GameObject noteSprite;

    [Header("Rotation")]
    public float rotationSpeed = 100f;
    private float angle = 0f;

    [Header("Up and Down")]
    public float amplitude = .1f;
    public float frequency = .5f;
    private Vector3 startPos;

    [Header("Paper")]
    public GameObject Paper;
    private bool hasAlreadyChangedStateThisFrame = false;

    private void Update()
    {
        if (hasAlreadyChangedStateThisFrame)
            hasAlreadyChangedStateThisFrame = false;

        IdleRotateNote();
        IdleMoveNote();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Paper.activeSelf && !hasAlreadyChangedStateThisFrame)
            {
                hasAlreadyChangedStateThisFrame = true;
                AudioManager.instance.Play("paper");
                Paper.SetActive(false);
                PlayerStatus.instance.GameMenuCursor(false);
                PlayerStatus.instance.isAnimated = false;
                FindAnyObjectByType<ThirdPersonMovement>().unblockPlayerMoveInputs();
            }
        }
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

        Paper.SetActive(false);
    }

    public void Interact()
    {
        if (!hasAlreadyChangedStateThisFrame)
        {
            hasAlreadyChangedStateThisFrame = true;
            AudioManager.instance.Play("paper");
            bool newState = !Paper.activeSelf;
            Paper.SetActive(newState);
            PlayerStatus.instance.GameMenuCursor(newState);
            PlayerStatus.instance.isAnimated = newState;
            if (newState)
            {
                FindAnyObjectByType<ThirdPersonMovement>().blockPlayerMoveInputs();
            }
            else
            {
                FindAnyObjectByType<ThirdPersonMovement>().unblockPlayerMoveInputs();
            }
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
