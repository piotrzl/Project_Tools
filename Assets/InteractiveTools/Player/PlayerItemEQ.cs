using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerItemEQ : MonoBehaviour
{
    [SerializeField] EQItemSlot[] eqSlots = new EQItemSlot[0];

    public int SlotCount => eqSlots.Length;

    void Start()
    {
        for(int i = 0; i < eqSlots.Length; ++i) 
        {
            eqSlots[i].InitSlot();
            if (eqSlots[i].item)
                eqSlots[i].item.ToEq();
        }
    }

    public bool TryAddItemToSlot(ItemObject item,int toSlot) 
    {
        if ((0 > toSlot || toSlot >= eqSlots.Length) || eqSlots[toSlot].item)
        {
            return false;
        }
        eqSlots[toSlot].item = item;
        item.transform.SetParent(eqSlots[toSlot].transform);
        item.ToEq();
        return true;
    }

    public bool TryTakeItemFormSlot(int slot, out ItemObject item) 
    {
        if ((0 > slot || slot >= eqSlots.Length) || !eqSlots[slot].item)
        {
            item = null;
            return false;
        }

        item = eqSlots[slot].item;
        eqSlots[slot].item = null;
        return true;
    }

    public bool SlotIsFree(int slot)
    {
        if ((0 > slot || slot >= eqSlots.Length) || eqSlots[slot].item)
        {
            return false;
        }

        return true;
    }

    public int FindFreeSlot() 
    {
        for(int i = 0; i < eqSlots.Length; ++i) 
        {
            if (!eqSlots[i].item) 
            {
                return i;
            }
        }

        return -1;
    }


}
