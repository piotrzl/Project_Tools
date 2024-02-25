using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWeapon : ItemObject
{
    [Header("Components")]
    [SerializeField] Collider _collider;
    [SerializeField] Rigidbody rb;
    [SerializeField] ParticleSystem shoot;
    [Header("Zoom overide")]
    [SerializeField] Vector3 zoomOffSet;
    [Header("Interaction")]
    [SerializeField] string interactInfo = "Weapon [F]";

    // state
    bool isZoom = false;
    public override void Use(PlayerHands hands, KeyCode key)
    {
        switch (key) 
        {
            case KeyCode.Mouse0:
                Shoot();
                break;
            case KeyCode.R:
                Debug.Log("Reload");
                break;
            case KeyCode.Mouse1:
                Zoom(hands);
                break;
        }
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
        isZoom = false;
    }

    public override string GetInteractInformation()
    {
        return interactInfo;
    }

    void Shoot() 
    {
        shoot.Emit(20);
    }

    void Reload() 
    {
        
    }

    void Zoom(PlayerHands hands) 
    {
        isZoom = !isZoom;
        if (isZoom)
            hands.OveridePlacmentPosition(zoomOffSet);
        else
            hands.OveridePlacmentPosition(PositionOffSet);
        
    }

}
