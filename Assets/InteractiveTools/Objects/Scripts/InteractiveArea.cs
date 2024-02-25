using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class InteractiveArea : InteractiveObject
{

    [SerializeField] GameObject targetObject;
    [SerializeField] bool useOneTime = true;
    [Header("Use Trigger")]
    [SerializeField] bool useOnEnter = true;
    [SerializeField] bool useOnExit = false;
    [SerializeField] bool useOnStay = false;
    [Header("State")]
    [SerializeField] bool isUsed = false;
    [Header("Events")]
    [SerializeField] UnityEvent AreaEvent;

    public override void Interact(PlayerInteraction interaction)
    {

        if (useOneTime || !isUsed)
            return;

        if (targetObject)
        {
            if (interaction.TryLookAtObject(targetObject))
            {
                AreaEvent?.Invoke();
                isUsed = true;
            }
        }
        else
        {
            AreaEvent?.Invoke();
            isUsed = true;
        }

    }


    public void InteractOnEnter(PlayerInteraction interaction) 
    {
        if(useOnEnter && (!useOneTime || !isUsed)) 
        {
            if (targetObject) 
            {
                if(interaction.TryLookAtObject(targetObject))
                {
                    AreaEvent?.Invoke();
                    isUsed = true;
                }
            }
            else 
            {
                AreaEvent?.Invoke();
                isUsed = true;
            }
        }
    }

    public void InteractOnExit(PlayerInteraction interaction)
    {
        if (useOnExit && (!useOneTime || !isUsed))
        {
            if (targetObject)
            {
                if(interaction.TryLookAtObject(targetObject))
                {
                    AreaEvent?.Invoke();
                    isUsed = true;
                }
            }
            else
            {
                AreaEvent?.Invoke();
                isUsed = true;
            }
        }
    }

    public void InteractOnStay(PlayerInteraction interaction)
    {
        if (useOnStay && (!useOneTime || !isUsed))
        {
            if (targetObject)
            {
                if (interaction.TryLookAtObject(targetObject))
                {
                    AreaEvent?.Invoke();
                    isUsed = true;
                }
            }
            else
            {
                AreaEvent?.Invoke();
                isUsed = true;
            }
        }
    }

    public override string GetInteractInformation()
    {
        return "";
    }

    #region Get

    public bool UseOnEnter => useOnEnter;
    public bool UseOnExit => useOnExit;
    public bool UseOnStay => useOnStay;

    #endregion
}
