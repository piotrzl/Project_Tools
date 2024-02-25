using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

public class InteractiveLamp : InteractiveObject
{
    [Header("Main")]
    [SerializeField] Light _light;
    [SerializeField] MeshRenderer _renderer;
    [Header("Interaction")]
    [SerializeField] float InteractionTime = -1f;
    [SerializeField] string MaterialKeyWordToEnable = "_EMISSION";





    public override void Interact()
    {
        _light.enabled = !_light.enabled;
        EnableMaterial();
    }

    public override void Interact(bool enable)
    {
        _light.enabled = enable;
        EnableMaterial();
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

    public override float GetInteractionTime()
    {
        return InteractionTime;
    }
}
