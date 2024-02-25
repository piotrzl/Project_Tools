using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : InteractiveObject
{
    public bool autoRotate = true;
    public Vector3 PositionOffSet = Vector3.zero;
   


    public override void Interact(PlayerInteraction interaction)
    {
        interaction.SetInteraction(this);
    }


    public virtual void Use(PlayerHands hands, KeyCode key) 
    {

    }

    public virtual void ToEq() 
    {
        gameObject.SetActive(false);
    }

    public virtual void ToHands() 
    {
        gameObject.SetActive(true);
    }

    public virtual void ToWorld() 
    {
        gameObject.SetActive(true);
    }
}
