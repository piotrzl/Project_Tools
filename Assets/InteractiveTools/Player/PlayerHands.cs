using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] PlayerControler playerControler;
    [SerializeField] Transform placeItemPoint;
    [Header("Item Holding stats")]
    [SerializeField] LayerMask targetAtMask;
    [SerializeField] float targetInRange = 100f;
    [SerializeField] float targetOutRange = 1000f;
    [SerializeField] float stiffnessPosition = 10f;
    [SerializeField] float stiffnessRotation = 25f;
    [Header("State")]
    public ItemObject currentItem;
    Vector3 placmentOffSetPosition;
    public void UpdateHands() 
    {
        if (!currentItem)
            return;

        Ray ray = new Ray(playerControler.Camera.transform.position, playerControler.Camera.transform.forward);

        if (currentItem.autoRotate && Physics.Raycast(ray, out RaycastHit hit, targetInRange, targetAtMask))
            ItemLookaAT(hit.point);
        else
            ItemLookaAT(playerControler.Camera.transform.position + playerControler.Camera.transform.forward * targetOutRange);
    }

    public bool AddItem(ItemObject item) 
    {
        if (currentItem || !item)
            return false;

        currentItem = item;
        currentItem.transform.SetParent(transform);
        currentItem.ToHands();
        placmentOffSetPosition = currentItem.PositionOffSet;
        return true;
    }


    public void TakeItemFromEQ(PlayerItemEQ playerItemEQ, int fromslot) 
    {
        if(!currentItem) 
        {
            if(playerItemEQ.TryTakeItemFormSlot(fromslot, out ItemObject item)) 
            {
                currentItem = item;
                currentItem.transform.SetParent(transform);
                currentItem.ToHands();
                placmentOffSetPosition = currentItem.PositionOffSet;
            }
        }
    }

    public void SwapItemFromEQ(PlayerItemEQ playerItemEQ, int fromslot) 
    {
        if(!currentItem) 
        {
            TakeItemFromEQ(playerItemEQ, fromslot);
            return;
        }

        if (playerItemEQ.SlotIsFree(fromslot)) 
        {
            PutItemToEQ(playerItemEQ, fromslot);
            return;
        }


        if (playerItemEQ.TryTakeItemFormSlot(fromslot, out ItemObject item))
        {
            PutItemToEQ(playerItemEQ, fromslot);

            AddItem(item);
        }

    }



    public void PutItemToEQ(PlayerItemEQ playerItemEQ, int toSlot) 
    {
        if (currentItem) 
        {
            if(playerItemEQ.TryAddItemToSlot(currentItem, toSlot)) 
            {
                currentItem = null;
            }
        }
    }



    void ItemLookaAT(Vector3 point) 
    {
        Vector3 offsetToLocal = placmentOffSetPosition;
        offsetToLocal = placeItemPoint.right * offsetToLocal.x + placeItemPoint.up * offsetToLocal.y + placeItemPoint.forward * offsetToLocal.z;

        Vector3 targetPosition = Vector3.Lerp(currentItem.transform.position, placeItemPoint.position + offsetToLocal, Time.deltaTime * stiffnessPosition);

        Quaternion targetRotation = Quaternion.LookRotation((point - targetPosition).normalized, placeItemPoint.up);
        targetRotation = Quaternion.Lerp(currentItem.transform.rotation, targetRotation, Time.deltaTime * stiffnessRotation);


        currentItem.transform.SetPositionAndRotation(targetPosition, targetRotation);
    }



    public void DropItem() 
    {
        if (currentItem) 
        {
            currentItem.ToWorld();
            currentItem.transform.SetParent(null);
            currentItem = null;
        }
    }

    public void UseItem(KeyCode keyCode) 
    {
        if (currentItem)
        currentItem.Use(this, keyCode);
    }

    public void OveridePlacmentPosition(Vector3 position) 
    {
        placmentOffSetPosition = position;
    }
}
