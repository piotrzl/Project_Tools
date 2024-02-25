using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EQItemSlot : MonoBehaviour
{
    public ItemObject item;

    public void InitSlot() 
    {
        for(int i = 0; i < transform.childCount; ++i) 
        {
            if(transform.GetChild(i).TryGetComponent(out ItemObject item)) 
            {
                this.item = item;
                break;
            }
        }
    }
}
