using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveButton : InteractiveObject
{
    [Header("Main")]
    [SerializeField] Transform buttonTransform;

    [Header("Interaction")]
    [SerializeField] string interactionInfo = "Press Button";
    [SerializeField] float InteractionTime = 0.2f;
    [Header("Press stats")]
    [SerializeField][Min(0.0001f)] float pressTime = 0.5f;
    float unPressYPosition = 0f;
    [SerializeField][Min(0f)] float pressYPosition = 0.05f;
    [SerializeField] bool useDelay = false;
    [SerializeField][Min(0f)] float delay = 0f;
    [Header("State")]
    [SerializeField] bool isPres;
    [Header("Events")]
    [SerializeField] UnityEvent ButtonEvent;


    public override void Interact()
    {
        if (!isPres)
        {
            unPressYPosition = buttonTransform.localPosition.y;
            StartCoroutine(PressButton());
            isPres = true;
        }
    }

    IEnumerator PressButton()
    {
        float pressSpeed = (-pressYPosition - buttonTransform.localPosition.y) / pressTime;
        // go down
        while (!Mathf.Approximately(buttonTransform.localPosition.y, -pressYPosition))
        {
            float newPosition = Mathf.Clamp(buttonTransform.localPosition.y + pressSpeed * Time.deltaTime, -pressYPosition, unPressYPosition);
            buttonTransform.localPosition = new Vector3(buttonTransform.localPosition.x, newPosition, buttonTransform.localPosition.z);

            yield return null;
        }

        if (useDelay)
            yield return new WaitForSeconds(delay);
        else
            yield return null;

        ButtonEvent?.Invoke();

        pressSpeed = (unPressYPosition - buttonTransform.localPosition.y) / pressTime;
        // go up
        while (!Mathf.Approximately(buttonTransform.localPosition.y, unPressYPosition))
        {
            float newPosition = Mathf.Clamp(buttonTransform.localPosition.y + pressSpeed * Time.deltaTime, -pressYPosition, unPressYPosition);
            buttonTransform.localPosition = new Vector3(buttonTransform.localPosition.x, newPosition, buttonTransform.localPosition.z);

            yield return null;
        }

        isPres = false;

    }

    public override string GetInteractInformation()
    {
        return interactionInfo;
    }

    public override float GetInteractionTime()
    {
        return InteractionTime;
    }
}
