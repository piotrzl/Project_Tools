using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractiveDor : InteractiveObject
{
    [Header("Main")]
    [SerializeField] Transform dorTransform;
    //[SerializeField] Collider disableColliderOnMove;
    [Header("Interaction")]
    [SerializeField] float InteractionTime = 0.5f;
    [SerializeField] string infoOnOpen = "close dor";
    [SerializeField] string infoOnClose = "open dor";
    [Header("Move Stats")]
    [SerializeField] float doorSpeed = 100f;
    [SerializeField][Range(0, 180)] float openAngle = 130f;
    [SerializeField][Range(0, 180)] float closeAngle = 0f;
    [SerializeField] bool endStateBeforeChange = false;
    [Header("State")]
    [SerializeField] bool isOpen = false;
    [SerializeField] bool onMove = false;
    Quaternion targetRotarion;




    public override void Interact()
    {
        if (endStateBeforeChange && onMove)
            return;

        if (isOpen)
            CloseDor();
        else
            OpenDor();

        if (!onMove)
        StartCoroutine(DorMove());
    }

    public override void Interact(bool enable)
    {
        if (endStateBeforeChange && onMove)
            return;

        if (enable)
            OpenDor();
        else
            CloseDor();

        if (!onMove)
            StartCoroutine(DorMove());
    }

    void OpenDor() 
    {
        targetRotarion = Quaternion.Euler(dorTransform.localRotation.x, openAngle, dorTransform.localRotation.z);
        isOpen = true;
    }

    void CloseDor() 
    {
        targetRotarion = Quaternion.Euler(dorTransform.localRotation.x, closeAngle, dorTransform.localRotation.z);
        isOpen = false;
    }


    IEnumerator DorMove() 
    {
        onMove = true;

        while (onMove) 
        {

            dorTransform.localRotation = Quaternion.RotateTowards(dorTransform.localRotation, targetRotarion, doorSpeed * Time.deltaTime);

            if (Quaternion.Angle(dorTransform.localRotation, targetRotarion) <= 0.000001)
                onMove = false;


            yield return null;
        }

    }

    public override float GetInteractionTime()
    {
        return InteractionTime;
    }

    public override string GetInteractInformation()
    {
        if (isOpen)
            return infoOnOpen;
        else
            return infoOnClose;

    }

    private void OnDisable()
    {
        onMove = false;
    }

}
