using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempVac : MonoBehaviour
{
    public InputActionReference gripInputActionReference;
    public InputActionReference triggerInputActionReference;

    public GameObject isVacuuming;
    public GameObject isFlashing;

    private float _gripValue;
    private float _triggerValue;

    private void OnEnable()
    {
        gripInputActionReference.action.performed += OnGripPerformed;
        triggerInputActionReference.action.performed += OnTriggerPerformed;
        gripInputActionReference.action.canceled += OnGripCanceled;
        triggerInputActionReference.action.canceled += OnTriggerCanceled;
    }

    private void OnDisable()
    {
        gripInputActionReference.action.performed -= OnGripPerformed;
        triggerInputActionReference.action.performed -= OnTriggerPerformed;
        gripInputActionReference.action.canceled -= OnGripCanceled;
        triggerInputActionReference.action.canceled -= OnTriggerCanceled;
    }

    private void OnGripPerformed(InputAction.CallbackContext context)
    {
        _gripValue = context.ReadValue<float>();
        // Call your method here for grip action
        HandleGripAction(_gripValue);
    }

    private void OnTriggerPerformed(InputAction.CallbackContext context)
    {
        _triggerValue = context.ReadValue<float>();
        // Call your method here for trigger action
        HandleTriggerAction(_triggerValue);
    }

    private void OnGripCanceled(InputAction.CallbackContext context)
    {
        // Handle grip release if needed
        HandleGripAction(0);
    }

    private void OnTriggerCanceled(InputAction.CallbackContext context)
    {
        // Handle trigger release if needed
        HandleTriggerAction(0);
    }

    private void HandleGripAction(float value)
    {
        // Handles the Flashing
        if (value > 0) {
            isFlashing.SetActive(true);
        }
        else
        {
            isFlashing.SetActive(false);
        }
    }

    private void HandleTriggerAction(float value)
    {
        // YHandles the Vacuuming
        if (value > 0)
        {
            isVacuuming.SetActive(true);
        }
        else
        {
            isVacuuming.SetActive(false);
        }


    }
}
