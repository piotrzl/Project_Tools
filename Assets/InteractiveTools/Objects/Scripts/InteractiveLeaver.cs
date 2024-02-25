using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveLeaver : InteractiveObject
{
    [Header("Main")]
    [SerializeField] Transform leaverTransform;
    [Header("Interaction")]
    [SerializeField] float InteractionTime = 0.5f;
    [SerializeField] string infoOnOn = "Turn On";
    [SerializeField] string infoOnOff = "Turn On";
    [Header("Move Stats")]
    [SerializeField] float speed = 50f;
    [SerializeField] float angleOn = 45f;
    [SerializeField] float angleOff = -45f;
    [Header("Delay")]
    [SerializeField] bool useDelay = false;
    [SerializeField] float delay = 0;
    [Header("State")]
    bool onMove = false;
    bool isOn = false;
    Quaternion targetRotarion;
    [Header("Events")]
    [SerializeField] UnityEvent<bool> LeaverEvent;


    public override void Interact()
    {
        if (isOn)
            LeaverOff();
        else
            LeaverOn();

        if (!onMove)
            StartCoroutine(LeaverMove());
    }


    void LeaverOn() 
    {
        isOn = true;
        targetRotarion = Quaternion.Euler(angleOn, leaverTransform.localRotation.y, leaverTransform.localRotation.z);
    }

    void LeaverOff() 
    {
        isOn = false;
        targetRotarion = Quaternion.Euler(angleOff, leaverTransform.localRotation.y, leaverTransform.localRotation.z);
    }

   

    IEnumerator LeaverMove()
    {
        onMove = true;

        while (onMove)
        {
            leaverTransform.localRotation = Quaternion.RotateTowards(leaverTransform.localRotation, targetRotarion, speed * Time.deltaTime);

            if (Quaternion.Angle(leaverTransform.localRotation, targetRotarion) <= 0.000001)
                onMove = false;


            yield return null;
        }

        if (useDelay)
            yield return new WaitForSeconds(delay);

        LeaverEvent?.Invoke(isOn);
    }

    public override string GetInteractInformation()
    {
        if (isOn)
            return infoOnOn;
        else
            return infoOnOff;
    }

    public override float GetInteractionTime()
    {
        return InteractionTime;
    }
}
