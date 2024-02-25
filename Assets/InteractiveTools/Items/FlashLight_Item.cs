using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FlashLight_Item : ItemObject
{
    [Header("Components")]
    [SerializeField] Collider _collider;
    [SerializeField] Rigidbody rb;
    [SerializeField] Light _light;
    [Header("Light Stats")]
    [SerializeField] MeshRenderer _renderer;
    [SerializeField] string MaterialKeyWordToEnable = "_EMISSION";
    [Header("Interaction")]
    [SerializeField] string interactInfo = "Flash light [F]";
    public override void Use(PlayerHands hands, KeyCode key)
    {
        if (key == KeyCode.E)
        {
            _light.enabled = !_light.enabled;
            EnableMaterial();
        }
    }

    void EnableMaterial()
    {
        if (_light.enabled)
        {

            if (_renderer != null && _renderer.material != null)
                _renderer.material.EnableKeyword(MaterialKeyWordToEnable);
        }
        else
        {
            if (_renderer != null && _renderer.material != null)
                _renderer.material.DisableKeyword(MaterialKeyWordToEnable);
        }

    }

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
