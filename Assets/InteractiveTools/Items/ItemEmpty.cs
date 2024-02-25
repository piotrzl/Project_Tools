using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEmpty : ItemObject
{
    [Header("Components")]
    [SerializeField] Collider _collider;
    [SerializeField] Rigidbody rb;
    [Header("Interaction")]
    [SerializeField] string interactInfo = "Take [F]";

    public override string GetInteractInformation()
    {
        return interactInfo;
    }

    public override void ToHands()
    {
        gameObject.SetActive(true);
        rb.isKinematic = true;
        _collider.enabled = false;
    }

    public override void ToWorld()
    {
        gameObject.SetActive(true);
        rb.isKinematic = false;
        _collider.enabled = true;
    }


}
