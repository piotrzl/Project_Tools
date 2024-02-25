using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour 
{

    public virtual void Interact(PlayerInteraction interaction)
    {
        Interact();
    }

    public virtual void Interact(bool enable) 
    {

    }

    public virtual void Interact()
    {

    }

    public virtual string GetInteractInformation()
    {
        return "inetract";
    }

    public virtual float GetInteractionTime() 
    {
        return 0f;
    }


}
